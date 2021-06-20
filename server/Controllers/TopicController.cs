using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Models;

namespace server.Controllers
{
    public class TopicController : Controller
    {
        private readonly FetchedDataContext _context;

        public TopicController(FetchedDataContext context)
        {
            _context = context;
        }

        public IActionResult Search(string name, string type)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type))
                return NotFound();
            
            ChoronicleRecordType typ = ChoronicleRecordType.Unkown;
            switch (type.ToUpper())
            {
                case "ZHIHU":
                    typ = ChoronicleRecordType.Zhihu;
                    break;
                case "WEIBO":
                    typ = ChoronicleRecordType.Weibo;
                    break;
                case "ALL":
                    typ = ChoronicleRecordType.All;
                    break;
            }
            
            var results = _context.TopicEntries
                .Where(e => e.ChoronicleRecord.Type == typ)
                .Where(e => e.Topic == name)
                .OrderByDescending(e => e.ChoronicleRecord.RecordedTime)
                .ToList();

            return View(results);
        }
    }
}