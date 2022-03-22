using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaFund.Data;

namespace SimchaFund.Web.Models
{
    public class HistoryViewModel
    {
        public string ContributorName { get; set; }
        public List<History>History { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
