using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlServerEFModel;

namespace EFConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //插入数据
            //T1();

            T2();
            
            Console.ReadKey();
        }

        static void T1()
        {
            using (var ctx = new Entities())
            {
                var result = ctx.Students.ToList();

                UserInfo t = new UserInfo
                {
                    ID = 1,
                    Name = "Sun",
                    Age = 21
                };

                //ctx.UserInfo.Attach(t);//不会修改记录
                ctx.Entry(t).State = EntityState.Modified;

                ctx.SaveChanges();
            }
        }

        static void T2()
        {
           using(var ctx=new Entities())
            {
                var data = ctx.Set<UserInfo>().Single(x => x.Name == "Sun");
                ctx.UserInfo.Attach(data);
                data.Age = 18;
                ctx.SaveChanges();
            }
        }
    }
}
