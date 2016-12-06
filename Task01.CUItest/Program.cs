using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task01.ORM;

namespace Task01.CUItest
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime start = new DateTime(10,10,10);
            DateTime end = new DateTime(2020,10,10);
            SellPointReport test = LINQqueries.GetReportFromSellPoint("minskShawarma", start, end);
            SellerReport test2 = LINQqueries.GetSellerReport(start,end);
        }
    }
}
