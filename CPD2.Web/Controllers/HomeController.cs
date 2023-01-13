using CPD2.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CPD2.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace CPD2.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

             IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            Settings.CPDConnectionString = config["CPDConnectionString"];
        }

        [HttpGet]
        public IActionResult Index()
        {
            //ISession lSession = (ISession)HttpContext.Session;

            //Requires SessionExtensions from sample download.
            //if (HttpContext.Session.Get<DateTime>(SessionKeyTime) == default)
            //{
            //    HttpContext.Session.Set<DateTime>(SessionKeyTime, currentTime);
            //}

            //HttpContext.Session.SetInt32("CustomerId", id );

            return View();
        }

     

        public IActionResult Welcome()
        {
    
            List<AvailableSurvey> lSurveys = ModuleData.GetAvailableSurveys(108244);

            return View(lSurveys);
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
