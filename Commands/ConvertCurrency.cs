using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ether_bot.Interfaces;
using Ether_bot.Models;

namespace Ether_bot.Commands
{
    public class ConvertCurrency : IConvertCurrency
    {
        public async Task<decimal> ConvertCurrencyAsync(decimal value, CurrencyModel currency)
        {
            decimal result = 0;
            switch (currency.Currency)
            {
                case "USD":
                result = value;
                break;
                case "RUB":
                    using (HttpClient client = new HttpClient())
                    {
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        var response = await client.GetStringAsync("http://www.cbr.ru/scripts/XML_daily.asp");
                        //var xml = Encoding.Convert(win1251,utf8, await response);
                        var incomingXml = XDocument.Parse(response);
                        var stringValue = (from cur in incomingXml.Element("ValCurs").Elements("Valute")
                                      where cur.Element("CharCode").Value == "USD"
                                      select cur.Element("Value").Value).First().Replace(',','.');
                        result = decimal.Parse(stringValue) * value;
                    }
                break;
            }
            return result;
        }
    }
}