using DemoInvitation.Infrastructure.Security;
using DemoInvitation.Models;
using DemoInvitation.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DemoInvitation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SingletonService _singletonService;
        private readonly ScopedService _scopedService;
        private readonly TransientService _transientService;
        private readonly ITokenRepository _tokenRepository;

        public HomeController(ILogger<HomeController> logger, ITokenRepository tokenRepository, SingletonService singletonService, ScopedService scopedService, TransientService transientService)
        {
            _logger = logger;
            _singletonService = singletonService;
            _scopedService = scopedService;
            _transientService = transientService;
            _tokenRepository = tokenRepository;
        }

        public IActionResult Index()
        {
            ViewBag.Singleton1 = _singletonService.GetHashCode();
            ViewBag.Singleton2 = HttpContext.RequestServices.GetService(typeof(SingletonService)).GetHashCode();

            ViewBag.Scoped1 = _scopedService.GetHashCode();
            ViewBag.Scoped2 = HttpContext.RequestServices.GetService(typeof(ScopedService)).GetHashCode();

            ViewBag.Transient1 = _transientService.GetHashCode();
            ViewBag.Transient2 = HttpContext.RequestServices.GetService(typeof(ScopedService)).GetHashCode();

            return View();
        }

        //Genère un token pour le test
        public IActionResult Create()
        {
            return Json(new { URL = "https://localhost:5001/Invit/" + _tokenRepository.GenerateToken(new TokenUser() { Id = 1, LastName = "Doe", FirstName = "John", Email = "john.doe@unknow.com", IsAdmin = false }) });
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
