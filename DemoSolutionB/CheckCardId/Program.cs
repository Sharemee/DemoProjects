using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace CheckCardId
{
    class Program
    {
        static void Main(string[] args)
        {
            string st = "<Req><Head><T>2123</T><K>0001</K><H>30666</H><C>1006</C><P>WST</P></Head><Service><CardNo>300003431276</CardNo><CardType>0</CardType><code>12601874660</code><IDType>1</IDType><denNo>3360731199703203433</denNo><name>Sunshine_SSSS</name><sex>男</sex><birthday>1997-03-20</birthday><phone>18296110110</phone><guardian></guardian><isguardian>0</isguardian></Service></Req>";
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(st);
            var res = xml.SelectSingleNode("Req/Service/IDTypes");
            if (res is null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine(res.InnerText);
            }

            Console.ReadKey();
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
