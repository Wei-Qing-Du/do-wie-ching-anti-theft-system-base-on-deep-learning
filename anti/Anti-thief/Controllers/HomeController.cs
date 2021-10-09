using Anti_thief.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anti_thief.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRecorderRepository _studentRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ILogger logger;

        private readonly IDataProtector dataProtector;

        public HomeController(IRecorderRepository studentRepository, IWebHostEnvironment hostingEnvironment,
            ILogger<HomeController> logger, DataProtectionPurposeStrings dataProtectionPurposeStrings, IDataProtectionProvider dataProtectionProvider)
        {
            _studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
           // dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.StudentIdRouteValue);

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
