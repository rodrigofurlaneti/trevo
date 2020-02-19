using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core.Exceptions;
using Core.Extensions;
using Entidade.Uteis;

namespace Portal.Helpers
{
    public static class Download
    {
        /// <summary>
        /// Rotina que encapsula o arquivo na página para fazer o download.
        /// </summary>
        /// <param name="dataSource">Lista com conteúdo do arquivo.</param>
        /// <param name="nome">Nome para o arquivo que será gerado.</param>
        /// <param name="contentType">Tipo de conteúdo que será gerado o arquivo.</param>
        public static void Criar(object dataSource, string nome, ContentType contentType, bool isString = false)
        {
            if (dataSource == null)
                throw new BusinessRuleException("Informe o conteúdo para download.");

            var gv = new GridView
            {
                DataSource = dataSource
            };

            gv.DataBind();

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", $"attachment; filename={nome}.{contentType.ToString()}");
            HttpContext.Current.Response.ContentType = contentType.ToDescription();
            HttpContext.Current.Response.Charset = "iso-8859-1";
            if (isString)
            {
                HttpContext.Current.Response.Output.Write(dataSource);
            }
            else
            {
                var objStringWriter = new StringWriter();
                var objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                HttpContext.Current.Response.Output.Write(objStringWriter.ToString());
            }
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static void DownloadArquivo(byte[] arquivo, string nome, ContentType contentType, bool isString = false)
        {

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", $"attachment; filename={nome}.{contentType.ToString()}");
            HttpContext.Current.Response.ContentType = contentType.ToDescription();
            HttpContext.Current.Response.Charset = "iso-8859-1";
            HttpContext.Current.Response.BinaryWrite(arquivo);

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}