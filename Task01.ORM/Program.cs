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
            LINQqueries.AddIngradientCategory("cheese");
            LINQqueries.AddIngradientCategory("pita");
            LINQqueries.AddIngradientCategory("meat");
            LINQqueries.AddIngradientCategory("vegetables");
            LINQqueries.AddIngradient("tomato", "vegetables", 5);
            LINQqueries.AddIngradient("carrot", "vegetables", 5);
            LINQqueries.AddIngradient("pig", "meat", 5);
            LINQqueries.AddIngradient("chiken", "meat", 5);
            LINQqueries.AddIngradient("cow", "meat", 5);
            LINQqueries.AddIngradient("normPita", "pita", 5);
            LINQqueries.AddIngradient("slimPita", "pita", 5);
            LINQqueries.AddIngradient("salt", "cheese", 5);
            LINQqueries.AddIngradient("notSalt", "cheese", 5);
        }
    }
}
