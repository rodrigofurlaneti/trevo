using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Drawing.Text;
using System.Web.Hosting;

namespace Entidade.Uteis.BarCode39
{
    public class BarCode
    {
        public string GenerateBarCode(string barcode)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Bitmap bitMap = new Bitmap(barcode.Length * 28, 80))
                {
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        string fontFile = HostingEnvironment.MapPath("~/fonts/IDAutomationHC39M.TTF");

                        PrivateFontCollection pfc = new PrivateFontCollection();
                        pfc.AddFontFile(fontFile);
                        FontFamily family = new FontFamily("IDAutomationHC39M", pfc);

                        Font oFont = new Font(family, 16);
                        PointF point = new PointF(2f, 2f);
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                    }

                    bitMap.Save(memoryStream, ImageFormat.Jpeg);

                    return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                }
            }




            //MemoryStream memoryStream = new MemoryStream();

            //int w = barcode.Length * 55;

            //// Create a bitmap object of the width that we calculated and height of 100
            //Bitmap oBitmap = new Bitmap(w, 100);
            //// then create a Graphic object for the bitmap we just created.
            //Graphics oGraphics = Graphics.FromImage(oBitmap);
            //// Now create a Font object for the Barcode Font
            //// (in this case the IDAutomationHC39M) of 18 point size

            //string fontFile = HostingEnvironment.MapPath("~/fonts/IDAutomationHC39M.TTF");

            //PrivateFontCollection pfc = new PrivateFontCollection();
            //pfc.AddFontFile(fontFile);
            //FontFamily family = new FontFamily("IDAutomationHC39M", pfc);

            //Font oFont = new Font(family, 18);
            //// Let's create the Point and Brushes for the barcode
            //PointF oPoint = new PointF(2f, 2f);
            //SolidBrush oBrushWrite = new SolidBrush(Color.Black);
            //SolidBrush oBrush = new SolidBrush(Color.White);
            //// Now lets create the actual barcode image
            //// with a rectangle filled with white color
            //oGraphics.FillRectangle(oBrush, 0, 0, w, 100);
            //// We have to put prefix and sufix of an asterisk (*),
            //// in order to be a valid barcode
            //oGraphics.DrawString("*" + barcode + "*", oFont, oBrushWrite, oPoint);
            //// Then we send the Graphics with the actual barcode
            //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();


            //oBitmap.Save(memoryStream, ImageFormat.Jpeg);

            //return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());

        }
    }
}
