using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationTest.Models;

namespace WebApplicationTest.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var list = GetData();
            ViewBag.Student = list;
            return View();
        }

        private IEnumerable<SelectListItem> GetData()
        {
            List<Student> list = new List<Student>
            {
                new Student{Id=1,Name="Sun_001",Age=15},
                new Student{Id=2,Name="Sun_002",Age=17},
                new Student{Id=3,Name="Sun_003",Age=19}
            };

            List<SelectListItem> li = new List<SelectListItem>();
            foreach (var item in list)
            {
                li.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            Random random = new Random();
            //li[random.Next(0, 4)].Selected = true;
            li[1].Selected = true;
            return li;
        }

        /// <summary>
        /// 获取颜色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetColorJson(string ParentID)
        {
            List<Color> list = new List<Color>();
            list.Add(new Color { Code = 10, ColorName = "红色" });
            list.Add(new Color { Code = 20, ColorName = "黄色" });
            list.Add(new Color { Code = 30, ColorName = "蓝色" });
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (var item in list)
            {
                result.Add(new SelectListItem { Text = item.Code.ToString(), Value = item.ColorName });
            }

            IEnumerable<SelectListItem> color = result;
            return Json(color, JsonRequestBehavior.AllowGet);
        }
    }
}