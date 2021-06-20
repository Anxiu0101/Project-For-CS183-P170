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
    public class ChoronicleController : Controller
    {
        private readonly FetchedDataContext _context;

        public ChoronicleController(FetchedDataContext context)
        {
            _context = context;
        }

        // GET: Choronicle
        public async Task<IActionResult> Index(string type)
        {
            if (string.IsNullOrEmpty(type)) type = "ALL";
            var nt = type.ToUpper();
            ChoronicleRecordType typ;
            switch (nt)
            {
                case "ZHIHU":
                    typ = ChoronicleRecordType.Zhihu;
                    break;
                case "WEIBO":
                    typ = ChoronicleRecordType.Weibo;
                    break;
                default:
                    typ = ChoronicleRecordType.All;
                    break;
            }
            IQueryable<ChoronicleRecord> rst;
            if (typ != ChoronicleRecordType.All) rst = _context.ChoronicleRecords.Where(i => i.Type == typ);
            else rst = _context.ChoronicleRecords;
            rst.OrderByDescending(i => i.RecordedTime);
            return View(await rst.ToListAsync());
        }

        public IActionResult List()
        {
            return RedirectToAction("Index");
        }

        // GET: Choronicle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var choronicleRecord = await _context.ChoronicleRecords
                .Where(r => r.Id == id)
                .Include(r => r.Topics
                    .OrderByDescending(t => t.HotScore))
                .FirstOrDefaultAsync();
            if (choronicleRecord == null)
            {
                return NotFound();
            }

            return View(choronicleRecord);
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
