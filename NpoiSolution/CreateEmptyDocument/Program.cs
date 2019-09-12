using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.XWPF.UserModel;

namespace CreateEmptyDocument
{
    class Program
    {
        static void Main()
        {
            XWPFDocument doc = new XWPFDocument();
            doc.CreateParagraph();
            FileStream fs = File.Create("Blank.docx");
            doc.Write(fs);
            fs.Close();

            Console.ReadKey();
        }
    }
}
