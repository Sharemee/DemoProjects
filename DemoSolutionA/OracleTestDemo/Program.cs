using BK.JXWSWX.OracleEFModel;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Odbc;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleTestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //超时时间10天
            int timeOutDay = 10;

            //开始时间
            DateTime startDay = DateTime.Now.AddDays(-1 * timeOutDay).Date;
            //结束时间 最后时间是昨天
            DateTime endDay = DateTime.Now.AddDays(-1).Date;

            //获取注册医院列表
            List<decimal> RegHos = GetRegHos();
            if (RegHos.Count == 0)
            {
                Console.WriteLine("入驻互联网医院列表为空, 统计结束");
                //LogHelper.Info(log, "入驻互联网医院列表为空, 统计结束");
                return;
            }

            //医院日统计数据-综合
            //StatHosDay(RegHos, startDay, endDay);

            //医院日统计数据之拒诊
            StatHosDayRejectTreat(RegHos, startDay, endDay);

            Console.WriteLine("数据计算完成");
            Console.ReadKey();
        }

        /// <summary>
        /// 获取互联网医院的ID
        /// </summary>
        /// <returns></returns>
        private static List<decimal> GetRegHos()
        {
            List<decimal> result = new List<decimal>();
            try
            {
                using (var ctx = new Entities())
                {
                    result = ctx.HOSPITAL.Where(x => x.NETHOSJOINMODE >= 0).Select(x => x.HOSPITALID).ToList();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error(log, $"获取互联网医院异常GetRegHos", ex);
            }
            return result;
        }

        /// <summary>
        /// 医院日统计数据-综合
        /// </summary>
        /// <param name="timeOut">超时时间</param>
        /// <param name="batchCount">批次处理数量</param>
        /// <param name="rangeCount">物理线程数</param>
        private static void StatHosDay(List<decimal> hoslist, DateTime startDay, DateTime endDay)
        {
            while (startDay <= endDay)
            {
                foreach (var hos in hoslist)
                {
                    Console.WriteLine("开始计算: 医院日统计数据-综合");
                    Console.WriteLine(startDay.ToString("D") + " \t" + hos);
                    //医院日统计数据-综合 StatHosDay
                    using (var ctx = new Entities())
                    {
                        //先检查有没有目标日期和医院ID的数据(唯一)
                        STATHOSDAY data = ctx.STATHOSDAY.AsNoTracking().FirstOrDefault(x => x.HOSID == hos && x.STATDAY == startDay);

                        //获取目标统计数据
                        var tem = GetStatHosDayData(hos, startDay);

                        //如果存在记录
                        if (data != null)
                        {
                            //更新数据
                            ctx.Entry(tem).State = EntityState.Modified;
                        }
                        else
                        {
                            //不存在则插入数据
                            ctx.Entry(tem).State = EntityState.Added;
                        }
                        ctx.SaveChanges();
                    }
                }
                //按日期循环累加
                startDay = startDay.AddDays(1);
            }
            Console.WriteLine("计算完成: 医院日统计数据-综合");
            //LogHelper.Info(log, "StatHosDay 计算完成");
        }

        /// <summary>
        /// 计算相应日期内的医院日统计数据-综合数据
        /// </summary>
        /// <param name="hosId"></param>
        /// <param name="StartTime"></param>
        /// <returns></returns>
        private static STATHOSDAY GetStatHosDayData(decimal hosId, DateTime StartTime)
        {
            STATHOSDAY result = new STATHOSDAY();
            try
            {
                var EndTime = StartTime.AddDays(1).AddSeconds(-1);
                using (var ctx = new Entities())
                {
                    //统计日期
                    result.STATDAY = StartTime;
                    //医院ID
                    result.HOSID = hosId;
                    //医生注册量
                    result.DOCREGCOUNT = ctx.DOCTOR.Count(x => x.HOSID == hosId && x.CREATEDT >= StartTime && x.CREATEDT <= EndTime);
                    //药师注册量
                    result.PHARREGCOUNT = ctx.PHARMACIST.Count(x => x.HOSID == hosId && x.CREATEDT >= StartTime && x.CREATEDT <= EndTime);
                    //注册用户数//从另一张表来##
                    result.USERREGCOUNT = 0;
                    //问诊患者数
                    result.INTREATUSERCOUNT = ctx.SDTREATINFO
                        .Where(x => x.NETHOSID == hosId && x.TREATSHEETTIME >= StartTime && x.TREATSHEETTIME <= EndTime)
                        .GroupBy(x => x.PATIENTPERMITNUM)
                        .Count();
                    //问诊量
                    result.INTREATTIMECOUNT = ctx.SDTREATINFO.Count(x => x.NETHOSID == hosId && x.TREATSHEETTIME >= StartTime && x.TREATSHEETTIME <= EndTime);
                    //有开处方的问诊量
                    result.INTREATTIMECOUNTTOP = ctx.SDTREATINFO.Count(x => x.NETHOSID == hosId && x.TREATSHEETTIME >= StartTime && x.TREATSHEETTIME <= EndTime && x.HASPRESCRIPTION == 1);
                    //问诊金额
                    result.TREATFEE = ctx.SDTREATINFO
                        .Where(x => x.NETHOSID == hosId && x.TREATSHEETTIME >= StartTime && x.TREATSHEETTIME <= EndTime)
                        .ToList()//数据为空会报异常, 使用DefaultIfEmpty()还会报异常
                        .Sum(x => x.FEE);
                    //平均问诊响应时间(秒)
                    var query1 = ctx.SDTREATINFO.Where(x => x.NETHOSID == hosId && x.TREATSHEETTIME >= StartTime && x.DOCFIRSTRESPTIME.HasValue && x.ISREJECT == 0);
                    if (query1.Count() == 0)
                    {
                        result.AVERAGEDOCRESPTIME = 0;
                    }
                    else
                    {
                        result.AVERAGEDOCRESPTIME = (decimal)query1.Average(x => DbFunctions.DiffSeconds(x.DOCFIRSTRESPTIME, x.TREATSHEETTIME));
                    }

                    //拒诊量
                    result.REJECTTREATCOUNT = ctx.SDTREATINFO.Count(x => x.NETHOSID == hosId && x.TREATSHEETTIME >= StartTime && x.TREATSHEETTIME <= EndTime && x.ISREJECT == 1);
                    //开处方量
                    result.PRESCRIPTIONCOUNT = ctx.SDPRESCRIPTION.Count(x => x.NETHOSID == hosId && x.CREATETIME >= StartTime && x.CREATETIME <= EndTime);
                    //平均开方通过时间(秒)
                    var tem1 = ctx.SDPRESCRIPTION.Where(x => x.NETHOSID == hosId && x.CREATETIME >= StartTime && x.CREATETIME <= EndTime);
                    if (tem1.Count() == 0)
                    {
                        result.AVERAGEPCHECKTIME = 0;
                    }
                    else
                    {
                        //不要使用ToList(), 在内存中无法使用计算DbFunctions, 会报异常
                        result.AVERAGEPCHECKTIME = (decimal)tem1.Average(x => DbFunctions.DiffSeconds(x.PHARCHECKTIME, x.CREATETIME));
                    }
                    //处方购药金额
                    result.PRESCRIPTIONPAYFEE = (from f in ctx.SDPRESCRIPTION
                                                 join j in ctx.SDPRESCRIPTIONPAYTYPE on f.SDPRESCRIPTIONID equals j.SDPRESCRIPTIONID
                                                 where f.NETHOSID == hosId && f.CREATETIME >= StartTime && f.CREATETIME <= EndTime
                                                 select j.FEE).ToList().Sum();
                    //处方用药金额
                    result.PRESCRIPTIONDRUGFEE = ctx.SDPRESCRIPTION
                        .Where(x => x.NETHOSID == hosId && x.CREATETIME >= StartTime && x.CREATETIME <= EndTime)
                        .ToList()
                        .Sum(x => x.DRUGTOTALVALUE);
                    //购买药品处方量 GROUP BY 是因为一张处方单可能有多个付款记录, 用户可以一个处方一个处方支付
                    string PURCHASEPCOUNT = $@"SELECT TEM.SDPRESCRIPTIONID
                                           FROM (
                                               SELECT SDP.SDPRESCRIPTIONID,SPY.FEE AS MONEY
                                               FROM SDPRESCRIPTION SDP
                                               LEFT JOIN SDPRESCRIPTIONPAYTYPE SPY
                                               ON SPY.SDPRESCRIPTIONID=SDP.SDPRESCRIPTIONID
                                               WHERE SDP.NETHOSID=:HOSID AND SDP.CREATETIME BETWEEN :STARTTIME AND :ENDTIME
                                           ) TEM
                                           WHERE TEM.MONEY>0
                                           GROUP BY TEM.SDPRESCRIPTIONID";
                    OracleParameter[] op = new OracleParameter[]
                    {
                   new OracleParameter(":HOSID",hosId),
                   new OracleParameter(":STARTTIME",StartTime),
                   new OracleParameter(":ENDTIME",EndTime)
                    };
                    result.PURCHASEPCOUNT = ctx.Database.SqlQuery<decimal>(PURCHASEPCOUNT, op).Count();
                    //处方转出量
                    result.PRESCRIPTIONOUTCOUNT = ctx.SDPRESCRIPTION
                        .Count(x => x.NETHOSID == hosId && x.CREATETIME >= StartTime && x.CREATETIME <= EndTime && x.DELIVERYTYPE == 3);
                    //开通科室数量
                    result.DEPTCOUNT = ctx.DEPART.Count(x => x.HOSPITALID == hosId && x.CREATEDT >= StartTime && x.CREATEDT <= EndTime);
                    //更新时间
                    result.UPDATETIME = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error(log, ex.Message, ex);
                return result;
            }

            return result;
        }

        private static void StatHosDayRejectTreat(List<decimal> hoslist, DateTime startDay, DateTime lastTime)
        {
            Console.WriteLine("开始计算: 医院日统计数据之拒诊");

            //开始计算日期
            DateTime TempTime = startDay;

            //获取拒诊类型Code和名称对应关系
            Dictionary<decimal, string> dic = new Dictionary<decimal, string>();
            var config = ConfigurationManager.AppSettings["RejectType"];
            if (config is null)
            {
                Console.WriteLine("拒诊类型未配置,计算提前结束");
                //LogHelper.Info(log, "拒诊类型未配置,计算提前结束");
                return;
            }
            config = config.TrimEnd('|');//去掉后面多余的|,不然会多一个空的字符数组.
            string[] s = config.Split('|');
            foreach (var item in s)
            {
                int index = item.IndexOf(":");
                decimal key = Convert.ToDecimal(item.Substring(0, index));
                string value = item.Substring(index + 1);
                dic.Add(key, value);
            }

            //在日期范围内遍历
            while (startDay <= lastTime)
            {
                foreach (var hos in hoslist)
                {
                    Console.WriteLine(startDay.ToString("D") + " \t" + hos);
                    //结束时间
                    var endTime = startDay.AddDays(1).AddSeconds(-1);
                    using (var ctx = new Entities())
                    {
                        //日期范围内目标医院拒诊类型问诊数量
                        var tempData = ctx.SDTREATINFO
                            .Where(x => x.NETHOSID == hos && x.TREATSHEETTIME >= startDay && x.TREATSHEETTIME <= endTime)
                            .GroupBy(x => new { x.NETHOSID, x.REJECTTYPE })
                            .Select(x => new { Hos = x.Key.NETHOSID, RejectType = x.Key.REJECTTYPE, Count = x.Count() });

                        //遍历所有的拒诊类型
                        foreach (var item in tempData)
                        {
                            //拒诊类型 RejectType
                            decimal id = item.RejectType.HasValue ? (decimal)item.RejectType : 0;

                            //入库的实体
                            var data = new STATHOSDAYREJECTTREAT()
                            {
                                STATDAY = startDay,
                                HOSID = hos,
                                REJECTTYPEID = id,
                                REJECTTYPENAME = dic.ContainsKey(id) ? dic[id] : "不在配置中",//不在配置中直接存希望有人能发现并做出配置
                                REJECTCOUNT = item.Count,
                                UPDATETIME = DateTime.Now
                            };

                            //决定入库类型insert or update
                            int hasData = ctx.STATHOSDAYREJECTTREAT.Count(x => x.HOSID == hos && x.STATDAY == startDay && x.REJECTTYPEID == id);
                            if (hasData == 0)
                            {
                                ctx.Entry(data).State = EntityState.Added;
                            }
                            else
                            {
                                ctx.Entry(data).State = EntityState.Modified;
                            }
                            ctx.SaveChanges();
                        }
                    }
                }
                //按日期循环累加
                startDay = startDay.AddDays(1);
            }
            Console.WriteLine("计算完成: 医院日统计数据之拒诊");
        }
    }
}
