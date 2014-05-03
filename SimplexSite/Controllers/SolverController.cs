using SimplexModel;
using SimplexModel.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SimplexSite.Controllers
{
    public class SolverController : Controller
    {
        //
        // GET: /Solver/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EnterData()
        {
            ViewBag.Error = TempData["Error"];
            ViewData["Text"] = TempData["Text"];
            return View();
        }

        [HttpPost]
        public ActionResult Solve(string text)
        { 
            Simplex tx = null;
            try
            {
                tx = new Parser(text).Parse();
                tx.Solve();
            }
            catch (ParseErrorException e)
            {
                TempData["Error"] = e.Message.ToString();
                TempData["Text"] = text;
                return RedirectToAction("EnterData");
            }
            return View(tx);
        }
        
	}
}