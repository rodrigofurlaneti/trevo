using System.IO;

namespace Core.Extensions
{
    public class Converts
    {
        public static byte[] StreamToByte(Stream stream)
        {
            var buffer = new byte[0];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
