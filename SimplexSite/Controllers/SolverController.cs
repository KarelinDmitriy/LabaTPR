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
            return View();
        }

        [HttpPost]
        public ActionResult Solve(string text)
        {
            var tx = new Parser(text).Parse();
            tx.Solve();
            return View(tx);
        }
        
	}
}