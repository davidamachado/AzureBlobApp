using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestWebAzureApp.Models;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace TestWebAzureApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static string Connection { get; set; }
        public static string Name { get; set; }
        public static IEnumerable<BlobClient> BlobList { get; set; }

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Connection = configuration.GetConnectionString("AzureConnection");
            BlobController.Create();
            BlobList = BlobController.List();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile formFile)
        {
            ViewData["message"] = null;

            //Check if a file has been selected
            if (formFile != null)
            {
                if(BlobController.Duplicate(formFile.FileName))
                {
                    ViewData["message"] = "THIS FILE ALREADY EXISTS";
                    return View();
                }
                else
                {
                    BlobController.Upload(formFile);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["message"] = "A FILE MUST BE SELECTED";
                return View();
            }
        }

        public IActionResult Delete(string fileName)
        {
            ViewData["File"] = fileName;
            Name = fileName;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteAction(string fileName)
        {
            BlobController.Delete(fileName);
            return RedirectToAction(nameof(Index));
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
