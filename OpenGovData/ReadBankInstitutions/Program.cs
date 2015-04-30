using System;
using System.Diagnostics;

namespace ReadBankInstitutions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            var bd = new BankDetails();
            bd.ReadStoreValues(5000);
            Console.WriteLine("Application ran for {0}", sw.Elapsed);
            Console.ReadKey();
        }
    }
}