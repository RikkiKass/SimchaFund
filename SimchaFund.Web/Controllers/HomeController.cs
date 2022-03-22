using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimchaFund.Web.Models;
using SimchaFund.Data;

namespace SimchaFund.Web.Controllers
{
    public class HomeController : Controller
    {

        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";


        public IActionResult Index()
        {
            Manager manager = new Manager(_connectionString);
            List<Simcha> simchos = manager.GetSimchos();
            string message = (string)TempData["Message"];
            return View(new SimchosViewModel { Simchos=simchos, TotalContributorCount=manager.GetContributorCount(), Message=message});
        }
        public IActionResult Contributors()
        {
            Manager manager = new Manager(_connectionString);
            List<Contributor> contributors = manager.GetContributors();
            return View(new ContributorsViewModel {Contributors=contributors});
        }
       

    }
}
