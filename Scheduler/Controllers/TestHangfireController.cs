using System.Diagnostics;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Scheduler.Controllers {
    
    public class TestHangfireController : Controller {
        private IBackgroundJobClient BackgroundJob { get; }

        public TestHangfireController(IBackgroundJobClient backgroundJob) {
            this.BackgroundJob = backgroundJob;
        }

        public IActionResult Index() {
            BackgroundJob.Enqueue(() => Debug.WriteLine("Background Job completed successfully!"));
            return RedirectToAction("Index", "Home");
        }
    }
}