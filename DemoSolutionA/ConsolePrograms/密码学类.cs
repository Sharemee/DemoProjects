using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePrograms
{
    /// <summary>
    /// 密码学类
    /// </summary>
    public class Cryptology
    {
        public static void 异或运算_不使用第三个变量交换两值(int n1,int n2,out int r1,out int r2)
        {
            n1 = n1 ^ n2;
            n2 = n1 ^ n2;
            n1 = n1 ^ n2;
            r1 = n1;
            r2 = n2;
        }
    }
}
