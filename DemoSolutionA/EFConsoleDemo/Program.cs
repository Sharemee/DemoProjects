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
            //T1();

            T2();
            
            Console.ReadKey();
        }

        static void T1()
        {
            using (var ctx = new Entities())
            {
                var result = ctx.Students.ToList();

                UserInfo user = new UserInfo
                {
                    ID = 1,
                    Name = "Sun",
                    Age = 21
                };

                //ctx.UserInfo.Attach(t);//不会修改记录
                ctx.Entry(user).State = EntityState.Modified;

                ctx.SaveChanges();
            }
        }

        static void T2()
        {
           using(var ctx=new Entities())
            {
                UserInfo user = new UserInfo
                {
                    ID = 2,
                    Name = "Sun",
                    Age = 19
                };

                //ctx.UserInfo.Attach(user);
                ctx.Entry(user).State = EntityState.Added;

                //UserInfo data = ctx.Set<UserInfo>().Single(x => x.Name == "Sun");
                //ctx.UserInfo.Attach(data);
                //data.Age = 18;

                ctx.SaveChanges();
            }
        }
    }
}
