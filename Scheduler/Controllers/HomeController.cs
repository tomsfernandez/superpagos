using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Models;

namespace Scheduler.Controllers {
    public class HomeController : Controller {
        public HomeController(IBackgroundJobClient backgroundJob) {
            BackgroundJob = backgroundJob;
        }
        private IBackgroundJobClient BackgroundJob { get; }
        public IActionResult Index() {
            return View();
        }

        public IActionResult About() {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact() {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [HttpPost("test")]
        public IActionResult Test() {
            BackgroundJob.Enqueue(() => Debug.WriteLine("Background Job completed successfully!"));
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}