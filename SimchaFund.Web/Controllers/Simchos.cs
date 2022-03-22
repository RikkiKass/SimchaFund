using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimchaFund.Data;
using SimchaFund.Web.Models;

namespace SimchaFund.Web.Controllers
{
    public class Simchos : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";


        [HttpPost]
        public IActionResult New(Simcha simcha)
        {
            Manager manager = new Manager(_connectionString);
            int id=manager.AddSimcha(simcha);
            TempData["Message"] = $"New Simcha added! Id is {id} ";
            return Redirect("/home/index");
        }
        public IActionResult Contributions(int simchaId)
        {
            Manager manager = new Manager(_connectionString);
            string simchaName = manager.GetSimchaName(simchaId);
            List<Contributor> contributors = manager.GetContributors();
           
            ContributionsViewModel vm = new ContributionsViewModel
            {
                SimchaId=simchaId,
                SimchaName = simchaName,
                
                Contributors = contributors
                
            };
            
            return View(vm);
        }
        [HttpPost]
        public IActionResult UpdateContributions( int simchaId, List<Contributor> contributors)
        {

            Manager manager = new Manager(_connectionString);
            manager.DeleteFromSimchosContributors(simchaId);
            foreach (Contributor contributor in contributors)
            {
                if (contributor.Include)
                {
                    manager.UpdateContributions(simchaId, contributor.Id,  contributor.AmountWishesToGive);
                }
                
            }
            return Redirect("/");
        }
    }
}
