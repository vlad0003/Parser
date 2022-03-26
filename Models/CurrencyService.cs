using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Parser.Models
{
    public class CurrencyService : BackgroundService
    {
        private readonly IMemoryCache _memoryCache;
        public CurrencyService(IMemoryCache memoryCache)
        { 
            this._memoryCache = memoryCache;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    XDocument xml = XDocument.Load("https://cbr.ru/scripts/XML_daily.asp");

                    CurrencyConvert currencyConvert = new CurrencyConvert();
                    currencyConvert.USD = Convert.ToDecimal(xml.Elements("ValCurs")
                        .Elements("Valute").FirstOrDefault(x => x.Element("NumCode")
                        .Value == "840").Elements("Value").FirstOrDefault().Value);
                    currencyConvert.EUR = Convert.ToDecimal(xml.Elements("ValCurs")
                        .Elements("Valute").FirstOrDefault(x => x.Element("NumCode")
                        .Value == "978").Elements("Value").FirstOrDefault().Value);
                    currencyConvert.UAH = Convert.ToDecimal(xml.Elements("ValCurs")
                        .Elements("Valute").FirstOrDefault(x => x.Element("NumCode")
                        .Value == "980").Elements("Value").FirstOrDefault().Value);

                    _memoryCache.Set("key_currency", currencyConvert, TimeSpan.FromHours(1));

                }

                catch(Exception)
                {
                    throw new Exception("Ошибка данных");
                }

                await Task.Delay(3600000,stoppingToken);
            }
        }
    }
}
