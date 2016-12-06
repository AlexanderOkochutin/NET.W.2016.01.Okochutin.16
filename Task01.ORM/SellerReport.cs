using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task01.ORM
{
    public class SellerReport
    {
        public List<string> SellersNames { get; set; }
        public List<string> SellPOintsTitle { get; set; }
        public List<int> CookingTime { get; set; }

        public SellerReport()
        {
            SellersNames = new List<string>();
            SellPOintsTitle = new List<string>();
            CookingTime = new List<int>();
        }
    }
}
