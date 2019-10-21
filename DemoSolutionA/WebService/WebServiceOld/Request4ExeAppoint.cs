using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceOld
{
    /// <summary>
    /// 预约请求
    /// </summary>
    public class Request4ExeAppoint
    {
        /// <summary>
        /// 就诊卡号 可为空，为空时为无卡预约
        /// </summary> 
        public string cardNo { get; set; }
        /// <summary>
        /// 身份证 可为空
        /// </summary> 
        public string denNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary> 
        public string name { get; set; }
        /// <summary>
        /// 监护人姓名
        /// </summary> 
        public string guardName { get; set; }
        /// <summary>
        /// 监护人身份证
        /// </summary> 
        public string guardIdCard { get; set; }
        /// <summary>
        /// 性别   性别入参传入  1、男 2、女
        /// </summary> 
        public string sex { get; set; }
        /// <summary>
        /// 出生年月 入参格式（YYYYMMDD）
        /// </summary> 
        public string birthday { get; set; }
        /// <summary>
        /// 地址
        /// </summary> 
        public string address { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary> 
        public string phone { get; set; }

        /// <summary>
        /// 院内HIS锁号产生的订单号(一附院使用)
        /// </summary> 
        public string hisOrderId { get; set; }
        /// <summary>
        /// 卫生厅生成的订单号(二附院使用)
        /// </summary> 
        public string wstOrderId { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary> 
        public string cardType { get; set; }

        public string code { get; set; }
        /// <summary>
        /// 是否代理
        /// </summary> 
        public string isAgent { get; set; }
        public string userid { get; set; }
        public string contactid { get; set; }
        public string BizCode { get; set; }
    }
}