using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Dominio
{
    public interface IImagemServico
    {
        byte[] GerarThumbnailEmBytes(byte[] imagemBytes, int novaLargura, int novaAltura);
    }

    public class ImagemServico : IImagemServico
    {
        public byte[] GerarThumbnailEmBytes(byte[] imagemBytes, int novaLargura, int novaAltura)
        {
            Bitmap startBitmap;
            using (var ms = new MemoryStream(imagemBytes))
            {
                startBitmap = new Bitmap(ms);
            }

            if(startBitmap.Width > novaAltura)
            {
                Bitmap newBitmap = new Bitmap(280, 150);
                using (Graphics graphics = Graphics.FromImage(newBitmap))
                {
                    graphics.DrawImage(startBitmap, new Rectangle(0, 0, 280, 150), new Rectangle(0, 0, startBitmap.Width, startBitmap.Height), GraphicsUnit.Pixel);
                }

                using (var ms = new MemoryStream())
                {
                    newBitmap.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            else
            {
                return imagemBytes;
            }
        }
    }
}