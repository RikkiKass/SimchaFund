using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaFund.Data;

namespace SimchaFund.Web.Models
{
    public class SimchosViewModel
    {
        public List<Simcha> Simchos { get; set; }
        public int TotalContributorCount { get; set; }
        public string Message { get; set; }
    }
}
