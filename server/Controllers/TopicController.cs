using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            
            ChronicleRecordType typ = ChronicleRecordType.Unkown;
            switch (type.ToUpper())
            {
                case "ZHIHU":
                    typ = ChronicleRecordType.Zhihu;
                    break;
                case "WEIBO":
                    typ = ChronicleRecordType.Weibo;
                    break;
                case "ALL":
                    typ = ChronicleRecordType.All;
                    break;
            }
            
            var results = _context.TopicEntries
                .Where(e => e.Topic == name)
                .Include(e => e.ChronicleRecord)
                .Where(e => e.ChronicleRecord.Type == typ)
                .OrderByDescending(e => e.ChronicleRecord.RecordedTime)
                .ToList();

            return View(results);
        }
    }
}