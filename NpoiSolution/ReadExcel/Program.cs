using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib;
using NPOI;
using NPOI.OpenXml4Net;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;

namespace ReadExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "Test.xlsx";
            NpoiHelper excel = new NpoiHelper();

            //显示工作表的名称
            Console.WriteLine("所有工作表名称：");
            var SheetNames = excel.GetSheetName(fileName);
            if (SheetNames.Length > 0)
            {
                foreach (var item in SheetNames)
                {
                    Console.WriteLine(item);
                }
            }
            Console.WriteLine("-----------------------");



            Console.ReadKey();
        }
    }
    public class NpoiHelper
    {
        /// <summary>
        /// 操作Excel文件对象
        /// </summary>
        private XSSFWorkbook Workbook { get; set; }

        public NpoiHelper() { }

        public NpoiHelper(string file)
        {
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            Workbook = new XSSFWorkbook(fs);
        }

        /// <summary>
        /// 获取Excel所有工作表的名称
        /// </summary>
        /// <returns>返回所有工作表的名称</returns>
        public string[] GetSheetName()
        {
            int sheetNum = 0;

            //获取Excel表的数量
            sheetNum = Workbook.NumberOfSheets;

            //根据索引获取工作表
            string[] result = new string[sheetNum];
            for (int i = 0; i < sheetNum; i++)
            {
                result[i] = Workbook.GetSheetName(i);
            }

            return result;
        }

        /// <summary>
        /// 获取Excel所有工作表的名称
        /// </summary>
        /// <param name="path">Excel文件名</param>
        /// <returns>返回所有工作表的名称</returns>
        public string[] GetSheetName(string path)
        {
            int sheetNum = 0;

            //读取Excel为文件流
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            //操作Excel文件对象
            Workbook = new XSSFWorkbook(fs);

            //获取Excel表的数量
            sheetNum = Workbook.NumberOfSheets;

            //根据索引获取工作表
            string[] result = new string[sheetNum];
            for (int i = 0; i < sheetNum; i++)
            {
                result[i] = Workbook.GetSheetName(i);
            }

            return result;
        }


    }
}
