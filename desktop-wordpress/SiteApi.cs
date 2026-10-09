using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Videira {
public sealed class SiteApi : IDisposable {
 public const string BaseUrl="https://lahajlrombmoftevweaf.supabase.co";
 public const string PublicKey="sb_publishable_ixLS7JIsBDtPF75R-rZwcA_2eAD0bKV";
 public const string AdminEmail="videiravinhoteca@gmail.com";
 readonly HttpClient http;
 readonly JavaScriptSerializer json=new JavaScriptSerializer{MaxJsonLength=10000000};
 string access,refresh; DateTime expiry;
 public bool Connected {get{return !String.IsNullOrEmpty(access);}}
 public SiteApi(HttpMessageHandler handler=null){http=handler==null?new HttpClient():new HttpClient(handler);http.Timeout=TimeSpan.FromSeconds(35);ServicePointManager.SecurityProtocol=SecurityProtocolType.Tls12;}
 public async Task<object> Request(string path,string method="GET",object body=null,bool authorized=true){
  if(authorized){if(!Connected)throw new Exception("Entre com a conta administradora do site.");if(DateTime.UtcNow>=expiry)await Refresh();}
  using(var req=new HttpRequestMessage(new HttpMethod(method),BaseUrl+path)){
   req.Headers.Add("apikey",PublicKey);req.Headers.Add("Authorization","Bearer "+(authorized?access:PublicKey));req.Headers.Add("Prefer","return=representation");
   if(body!=null)req.Content=new StringContent(json.Serialize(body),Encoding.UTF8,"application/json");
   using(var response=await http.SendAsync(req)){string text=await response.Content.ReadAsStringAsync();object value=null;try{if(text.Length>0)value=json.DeserializeObject(text);}catch{}
    if(!response.IsSuccessStatusCode){if(response.StatusCode==HttpStatusCode.Unauthorized&&authorized){access=null;refresh=null;}var error=value as Dictionary<string,object>;string message=error==null?"":Read(error,"msg");if(message=="")message=error==null?"":Read(error,"message");if(message=="")message=error==null?"":Read(error,"error_description");throw new Exception("O site não confirmou a operação ("+(int)response.StatusCode+"). "+message);}
    return value;
   }
  }
 }
 public static string Read(Dictionary<string,object> row,string key){object v;return row.TryGetValue(key,out v)&&v!=null?Convert.ToString(v,CultureInfo.InvariantCulture):"";}
 void Session(Dictionary<string,object> s){access=Read(s,"access_token");refresh=Read(s,"refresh_token");expiry=DateTime.UtcNow.AddSeconds(Convert.ToDouble(s["expires_in"])-60);}
 async Task Refresh(){try{Session((Dictionary<string,object>)await Request("/auth/v1/token?grant_type=refresh_token","POST",new{refresh_token=refresh},false));}catch{access=null;refresh=null;throw new Exception("Sua sessão expirou. Entre novamente para continuar.");}}
 public async Task Login(string email,string password){if(String.IsNullOrEmpty(password))throw new Exception("Digite sua senha.");try{Session((Dictionary<string,object>)await Request("/auth/v1/token?grant_type=password","POST",new{email=email.Trim().ToLowerInvariant(),password=password},false));}catch(Exception ex){if(ex.Message.IndexOf("Invalid login credentials",StringComparison.OrdinalIgnoreCase)>=0)throw new Exception("A senha foi recusada para videiravinhoteca@gmail.com. Confira a senha ou use Recuperar acesso. Sua conta está cadastrada; não crie outra.");throw;}try{var user=(Dictionary<string,object>)await Request("/auth/v1/user");var meta=user["app_metadata"] as Dictionary<string,object>;if(meta==null||Read(meta,"role")!="admin")throw new Exception("Esta conta não tem permissão para editar o site.");}catch{access=null;refresh=null;throw;}}
 public async Task SendRecovery(){await Request("/auth/v1/recover","POST",new{email=AdminEmail},false);}
 public async Task Recover(string link,string password){Uri uri;if(!Uri.TryCreate(link.Trim(),UriKind.Absolute,out uri)||uri.Scheme!="https"||uri.Host!=new Uri(BaseUrl).Host||uri.AbsolutePath!="/auth/v1/verify")throw new Exception("Cole o endereço do botão de recuperação recebido por e-mail, sem abrir o link antes.");var query=System.Web.HttpUtility.ParseQueryString(uri.Query);string token=query["token"];if(query["type"]!="recovery"||String.IsNullOrEmpty(token))throw new Exception("O link não é de recuperação de senha.");if(password.Length<10)throw new Exception("Use uma senha nova de pelo menos 10 caracteres.");try{Session((Dictionary<string,object>)await Request("/auth/v1/verify","POST",new{token_hash=token,type="recovery"},false));var user=(Dictionary<string,object>)await Request("/auth/v1/user");if(!String.Equals(Read(user,"email"),AdminEmail,StringComparison.OrdinalIgnoreCase))throw new Exception("O link não pertence à conta administradora.");await Request("/auth/v1/user","PUT",new{password=password});}catch{access=null;refresh=null;throw;}await Logout();}
 public async Task Signup(string password){await Request("/auth/v1/signup?redirect_to=https%3A%2F%2Fvideira-loja.vercel.app%2Fadmin","POST",new{email=AdminEmail,password=password},false);}
 public async Task Logout(){try{if(Connected)await Request("/auth/v1/logout","POST");}finally{access=null;refresh=null;}}
 public async Task<List<Dictionary<string,object>>> Products(){var rows=new List<Dictionary<string,object>>();int offset=0;while(true){var batch=(object[])await Request("/rest/v1/products?select=*&order=created_at.desc&limit=500&offset="+offset);rows.AddRange(batch.Cast<Dictionary<string,object>>());if(batch.Length<500)return rows;offset+=batch.Length;}}
 public async Task Save(Dictionary<string,object> item,string id){var rows=(object[])await Request("/rest/v1/products"+(id==""?"":"?id=eq."+Uri.EscapeDataString(id)),id==""?"POST":"PATCH",item);if(rows==null||rows.Length!=1)throw new Exception("Nenhum produto foi alterado. Atualize a lista e confira suas permissões.");var saved=(Dictionary<string,object>)rows[0];if(Read(saved,"name")!=Read(item,"name"))throw new Exception("O site retornou dados diferentes. Atualize a lista antes de continuar.");}
 public async Task Hide(string id){var rows=(object[])await Request("/rest/v1/products?id=eq."+Uri.EscapeDataString(id),"PATCH",new{active=false});if(rows==null||rows.Length!=1)throw new Exception("O site não confirmou a alteração.");}
 public async Task<string> UploadPhoto(string path){if(!Connected)throw new Exception("Entre na conta do site antes de enviar fotos.");if(DateTime.UtcNow>=expiry)await Refresh();string ext=Path.GetExtension(path).ToLowerInvariant();string mime=ext==".png"?"image/png":ext==".webp"?"image/webp":ext==".jpg"||ext==".jpeg"?"image/jpeg":"";if(mime=="")throw new Exception("Escolha uma imagem JPG, PNG ou WebP.");var file=new FileInfo(path);if(file.Length>6000000)throw new Exception("Use uma imagem de até 6 MB.");string key="produtos/"+Guid.NewGuid().ToString("N")+ext;using(var req=new HttpRequestMessage(HttpMethod.Post,BaseUrl+"/storage/v1/object/catalog-images/"+key)){req.Headers.Add("apikey",PublicKey);req.Headers.Add("Authorization","Bearer "+access);req.Content=new ByteArrayContent(File.ReadAllBytes(path));req.Content.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue(mime);using(var response=await http.SendAsync(req)){if(!response.IsSuccessStatusCode)throw new Exception("Não foi possível enviar a foto ("+(int)response.StatusCode+"). Confira a sessão e tente novamente.");}}return BaseUrl+"/storage/v1/object/public/catalog-images/"+key;}
 public async Task UpdateProducts(string[] ids,Dictionary<string,object> changes){if(ids==null||ids.Length==0)throw new Exception("Selecione produtos para atualizar.");Guid parsed;foreach(string id in ids)if(!Guid.TryParse(id,out parsed))throw new Exception("Identificador de produto inválido. Atualize a lista.");if(changes.Count!=1||changes.Keys.Any(k=>k!="active"&&k!="featured")||changes.Values.Any(v=>!(v is bool)))throw new Exception("Ação de catálogo inválida.");var expected=new HashSet<string>(ids,StringComparer.OrdinalIgnoreCase);var rows=(object[])await Request("/rest/v1/products?id=in.("+String.Join(",",expected)+")","PATCH",changes);var actual=rows==null?new HashSet<string>():new HashSet<string>(rows.Cast<Dictionary<string,object>>().Select(p=>Read(p,"id")),StringComparer.OrdinalIgnoreCase);if(!expected.SetEquals(actual))throw new Exception("O site não confirmou todos os produtos. Atualize a lista para conferir o resultado antes de repetir.");}
 public void Dispose(){access=null;refresh=null;http.Dispose();}
}
}
