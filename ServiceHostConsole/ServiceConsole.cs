using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Web.UI;
using System.Xml;

namespace ServiceHostConsole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceConsole" in both code and config file together.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single)]
    [ServiceContract]
    partial class ConsoleService : ServiceBase
    {
        static String cssHeader = @"style.css";

        private void WriteAvailRow(XmlNode dc, System.Web.UI.HtmlTextWriter rs)
        {
            if (dc.Name != "AvailabiltyFeed")
                throw new Exception("expected node name AvailabiltyFeed got " + dc.Name);

            rs.AddAttribute("class", "tab-row");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            rs.AddAttribute("class", "cell");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(dc["UPC"].InnerText);
            rs.RenderEndTag();

            rs.AddAttribute("class", "cell");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(dc["MaterialDescription"].InnerText);
            rs.RenderEndTag();

            rs.AddAttribute("class", "cell");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(dc["OnHand"] == null? string.Empty : dc["OnHand"].InnerText);
            rs.RenderEndTag();

            rs.AddAttribute("class", "cell");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(dc["Future"] == null ? string.Empty : dc["Future"].InnerText);
            rs.RenderEndTag();

            rs.AddAttribute("class", "cell");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(dc["Scheduled"] == null ? string.Empty : dc["Scheduled"].InnerText.Split('T')[0]);
            rs.RenderEndTag();

            rs.RenderEndTag();
        }
        public String writeAvailTable(XmlDocument source)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter stringWriter = new System.IO.StringWriter(sb);
            using (System.Web.UI.HtmlTextWriter rs = new System.Web.UI.HtmlTextWriter(stringWriter))
            {
                rs.AddAttribute("class", "div-table");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

                //Header
                rs.AddAttribute("class", "tab-row");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

                rs.AddAttribute("class", "cell");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.WriteEncodedText("UPC");
                rs.RenderEndTag();

                rs.AddAttribute("class", "cell");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.WriteEncodedText("MaterialDescription");
                rs.RenderEndTag();

                rs.AddAttribute("class", "cell");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.WriteEncodedText("OnHand");
                rs.RenderEndTag();

                rs.AddAttribute("class", "cell");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.WriteEncodedText("Future");
                rs.RenderEndTag();

                rs.AddAttribute("class", "cell");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.WriteEncodedText("Scheduled");
                rs.RenderEndTag();

                rs.RenderEndTag();
                rs.WriteBreak();
                foreach (XmlNode dc in source.SelectNodes("//AvailabiltyFeed"))
                {
                    WriteAvailRow(dc, rs);
                    rs.WriteBreak();
                }

                rs.RenderEndTag();
            }
            return sb.ToString();
        }
    }

}
