using System;
using System.Collections.Generic;
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
    class ThirdPartyAssignmentService : ServiceBase
    {
        private readonly string cssHeader = "tpaStyle.css";

        String dataSource = @"MagProductTree.xml";
        String uri = @"http://localhost:9000/";
        System.Xml.XmlDocument productTree = new System.Xml.XmlDocument();

        public ThirdPartyAssignmentService()
        {
            //loadProductTree();
        }

        [OperationContract]
        [WebGet(UriTemplate = "view/{soldTo}", BodyStyle = WebMessageBodyStyle.Bare)]
        Stream viewAssignmentSoldTo(String soldTo)
        {

            XmlDocument result = new XmlDocument();

            result.InsertBefore(result.CreateXmlDeclaration("1.0", "UTF-8", null), result.DocumentElement);
            result.AppendChild(result.CreateElement("MaterialAssignments"));
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(archiveDbString))
            {
                System.Data.SqlClient.SqlCommand getAssignments = new System.Data.SqlClient.SqlCommand("SELECT [MaterialNumber] ,[Assignment].query('/Customer[@SoldTo = sql:variable(\"@soldTo\")]') FROM [ThirdParty].[dbo].[MaterialAssignments] where Assignment.exist('/Customer[@SoldTo = sql:variable(\"@soldTo\")]') > 0", con);
                getAssignments.Parameters.AddWithValue("@soldto", soldTo);
                con.Open();
                System.Data.SqlClient.SqlDataReader dr = getAssignments.ExecuteReader();
                while (dr.Read())
                {
                    String matColor = dr.GetString(0);
                    XmlNode matNode = result.SelectSingleNode("//Assignment[@MaterialColor = \"" + matColor + "\"]");

                    if (matNode == null)
                    {
                        matNode = result.CreateNode(XmlNodeType.Element, "Assignment", string.Empty);
                        XmlAttribute matName = result.CreateAttribute("MaterialColor");
                        matName.InnerText = matColor;
                        matNode.Attributes.Append(matName);
                    }
                    XmlReader xr = dr.GetSqlXml(1).CreateReader();
                    xr.ReadToDescendant("UPC");
                    do
                    {
                        XmlNode xn = result.CreateNode(XmlNodeType.Element, "UPC", string.Empty);
                        xr.MoveToAttribute("Size");
                        XmlAttribute sizeAttr = result.CreateAttribute("Size");
                        sizeAttr.InnerText = xr.ReadContentAsString();
                        xn.Attributes.Append(sizeAttr);
                        xr.MoveToContent();
                        xn.InnerText = xr.ReadElementContentAsString();
                        if (xn.InnerXml.Length > 0)
                        {
                            matNode.AppendChild(xn);
                        }
                    } while (xr.MoveToContent() == XmlNodeType.Element);

                    result.DocumentElement.AppendChild(matNode);
                }

                con.Close();
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);
            foreach (XmlNode resultAssignment in result.SelectNodes("//Assignment"))
            {
                rs.AddAttribute("class", "Assignment");
                rs.RenderBeginTag(HtmlTextWriterTag.Div);
                rs.WriteEncodedText(resultAssignment.Attributes["MaterialColor"].InnerText);
                foreach (XmlNode resultUpc in resultAssignment.SelectNodes("UPC"))
                   renderUPCNode(resultUpc, rs);
                rs.RenderEndTag();//Assignment
            }



            return wrapHtmlOutput("View Assignment", sb.ToString());
        }

        [OperationContract]
        [WebGet(UriTemplate = "view/{soldTo}/{upc}", BodyStyle = WebMessageBodyStyle.Bare)]
        Stream viewAssignmentUPC(String soldTo, String upc)
        {

            XmlDocument result = new XmlDocument();

            result.InsertBefore(result.CreateXmlDeclaration("1.0", "UTF-8", null), result.DocumentElement);
            result.AppendChild(result.CreateElement("Assignment"));
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(archiveDbString))
            {
                System.Data.SqlClient.SqlCommand getAssignments = new System.Data.SqlClient.SqlCommand("SELECT [MaterialNumber] ,[Assignment].query('/Customer[@SoldTo = sql:variable(\"@soldTo\")]/UPC[text() = sql:variable(\"@upc\")]') FROM [ThirdParty].[dbo].[MaterialAssignments] where Assignment.exist('/Customer[@SoldTo = sql:variable(\"@soldTo\")][UPC = sql:variable(\"@upc\")]') > 0", con);
                //System.Data.SqlClient.SqlCommand getStatus = new System.Data.SqlClient.SqlCommand("SELECT top 10 Bridge.query('InboundASNBridge/InboundASN[Warehouse = sql:variable(\"@pt\")]') FROM [dbo].[WMiArchive] with (nolock) where Bridge.exist('InboundASNBridge/InboundASN[Warehouse = sql:variable(\"@pt\")]') > 0 AND Filename like 'I8%' order by ProcessedTime DESC", con);
                getAssignments.Parameters.AddWithValue("@soldto", soldTo);
                getAssignments.Parameters.AddWithValue("@upc", upc);
                con.Open();
                System.Data.SqlClient.SqlDataReader dr = getAssignments.ExecuteReader();
                while (dr.Read())
                {
                    XmlReader xr = dr.GetSqlXml(1).CreateReader();
                    xr.ReadToDescendant("UPC");
                    do
                    {
                        XmlNode xn = result.CreateNode(XmlNodeType.Element, "UPC", string.Empty);
                        xr.MoveToAttribute("Size");
                        XmlAttribute sizeAttr = result.CreateAttribute("Size");
                        sizeAttr.InnerText = xr.ReadContentAsString();
                        xn.Attributes.Append(sizeAttr);
                        //if (xr.MoveToContent() != XmlNodeType.Element)
                        //    continue;
                        xr.MoveToContent();
                        xn.InnerText = xr.ReadElementContentAsString();
                        //result.DocumentElement.AppendChild(result.ImportNode(xr.ReadOuterXml(), true));
                        //                        xn.Attributes.Append(xr.rea
                        if (xn.InnerXml.Length > 0)
                        {
                            result.DocumentElement.AppendChild(xn);
                        }
                    } while (xr.MoveToContent() == XmlNodeType.Element);

                }

                con.Close();
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);
            foreach (XmlNode resultUpc in result.SelectNodes("//UPC"))
            {
                renderUPCNode(resultUpc, rs);
            }



            return wrapHtmlOutput("View Assignment", sb.ToString());
        }

        private void renderUPCNode(XmlNode resultUpc, HtmlTextWriter rs)
        {
            if (resultUpc.Name != "UPC")
                throw new Exception("expected node name UPC got " + resultUpc.Name);

            rs.AddAttribute("class", "Size");
            rs.RenderBeginTag(HtmlTextWriterTag.Div);
            rs.WriteEncodedText(resultUpc.Attributes["Size"].InnerText + " ");

            rs.AddAttribute("class", "UPC");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.RenderBeginTag(HtmlTextWriterTag.Code);
            rs.WriteEncodedText(resultUpc.InnerText);
            rs.RenderEndTag();
            rs.RenderEndTag();
            rs.RenderEndTag();
            rs.WriteBreak();

            return;
        }


        private void renderSizeNode(XmlNode sizeNode, HtmlTextWriter rs)
        {
            if (sizeNode.Name != "UPC")
                throw new Exception("expected node name UPC got " + sizeNode.Name);

            rs.AddAttribute("class", "UPC");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(sizeNode.Attributes["Size"].InnerText + " ");
            rs.RenderBeginTag(HtmlTextWriterTag.Code);
            rs.WriteEncodedText(sizeNode.InnerText);
            rs.RenderEndTag();
            rs.RenderEndTag();
            rs.WriteBreak();

            return;
        }

        private void renderAssigment(XmlNode materialAssignment, HtmlTextWriter rs)
        {
            if (materialAssignment.Name != "UPC")
                throw new Exception("expected node name UPC got " + materialAssignment.Name);

            rs.AddAttribute("class", "UPC");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(materialAssignment.Attributes["Size"].InnerText + " ");
            rs.RenderBeginTag(HtmlTextWriterTag.Code);
            rs.WriteEncodedText(materialAssignment.InnerText);
            rs.RenderEndTag();
            rs.RenderEndTag();
            rs.WriteBreak();

            return;
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
