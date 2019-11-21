using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EfUnitOfWorkDemo.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    public class HomeController : ControllerBase
    {

        public HomeController()
        {

        }

        public ActionResult<string> Index()
        {
            return "Home/Index";
        }
    }
}
