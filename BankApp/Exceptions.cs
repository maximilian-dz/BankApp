using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Exceptions : Exception
    {
        public static int InputInt(string userInput)
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid input. Try again.");
            }

            return result;
        }

        public static string CheckIfEmpty(string userInput)
        {
            userInput = Console.ReadLine();
            while (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Oops! You forgot to enter the information we requested. Try again.");
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        public static decimal InputDec(string userInput)
        {
            decimal result;
            
            while (!decimal.TryParse(Console.ReadLine(), out result))
            {
               Console.WriteLine("Invalid input. Try again.");
            }
            
                return result;
        }
    }
}
