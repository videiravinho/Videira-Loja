using System;
using System.Data;
using System.Linq;
namespace Videira { partial class Store {
 public bool ImportInfiniteRates(){var company=Company;if(company["Taxas Infinite importadas"].ToString()=="Sim")return false;decimal[] link={4.20m,6.09m,7.01m,7.91m,8.80m,9.67m,12.59m,13.42m,14.25m,15.06m,15.87m,16.66m};for(int i=0;i<link.Length;i++)AddInfiniteRate("Link • "+(i+1)+"x • 1 dia útil",link[i]);AddInfiniteRate("InfiniteTap • Débito Visa • 1 dia útil",1.37m);AddInfiniteRate("InfiniteTap • Débito Elo • 1 dia útil",2.58m);AddInfiniteRate("InfiniteTap • Crédito Amex 1x • 1 dia útil",4.91m);company["Taxas Infinite importadas"]="Sim";return true;}
 void AddInfiniteRate(string name,decimal value){if(!Data.Tables["Taxas"].AsEnumerable().Any(r=>r["Modalidade"].ToString()==name))Data.Tables["Taxas"].Rows.Add(Next("Taxas"),name,value);}
}}
