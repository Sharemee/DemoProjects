using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardValidation
{
    public class IdCardValidation
    {
        /// <summary>  
        /// 验证身份证合理性  
        /// </summary>  
        /// <param name="idNumber"></param>  
        /// <returns></returns>
        public bool CheckIdCard(string idNumber)
        {
            switch (idNumber.Length)
            {
                case 18:
                    {
                        var check = CheckIdCard18(idNumber);
                        return check;
                    }
                case 15:
                    {
                        var check = CheckIdCard15(idNumber);
                        return check;
                    }
                default:
                    return false;
            }
        }

        /// <summary>  
        /// 18位身份证号码验证  
        /// </summary>  
        private static bool CheckIdCard18(string idNumber)
        {
            if (long.TryParse(idNumber.Remove(17), out var n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false; //数字验证  
            }

            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false; //省份验证
            }

            var birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            if (DateTime.TryParse(birth, out _) == false)
            {
                return false;//生日验证  
            }

            //校验码验证
            var arrArrifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            var wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            var ai = idNumber.Remove(17).ToCharArray();
            var sum = 0;
            for (var i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            Math.DivRem(sum, 11, out var y);
            if (arrArrifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false; //校验码（最后一位）验证  
            }

            return true; //符合GB11643-1999标准  
        }

        /// <summary>  
        /// 15位身份证号码验证  
        /// </summary>  
        private static bool CheckIdCard15(string idNumber)
        {
            if (long.TryParse(idNumber, out var n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证  
            }

            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证  
            }

            var birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            if (DateTime.TryParse(birth, out _) == false)
            {
                return false;//生日验证  
            }

            return true;
        }
    }
}
