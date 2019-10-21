using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;

namespace WebServiceOld
{
    /// <summary>
    /// Demo 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Demo : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            string str = "<?xml version=\"1.0\" encoding=\"UTF - 8\" ?>" +
                         "<res service = \"BKCloud_ExeAppoint\">" +
                         "<resultCode>0</resultCode>" +
                         "<resultDesc>预约成功！</resultDesc>"+
                         "<hisOrderId>wx11110992989</hisOrderId>"+
                         "<orderMessage>12356</orderMessage>"+
                         "<realFee></realFee>"+
                         "<realFeeDesc></realFeeDesc>"+
                         "</res>";
            Request4ExeAppoint request = new Request4ExeAppoint();
            request.cardNo = "10001";
            request.denNo = "10002";
            request.sex = "nan";

            XmlSerializer serializer = new XmlSerializer(typeof(Request4ExeAppoint));
            StringBuilder stringBuilder = new StringBuilder();

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.OmitXmlDeclaration = true;
            xmlSerializerNamespaces.Add("", "");

            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
            {
                serializer.Serialize(xmlWriter, request, xmlSerializerNamespaces);
                System.Diagnostics.Debug.WriteLine(stringBuilder.ToString());

            }
            //   stringBuilder.Replace(typeof(T).Name, "req");

            return stringBuilder.ToString();

            //return str;
        }
    }
}
