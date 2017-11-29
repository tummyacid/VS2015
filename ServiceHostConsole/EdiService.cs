using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml;

namespace ServiceHostConsole
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single)]
    [ServiceContract]
    partial class EdiService : ServiceBase
    {


        static String cssHeader = @"style.css";

        public EdiService()
        {
        }



        [OperationContract]
        [WebGet(UriTemplate = "order/{soldTo}/{PONumber}", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream viewOrder(string soldTo, string PONumber)
        {

            DataTable CB_PO_Item_Detail = new DataTable();
            DataRow CB_PO_Header;
            DataRow vContact;
            Queue<DataRow> canceledDetails = new Queue<DataRow>();



            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);
            //renderOrder(CB_PO_Header, vContact, CB_PO_Item_Detail, rs);
            renderOrder(CB_PO_Header, CB_PO_Header, CB_PO_Item_Detail, rs);
            return wrapHtmlOutput("Edi Order " + PONumber, sb.ToString());
                
        }

        private void renderOrder(DataRow CB_PO_Header, DataRow vContact, DataTable CB_PO_Item_Detail, HtmlTextWriter rs)
        {
            rs.AddAttribute("class", "InboundOrder");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            //rs.AddAttribute("class", "tab-row");
            //rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.AddAttribute("class", "InboundOrderHeader");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(CB_PO_Header["Sold_to_Customer_Number"].ToString().Trim());
            rs.RenderEndTag();

            rs.AddAttribute("class", "InboundOrderHeader");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(CB_PO_Header["PO_Number"].ToString().Trim());
            rs.RenderEndTag();

            rs.AddAttribute("class", "InboundOrderHeader");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(CB_PO_Header["PO_Date"].ToString().Trim());
            rs.RenderEndTag();

            //          rs.RenderEndTag(); //tab-row

            rs.AddAttribute("action", String.Format("/Edi/apply/{0}/{1}", CB_PO_Header["Sold_to_Customer_Number"].ToString().Trim(), CB_PO_Header["PO_Number"].ToString().Trim()));
            rs.RenderBeginTag(HtmlTextWriterTag.Form);
            foreach (DataRow dr in CB_PO_Item_Detail.Select("", "PO_Line_Number"))
            {

                rs.AddAttribute("class", "InboundOrderLineItem");
                rs.AddAttribute("data-poLine", dr["PO_Line_Number"].ToString().Trim());
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);


                rs.AddAttribute("class", "cell");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.AddAttribute("type", "checkbox");
                rs.AddAttribute("name", "includePOLine");
                rs.AddAttribute("value", dr["PO_Line_Number"].ToString().Trim());
                rs.RenderBeginTag(HtmlTextWriterTag.Input);
                rs.RenderEndTag();
                rs.RenderEndTag();
                rs.AddAttribute("class", "cell");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.WriteEncodedText(dr["PO_Line_Number"].ToString().Trim());
                rs.RenderEndTag();
                rs.AddAttribute("class", "UPC");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.WriteEncodedText(dr["UPC"].ToString().Trim());
                rs.RenderEndTag();
                rs.AddAttribute("class", "cell");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.WriteEncodedText(dr["Quantity_Ordered"].ToString().Trim());
                rs.RenderEndTag();

                rs.RenderEndTag(); //detail-row
            }
            rs.AddAttribute("type", "submit");
            rs.AddAttribute("value", "Submit 870");
            rs.RenderBeginTag(HtmlTextWriterTag.Input);
            rs.RenderEndTag(); //Input
            rs.RenderEndTag(); //form

            rs.RenderEndTag(); //InboundOrder

            return;
        }





        [OperationContract]
        [WebGet(UriTemplate = "apply/{soldTo}/{PONumber}?includePOLine={includePOLine}", BodyStyle = WebMessageBodyStyle.Bare)]

        public System.IO.Stream apply(string soldTo, string PONumber, string includePOLine)
        {
            StringBuilder sb = new StringBuilder();


            //byte[] bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());

            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return wrapHtmlOutput("Apply Order " + soldTo, string.Empty);
          //      , sb.ToString());
            //return new System.IO.MemoryStream(bytes);
        }
        

        public Stream wrapHtmlOutput(String title, String body)
        {
            Encoding encloding = Encoding.UTF8;
            int bufferSize = 1024;
            MemoryStream ms = new MemoryStream();
            using (StreamWriter sr = new StreamWriter(ms, encloding, bufferSize, true))
            using (System.Web.UI.HtmlTextWriter rs = new System.Web.UI.HtmlTextWriter(sr))
            {
                byte[] header;

                try
                {
                    header = File.ReadAllBytes(cssHeader);
                }
                catch (Exception)
                {

                    throw new FileNotFoundException(cssHeader);
                }
                //sr.Write("<style type=\"text/css\">/* <![CDATA[ */");
                sr.Write("<style type=\"text/css\">/* <![CDATA[ */");
                sr.Write(encloding.GetString(header));
                sr.WriteLine(" ]]>");
                sr.WriteLine("</style>");

                rs.RenderBeginTag("html");
                rs.WriteFullBeginTag("title");
                rs.WriteEncodedText(title);
                rs.WriteEndTag("title");
                rs.WriteFullBeginTag("body");

                sr.WriteLine(body);

                rs.WriteEndTag("body");
                rs.WriteEndTag("html");
                ms.Flush();
            }
            ms.Seek(0, SeekOrigin.Begin);

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return ms;
        }
    }

}
