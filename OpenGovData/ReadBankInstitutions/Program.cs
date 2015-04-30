using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadBankInstitutions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            var bd = new BankDetails();
            bd.ReadStoreValues(0);
            Console.WriteLine("Application ran for {0}", sw.Elapsed);
            Console.ReadKey();
        }
    }
}