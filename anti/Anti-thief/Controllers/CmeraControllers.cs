using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anti_thief.Controllers
{
    public class CmeraControllers : Controller
    {
        [HttpPost]
        public IActionResult Capture()
        {

            return View();
        }
    }
}
