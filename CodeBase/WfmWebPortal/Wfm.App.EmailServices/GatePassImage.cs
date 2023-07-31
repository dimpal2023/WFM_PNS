using System.Drawing;
using Wfm.App.Core.Model;
using System.Web.UI;
using System.IO;
using System.Drawing.Imaging;
using System.Web;

namespace Wfm.App.EmailServices
{
    public class GatePassImage
    {
        //public string WriteGatePassContentToImage(GatePassMetaData gatepass)
        //{
        //    StringWriter strWriter = new StringWriter();
        //    HtmlTextWriter writer = new HtmlTextWriter(strWriter);

        //    WriteHtml(ref writer, gatepass);

        //    return ConvertHtmlToImage(strWriter.ToString(), gatepass);
        //}

        //private string ConvertHtmlToImage(string htmlString, GatePassMetaData gatepass)
        //{
        //    HtmlToPdfConverter converter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);            

        //    WebKitConverterSettings webKitSettings = new WebKitConverterSettings();
        //    webKitSettings.PdfPageSize = new SizeF(new PointF(300, 600));
        //    webKitSettings.WebKitPath = @"E:\Projects\Research\packages\Syncfusion.HtmlToPdfConverter.QtWebKit.WinForms.18.4.0.30\lib\QtBinaries";          
        //    converter.ConverterSettings = webKitSettings;            
        //    Image[] image = converter.ConvertToImage(htmlString, string.Empty);            
        //    string path = HttpContext.Current.Server.MapPath("~/Content/assets/images/");
        //    string mappedPath = string.Format("{0}{1}.jpg", path, gatepass.WORKFORCE.FULL_NAME.Trim());
        //    image[0].Save(mappedPath);
        //    return mappedPath;
        //}

        //private string GenerateMyQCCode(string QCText)
        //{
        //    var QCwriter = new BarcodeWriter();
        //    QCwriter.Format = BarcodeFormat.QR_CODE;
        //    var result = QCwriter.Write(QCText);
        //    string path = "E:\\MyQRImage.jpg";
        //    var barcodeBitmap = new Bitmap(result);

        //    using (MemoryStream memory = new MemoryStream())
        //    {
        //        using (FileStream fs = new FileStream(path,
        //           FileMode.Create, FileAccess.ReadWrite))
        //        {
        //            barcodeBitmap.Save(memory, ImageFormat.Jpeg);
        //            byte[] bytes = memory.ToArray();
        //            fs.Write(bytes, 0, bytes.Length);
        //        }
        //    }
        //    return path;
        //}

        //private void WriteHtml(ref HtmlTextWriter writer, GatePassMetaData gatepass)
        //{
        //    writer.RenderBeginTag(HtmlTextWriterTag.Html);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Head);

        //    writer.RenderBeginTag(HtmlTextWriterTag.Style);
        //    writer.Write("table {  font-family: arial, sans-serif; border-collapse: collapse; width: 50%; } td, th {  text-align:left;  padding:8px;  }");
        //    writer.RenderEndTag(); //End Style           

        //    writer.RenderBeginTag(HtmlTextWriterTag.Body);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Table);

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align:center");
        //    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>ePass</b>");
        //    writer.RenderEndTag();
        //    writer.RenderEndTag();

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Style, "text-align:center");
        //    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Src, GenerateMyQCCode(gatepass.WORKFORCE.FULL_NAME + gatepass.START_DATE));
        //    writer.RenderBeginTag(HtmlTextWriterTag.Img);
        //    writer.RenderEndTag(); //End img
        //    writer.RenderEndTag(); //End td
        //    writer.RenderEndTag(); //End tr

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);            
        //    writer.Write("Gate Pass Details");
        //    writer.RenderEndTag();           
        //    writer.RenderEndTag();

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);                      
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>Name</b>");
        //    writer.RenderEndTag(); // End td
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write(gatepass.WORKFORCE.FULL_NAME);
        //    writer.RenderEndTag(); // End td
        //    writer.RenderEndTag(); // End tr

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>Start Date</b>");
        //    writer.RenderEndTag(); // End td
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write(gatepass.START_DATE.ToShortDateString());
        //    writer.RenderEndTag(); // End td
        //    writer.RenderEndTag(); // End tr

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>End Date</b>");
        //    writer.RenderEndTag(); // End td
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write(gatepass.END_DATE.ToShortDateString());
        //    writer.RenderEndTag(); // End td
        //    writer.RenderEndTag(); // End tr

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>Out Time</b>");
        //    writer.RenderEndTag(); // End td
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write(gatepass.OUT_TIME.ToString());
        //    writer.RenderEndTag(); // End td
        //    writer.RenderEndTag(); // End tr

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>In Time</b>");
        //    writer.RenderEndTag(); // End td
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write(gatepass.IN_TIME.ToString());
        //    writer.RenderEndTag(); // End td
        //    writer.RenderEndTag(); // End tr

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>Email</b>");
        //    writer.RenderEndTag(); // End td
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write(gatepass.WORKFORCE.EMAIL);
        //    writer.RenderEndTag(); // End td
        //    writer.RenderEndTag(); // End tr

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>Phone No</b>");
        //    writer.RenderEndTag(); // End td
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write(gatepass.WORKFORCE.PHONE_NO);
        //    writer.RenderEndTag(); // End td
        //    writer.RenderEndTag(); // End tr

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>Purpose</b>");
        //    writer.RenderEndTag(); // End td
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write(gatepass.PURPOSE);
        //    writer.RenderEndTag(); // End td
        //    writer.RenderEndTag(); // End tr

        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write("<b>Remark</b>");
        //    writer.RenderEndTag(); // End td
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    writer.Write(gatepass.REMARK);
        //    writer.RenderEndTag(); // End td
        //    writer.RenderEndTag(); // End tr
            
        //    writer.RenderEndTag(); // End table
        //    writer.RenderEndTag(); // End body

        //    writer.RenderEndTag(); // End head
        //    writer.RenderEndTag(); //End html
        //}
    }
}
