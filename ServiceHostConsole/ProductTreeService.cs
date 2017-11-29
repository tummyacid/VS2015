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
    partial class ProductTreeService : ServiceBase
    {
        System.Xml.XmlDocument productTree = new System.Xml.XmlDocument();

        static String cssHeader = @"style.css";

        public ProductTreeService()
        {
            loadProductTree();
        }

        [OperationContract]
        [WebGet(UriTemplate = "ProductTree", BodyStyle = WebMessageBodyStyle.Bare)]

        public System.IO.Stream ProductTree()
        {
            WebRequest client = System.Net.WebRequest.Create(@"http://");

            client.Timeout = 600000; //5 minutes 3 should be enough though...
            client.Method = "GET";

            DateTime runtime = DateTime.Now;


            //try
            //{
            //    System.Net.WebResponse resp = (HttpWebResponse)client.GetResponse();

            //    if (resp == null)
            //        throw new WebException("null response from web service");


            //    AvailabiltyFeed.Load(resp.GetResponseStream()); //Potential memory leak if this fails
            //    resp.Close();

            //    AvailabiltyFeed.Save("cache.xml");
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}



            StringBuilder sb = new StringBuilder();

            sb.AppendLine(writeProductTree(productTree));

            //byte[] bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());

            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return wrapHtmlOutput("ProductTree", sb.ToString());
            //return new System.IO.MemoryStream(bytes);
        }
        [OperationContract]
        [WebGet(UriTemplate = "view", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream viewAllMaterials()
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);



            foreach (XmlNode xn in productTree.SelectNodes("//material"))
            {
                rs.AddAttribute("class", "material");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);


                rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}", xn.Attributes["name"].InnerText));
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
                rs.WriteEncodedText(xn.Attributes["name"].InnerText);
                rs.RenderEndTag(); //A

                rs.WriteBreak();
                rs.RenderEndTag();
            }


            //byte[] bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());

            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return wrapHtmlOutput("Materials", sb.ToString());
            //return new System.IO.MemoryStream(bytes);
        }
        [OperationContract]
        [WebGet(UriTemplate = "viewLeaf?logo={logo}", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream viewLogoNode(String logo)
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);
            foreach (XmlNode xn in productTree.SelectNodes("//logo[@name=\"" + logo + "\"]"))
                renderLogoNode(xn, rs);

            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return wrapHtmlOutput("Logo view " + logo, sb.ToString());
        }
        [OperationContract]
        [WebGet(UriTemplate = "view/{material}", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream viewMaterial(String material)
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);
            foreach (XmlNode xn in productTree.SelectNodes("//material[@name=\"" + material + "\"]"))
                renderMaterialNode(xn, rs);

            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return wrapHtmlOutput("Material view " + material, sb.ToString());
        }
        [OperationContract]
        [WebGet(UriTemplate = "view/{material}/{color}", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream viewMaterialColor(String material, String color)
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);
            foreach (XmlNode xn in productTree.SelectNodes("//material[@name=\"" + material + "\"]/color[@name=\"" + color + "\"]"))
                renderColorNode(xn, rs);

            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return wrapHtmlOutput("Color view " + material, sb.ToString());
        }
        [OperationContract]
        [WebGet(UriTemplate = "view/{material}/{color}/{size}", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream viewMaterialColorSize(String material, String color, String size)
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);
            foreach (XmlNode xn in productTree.SelectNodes("//material[@name=\"" + material + "\"]/color[@name=\"" + color + "\"]/size[@name=\"" + size + "\"]"))
                renderSizeNode(xn, rs);

            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return wrapHtmlOutput("Size view " + material, sb.ToString());
        }
        [OperationContract]
        [WebGet(UriTemplate = "view/{material}/{color}/{size}/{logo}", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream viewMaterialColorSizeLogo(String material, String color, String size, String logo)
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);
            foreach (XmlNode xn in productTree.SelectNodes("//material[@name=\"" + material + "\"]/color[@name=\"" + color + "\"]/size[@name=\"" + size + "\"]/logo[@name=\"" + logo + "\"]"))
                renderLogoNode(xn, rs);

            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return wrapHtmlOutput("Logo view " + material, sb.ToString());
        }

        public void loadProductTree()
        {
            productTree.Load(productTreeSrc);
        }
        private string writeProductTree(XmlDocument productTree)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Web.UI.HtmlTextWriter rs = new HtmlTextWriter(sw);

            foreach (XmlNode mat in productTree.SelectNodes("//material"))
                renderMaterialNode(mat, rs);

            return sb.ToString();
        }
        private void renderMaterialNode(XmlNode dc, HtmlTextWriter rs)
        {
            if (dc.Name != "material")
                throw new Exception("expected node name material got " + dc.Name);

            rs.AddAttribute("class", "material");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(dc.Attributes["name"].InnerText);
            rs.WriteBreak();
            foreach (XmlNode colorNode in dc.SelectNodes("color"))
            {
                rs.AddAttribute("class", "color");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}/{1}", dc.Attributes["name"].InnerText, colorNode.Attributes["name"].InnerText));
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
                rs.WriteEncodedText(colorNode.Attributes["name"].InnerText);
                rs.RenderEndTag(); //A
                rs.WriteBreak();
                foreach (XmlNode sizeNode in colorNode.SelectNodes("size"))
                {
                    rs.AddAttribute("class", "size");
                    rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                    rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}/{1}/{2}"
                        , dc.Attributes["name"].InnerText
                        , colorNode.Attributes["name"].InnerText
                        , sizeNode.Attributes["name"].InnerText));
                    rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
                    rs.WriteEncodedText(sizeNode.Attributes["name"].InnerText);
                    rs.RenderEndTag(); //A

                    rs.WriteBreak();
                    foreach (XmlNode logoNode in sizeNode.SelectNodes("logo"))
                    {
                        rs.AddAttribute("class", "logo");
                        rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                        //rs.WriteEncodedText(logoNode.Attributes["name"].InnerText);
                        rs.RenderEndTag();
                    }
                    rs.RenderEndTag();
                }
                rs.RenderEndTag();
            }
            rs.RenderEndTag();

            return;
        }
        private void renderColorNode(XmlNode colorNode, HtmlTextWriter rs)
        {
            rs.AddAttribute("class", "material");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}"
              , colorNode.ParentNode.Attributes["name"].InnerText));
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
            rs.WriteEncodedText(colorNode.ParentNode.Attributes["name"].InnerText);
            rs.RenderEndTag(); //A


            rs.AddAttribute("class", "color");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);


            rs.WriteEncodedText(colorNode.Attributes["name"].InnerText);
            rs.WriteBreak();

            foreach (XmlNode sizeNode in colorNode.SelectNodes("size"))
            {
                rs.AddAttribute("class", "size");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}/{1}/{2}"
                    , colorNode.ParentNode.Attributes["name"].InnerText
                    , colorNode.Attributes["name"].InnerText
                    , sizeNode.Attributes["name"].InnerText));
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
                rs.WriteEncodedText(sizeNode.Attributes["name"].InnerText);
                rs.RenderEndTag(); //A
                rs.WriteBreak();
                foreach (XmlNode logoNode in sizeNode.SelectNodes("logo"))
                {
                    rs.AddAttribute("class", "logo");
                    rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                    //rs.WriteEncodedText(logoNode.Attributes["name"].InnerText);
                    rs.RenderEndTag();
                }
                rs.RenderEndTag();//size
            }

            rs.RenderEndTag(); //color

            rs.RenderEndTag(); //material

        }
        private void renderSizeNode(XmlNode sizeNode, HtmlTextWriter rs)
        {
            rs.AddAttribute("class", "material");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}"
              , sizeNode.ParentNode.ParentNode.Attributes["name"].InnerText));
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
            rs.WriteEncodedText(sizeNode.ParentNode.ParentNode.Attributes["name"].InnerText);
            rs.RenderEndTag(); //A

            rs.AddAttribute("class", "material");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}/{1}"
              , sizeNode.ParentNode.ParentNode.Attributes["name"].InnerText
              , sizeNode.ParentNode.Attributes["name"].InnerText));
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
            rs.WriteEncodedText(sizeNode.ParentNode.Attributes["name"].InnerText);
            rs.RenderEndTag(); //A


            rs.AddAttribute("class", "leaf-size");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            rs.WriteEncodedText(sizeNode.Attributes["name"].InnerText);
            foreach (XmlNode logoNode in sizeNode.SelectNodes("logo"))
            {
                rs.AddAttribute("class", "leaf-logo");
                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
                rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}/{1}/{2}/{3}"
                    , sizeNode.ParentNode.ParentNode.Attributes["name"].InnerText
                    , sizeNode.ParentNode.Attributes["name"].InnerText
                    , sizeNode.Attributes["name"].InnerText
                    , logoNode.Attributes["name"].InnerText));

                rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
                rs.WriteEncodedText(logoNode.Attributes["name"].InnerText);
                rs.RenderEndTag(); //A

                rs.RenderEndTag();
            }

            rs.RenderEndTag(); //leaf-size

            rs.RenderEndTag(); //color

            rs.RenderEndTag(); //material

        }
        private void renderLogoNode(XmlNode logoNode, HtmlTextWriter rs)
        {
            if (logoNode.Name != "logo")
                throw new Exception("expected node name logo got " + logoNode.Name);


            rs.AddAttribute("class", "leaf-material");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}"
              , logoNode.ParentNode.ParentNode.ParentNode.Attributes["name"].InnerText));
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
            rs.WriteEncodedText(logoNode.ParentNode.ParentNode.ParentNode.Attributes["name"].InnerText);
            rs.RenderEndTag(); //A

            rs.AddAttribute("class", "color");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}/{1}"
              , logoNode.ParentNode.ParentNode.ParentNode.Attributes["name"].InnerText
              , logoNode.ParentNode.ParentNode.Attributes["name"].InnerText));
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
            rs.WriteEncodedText(logoNode.ParentNode.ParentNode.Attributes["name"].InnerText);
            rs.RenderEndTag(); //A

            rs.AddAttribute("class", "size");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            rs.AddAttribute("href", String.Format(productTreeUri + "view/{0}/{1}/{2}"
              , logoNode.ParentNode.ParentNode.ParentNode.Attributes["name"].InnerText
              , logoNode.ParentNode.ParentNode.Attributes["name"].InnerText
              , logoNode.ParentNode.Attributes["name"].InnerText));
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
            rs.WriteEncodedText(logoNode.ParentNode.Attributes["name"].InnerText);
            rs.RenderEndTag(); //A

            rs.AddAttribute("class", "leaf-logo");
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div); //
            rs.AddAttribute("href", String.Format(productTreeUri + "viewLeaf?logo={0}", logoNode.Attributes["name"].InnerText));
            rs.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
            rs.WriteEncodedText(logoNode.Attributes["name"].InnerText);
            rs.RenderEndTag(); //A

            rs.RenderBeginTag(HtmlTextWriterTag.Code);
            rs.WriteBreak();
            rs.WriteEncodedText("SKU: " + logoNode.ParentNode.ParentNode.ParentNode.Attributes["name"].InnerText
                                        + logoNode.ParentNode.ParentNode.Attributes["name"].InnerText
                                        + logoNode.ParentNode.Attributes["name"].InnerText
                                        + logoNode.Attributes["name"].InnerText);

            rs.WriteBreak();
            rs.WriteEncodedText("IMG: " + logoNode.ParentNode.ParentNode.ParentNode.Attributes["name"].InnerText
                                        + logoNode.ParentNode.ParentNode.Attributes["name"].InnerText 
                                        + logoNode.Attributes["name"].InnerText
                                        + "_" + logoNode.ParentNode.ParentNode.Attributes["name"].InnerText + "_"
                                        + "MANN_HG.jpg");
            rs.RenderEndTag(); //small
            rs.RenderEndTag(); //leaf-logo

            rs.RenderEndTag(); //size

            rs.RenderEndTag(); //color

            rs.RenderEndTag(); //material

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
