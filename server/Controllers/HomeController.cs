using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Models;
using server.Data;

namespace server.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FetchedDataContext _context;

        public HomeController(ILogger<HomeController> logger, FetchedDataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.ChoronicleRecords
                .OrderByDescending(r => r.RecordedTime);
            var weibo = data.First(r => r.Type == ChoronicleRecordType.Weibo);
            var zhihu = data.First(r => r.Type == ChoronicleRecordType.Zhihu);
            var model = new HomeViewModel()
            {
                WeiboId = weibo.Id,
                ZhihuId = zhihu.Id,
                WeiboLast = weibo.RecordedTime,
                ZhihuLast = zhihu.RecordedTime
            };
            return View(model);
        }

        public IActionResult About()
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
