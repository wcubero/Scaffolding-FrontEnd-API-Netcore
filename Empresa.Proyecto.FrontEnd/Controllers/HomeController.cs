using Empresa.Proyecto.FrontEnd.Models;
using Empresa.Proyecto.Models;
using Empresa.Proyecto.Models.Minion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using RestSharp;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Empresa.Proyecto.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppSettings _appSettings;

        public HomeController(ILogger<HomeController> logger, IOptions<AppSettings> options)
        {
            _logger = logger;

            _appSettings = options.Value;
        }

        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        public JsonResult GetMinion([FromBody] Minion model)
        {
            return Json(Extensions.RestSharpRequest(_appSettings.ApiUrl + "Example/GetMinions", JsonConvert.SerializeObject(model), Method.POST)); 
        }

    }
}