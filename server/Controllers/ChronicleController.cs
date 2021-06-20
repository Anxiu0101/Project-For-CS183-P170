using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    public class ChronicleController : Controller
    {
        private readonly FetchedDataContext _context;

        public ChronicleController(FetchedDataContext context)
        {
            _context = context;
        }

        // GET: Chronicle
        public async Task<IActionResult> Index(string type)
        {
            if (string.IsNullOrEmpty(type)) type = "ALL";
            var nt = type.ToUpper();
            ChronicleRecordType typ;
            switch (nt)
            {
                case "ZHIHU":
                    typ = ChronicleRecordType.Zhihu;
                    break;
                case "WEIBO":
                    typ = ChronicleRecordType.Weibo;
                    break;
                default:
                    typ = ChronicleRecordType.All;
                    break;
            }
            IQueryable<ChronicleRecord> rst;
            if (typ != ChronicleRecordType.All) rst = _context.ChronicleRecords.Where(i => i.Type == typ);
            else rst = _context.ChronicleRecords;
            rst.OrderByDescending(i => i.RecordedTime);
            return View(await rst.ToListAsync());
        }

        public IActionResult List()
        {
            return RedirectToAction("Index");
        }

        // GET: Chronicle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ChronicleRecord = await _context.ChronicleRecords
                .Where(r => r.Id == id)
                .Include(r => r.Topics
                    .OrderByDescending(t => t.HotScore))
                .FirstOrDefaultAsync();
            if (ChronicleRecord == null)
            {
                return NotFound();
            }

            return View(ChronicleRecord);
        }

        public IActionResult ListZhihu()
        {
            return RedirectToAction("Index", new { type = "zhihu" });
        }

        public IActionResult ListWeibo()
        {
            return RedirectToAction("Index", new { type = "weibo" });
        }
    }
}
