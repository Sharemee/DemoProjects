using BK.JXWSWX.OracleEFModel;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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
            int batchCount = 200;
            int rangeCount = 1;

            //医院日统计数据-综合
            StatHosDay(timeOutDay, batchCount, rangeCount);

            Console.WriteLine("数据计算完成");
            Console.ReadKey();
        }

        /// <summary>
        /// 医院日统计数据-综合
        /// </summary>
        /// <param name="timeOut">超时时间</param>
        /// <param name="batchCount">批次处理数量</param>
        /// <param name="rangeCount">物理线程数</param>
        private static void StatHosDay(int timeOut, int batchCount, int rangeCount)
        {
            DateTime startDay = DateTime.Now.AddDays(-1 * timeOut).Date;
            DateTime TempTime = startDay;
            DateTime endDay = DateTime.Now.AddDays(-1).Date;//最后时间是昨天

            var hoslist = GetRegHos();
            if (hoslist.Count == 0)
            {
                //LogHelper.Info(log, "互联网医院列表为空, StatHosDay-统计结束");
                return;
            }
            
            foreach (var hos in hoslist)
            {
                Console.WriteLine("目标医院ID: " + hos);

                //按日期循环累加
                while (startDay <= endDay)
                {
                    //医院日统计数据-综合 StatHosDay
                    using (var ctx = new Entities())
                    {
                        Console.WriteLine(startDay + ": " + hos);

                        //先检查有没有目标日期和医院ID的数据(唯一)
                        STATHOSDAY data = ctx.STATHOSDAY.AsNoTracking().FirstOrDefault(x => x.HOSID == hos && x.STATDAY == startDay);

                        //获取目标统计数据
                        var tem = GetStatHosDayDate(hos, startDay);

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
                        startDay = startDay.AddDays(1);
                    }
                }
                startDay = TempTime;//重置时间, 时间循环是医院为周期的(一个医院循环一次)
            }
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
                //LogHelper.Error(log, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 获取互联网医院异常GetRegHos", ex);
            }
            return result;
        }

        /// <summary>
        /// 计算响应日期的数据
        /// </summary>
        /// <param name="hosId"></param>
        /// <param name="StartTime"></param>
        /// <returns></returns>
        private static STATHOSDAY GetStatHosDayDate(decimal hosId, DateTime StartTime)
        {
            STATHOSDAY result = new STATHOSDAY();
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
                //购买药品处方量 GROUP BY 是因为一张处方单可能有多个付款记录, 用户可以一个药品一个药品支付
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

            return result;
        }


        private static void Testc()
        {
            Entities ctx = new Entities();
            var hos = 30666;
            var tempData = ctx.SDTREATINFO
                            .Where(x => x.NETHOSID == hos) //&& x.TREATSHEETTIME >= startDay && x.TREATSHEETTIME <= endTime
                            .GroupBy(x => new { x.NETHOSID, x.REJECTTYPE })
                            .Select(x => new {
                                Hos = x.Key.NETHOSID,
                                RejectType = x.Key.REJECTTYPE,
                                Count = x.Count()
                            });
        }
    }
}
