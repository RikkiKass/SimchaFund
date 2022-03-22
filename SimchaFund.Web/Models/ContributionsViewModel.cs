using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaFund.Data;

namespace SimchaFund.Web.Models
{
    public class ContributionsViewModel
    {
        public int SimchaId { get; set; }
        public string SimchaName { get; set; }
        public List<Contributor> Contributors { get; set; }
    }
}
