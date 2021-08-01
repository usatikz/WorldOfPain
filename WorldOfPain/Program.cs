using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfPain
{
    class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu();
            menu.ClearLog();
            menu.ShowMenu();
        }
    }
}
