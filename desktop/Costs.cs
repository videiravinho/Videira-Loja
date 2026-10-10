using System;
using System.Data;
using System.Linq;
namespace Videira {
partial class Store {
 public static void Upgrade(DataSet data){StrategySchema(data);MarketSchema(data);Add(data.Tables["Empresa"],"Taxas Infinite importadas",typeof(string),"Não");
  Add(data.Tables["Despesas"],"Plano mensal",typeof(int),0);Add(data.Tables["Plano mensal"],"Dia do mês",typeof(int),1);var p=data.Tables["Produtos"];
  foreach(string name in new[]{"EAN","EAN Caixa","Código fábrica","Abreviação","Sigla","Linha","Grupo","Subgrupo","Família","Marca","Fabricante","Unidade","Tipo embalagem","Localização","Prateleira","Observações"})Add(p,name,typeof(string),name=="Unidade"?"UN":name=="Tipo embalagem"?"Garrafa":"");
  foreach(string name in new[]{"Embalagem unitária","Outros custos unitários","Despesa variável %","Estoque mínimo","Volume ml","Peso líquido","Peso bruto","Validade dias","Unidades por caixa"})Add(p,name,typeof(decimal),name=="Embalagem unitária"||name=="Unidades por caixa"?1m:0m);
  Add(p,"Cadastro",typeof(DateTime),DateTime.Today);
  Add(data.Tables["Vendas"],"Outras despesas",typeof(decimal),0m);
  Add(data.Tables["Itens"],"Embalagem unitária",typeof(decimal),1m);
  Add(data.Tables["Itens"],"Outras despesas",typeof(decimal),0m);Add(data.Tables["Itens"],"Preço original",typeof(decimal),0m);Add(data.Tables["Itens"],"Desconto %",typeof(decimal),0m);DeliverySchema(data);MaterialsSchema(data);
 }
 static void Add(DataTable t,string name,Type type,object value){var c=t.Columns.Contains(name)?t.Columns[name]:t.Columns.Add(name,type);c.DefaultValue=value;foreach(DataRow r in t.Rows)if(r.IsNull(c))r[c]=value;}
 public DataTable Finance(DateTime from,DateTime to){
  var vs=Data.Tables["Vendas"].AsEnumerable().Where(r=>r["Situação"].ToString()=="Concluída"&&(DateTime)r["Data"]>=from.Date&&(DateTime)r["Data"]<to.Date.AddDays(1));
  Func<string,decimal> sum=col=>vs.Sum(r=>(decimal)r[col]);decimal sales=sum("Total"),cost=sum("Custo"),pack=sum("Embalagem"),fee=sum("Taxa R$"),extra=sum("Outras despesas");
  var ds=Data.Tables["Despesas"].AsEnumerable().Where(r=>(DateTime)r["Data"]>=from.Date&&(DateTime)r["Data"]<to.Date.AddDays(1));decimal fixedCost=ds.Where(r=>r["Categoria"].ToString()=="Gastos fixos").Sum(r=>(decimal)r["Valor"]),variable=ds.Where(r=>r["Categoria"].ToString()=="Gastos variáveis"&&(int)r["Insumo"]==0).Sum(r=>(decimal)r["Valor"]);
  var t=new DataTable();t.Columns.Add("Indicador");t.Columns.Add("Valor (R$)",typeof(decimal));t.Rows.Add("Vendas concluídas",sales);t.Rows.Add("Custo dos produtos vendidos",cost);t.Rows.Add("Embalagens",pack);t.Rows.Add("Taxas de maquininha",fee);t.Rows.Add("Outros custos dos produtos",extra);t.Rows.Add("Resultado das vendas",sales-cost-pack-fee-extra);t.Rows.Add("Gastos fixos do período",fixedCost);t.Rows.Add("Gastos variáveis do período",variable);t.Rows.Add("Resultado após despesas",sales-cost-pack-fee-extra-fixedCost-variable);t.Rows.Add("Compras de embalagens / saída de caixa (não descontar novamente)",ds.Where(r=>(int)r["Insumo"]>0).Sum(r=>(decimal)r["Valor"]));return t;
 }
}
}



