using System.Configuration;
using System.IO;
using Entidade.Uteis;

namespace Portal.Helpers
{
    public static class PathArquivo
    {
        public static string RetornePathPorTipoArquivo(TipoArquivo tipoArquivo)
        {
            switch (tipoArquivo)
            {
                case TipoArquivo.Arquivo:
                    return ConfigurationManager.AppSettings["PATH_ARQUIVO_PADRAO"];
                case TipoArquivo.Thumbnail:
                    return ConfigurationManager.AppSettings["PATH_THUMBNAIL"];
                case TipoArquivo.Foto:
                    return ConfigurationManager.AppSettings["PATH_FOTO_LOCAL"];
                default:
                    return ConfigurationManager.AppSettings["PATH_ARQUIVO_PADRAO"];
            }
        }

        public static string ReturnPath(TipoArquivo tipoArquivo)
        {
            var path = RetornePathPorTipoArquivo(tipoArquivo);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static string ReturnPath(TipoArquivo tipoArquivo, string idUsuario)
        {
            var path = Path.Combine(RetornePathPorTipoArquivo(tipoArquivo), idUsuario);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
    }
}