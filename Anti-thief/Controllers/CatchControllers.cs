using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anti_thief.Controllers
{
    public class CatchControllers : Controller
    {
        [Route("")]
        [Route("Catch/Index")]
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
