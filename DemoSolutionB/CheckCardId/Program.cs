using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CheckCardId
{
    class Program
    {
        static void Main(string[] args)
        {
            string cardId = "360121198607318353";
            Console.WriteLine(CheckCardIds(cardId));

            Console.ReadKey();
        }

        static bool CheckCardIds(string id)
        {
            if (id.Length == 18)
            {
                return A(id);
            }
            else
            {
                if (id.Length == 15)
                {
                    return B(id);
                }
                else
                {
                    return false;
                }
            }
        }

        static bool A(string str)
        {
            string reg18 = @"(^[0-9]{17}[x|S]$)|(^[0-9]{18})";
            return Regex.IsMatch(str, reg18);
        }

        static bool B(string str)
        {
            string reg15 = @"^[0-9]{15}";
            return Regex.IsMatch(str, reg15);
        }
    }
}
