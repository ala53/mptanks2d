using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var window = new Window())
                window.Run(60);
        }
    }
}
