using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task01.ORM
{
    public class SellPointReport
    {
        public List<string> SellerName { get; set; }
        public string Address { get; set; }
        public string SellingPointCategoryName { get; set; }
        public string TitleOfSellingPoint { get; set; }
        public int NumberOfOders { get; set; }
        public decimal Profit { get; set; }
    }
}
