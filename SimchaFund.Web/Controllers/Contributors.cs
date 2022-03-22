using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimchaFund.Data;
using SimchaFund.Web.Models;

namespace SimchaFund.Web.Controllers
{
    public class Contributors : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";
        public IActionResult Index()
        {
            Manager manager = new Manager(_connectionString);
            List<Contributor> contributors = manager.GetContributors();
            decimal total = 0;
            foreach(Contributor contributor in contributors)
            {
                total += contributor.Balance;
            }
            return View(new ContributorsViewModel { Contributors = contributors, TotalAmount=total});
         
        }
        public IActionResult New(Contributor contributor, string lastName, decimal deposit, DateTime date)
        {
            Manager manager = new Manager(_connectionString);
            contributor.Name = contributor.Name + lastName;
            int id=manager.AddContributor(contributor);
            manager.Deposit(new Deposit { DepositAmount = deposit, Date = date, ContributorId = id });

            return Redirect("/contributors/index");
        }


        public IActionResult Edit(Contributor contributor)
        {
            Manager manager = new Manager(_connectionString);

            manager.EditContributor(contributor);

            return Redirect("/contributors/index");
        }


        public IActionResult Deposit(Deposit deposit)
        {
            Manager manager = new Manager(_connectionString);
            manager.Deposit(deposit);
            return Redirect("/contributors/index");
        }
        public IActionResult ShowHistory(int contributorId)
        {
            Manager manager = new Manager(_connectionString);
            List<History> histories = manager.GetHistory(contributorId);
            string conName = manager.GetContributorName(contributorId);
            histories.Sort((x, y) => DateTime.Compare(x.Date , y.Date));
       


            return View(new HistoryViewModel {ContributorName=conName, History=histories, TotalBalance=manager.GetBalance(contributorId)});
        }
    }
}
