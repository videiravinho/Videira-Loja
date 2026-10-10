using System;using System.Data;using System.Globalization;
namespace Videira {partial class Store {
public void AdjustProductStock(DataRow product,decimal quantity){
if(quantity<0||quantity!=decimal.Truncate(quantity))throw new Exception("O estoque deve ser uma quantidade inteira e não negativa.");
decimal before=(decimal)product["Estoque"];decimal difference=quantity-before;if(difference==0)return;
product["Estoque"]=quantity;Move(product,difference,"Ajuste","Edição do cadastro: "+before.ToString("N0",CultureInfo.GetCultureInfo("pt-BR"))+" → "+quantity.ToString("N0",CultureInfo.GetCultureInfo("pt-BR"))+" unidades");
}
}}
