using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Parser.Models;
using System;
using System.Diagnostics;

namespace Parser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
       
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            this._memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("key_currency", out CurrencyConvert model))
            {
                throw new Exception("Ошибка получения данных");
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
