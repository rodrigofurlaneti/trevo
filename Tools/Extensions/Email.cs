using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace Core.Extensions
{
    public static class Mail
    {
        public static void SendMail(string to, string subject, string body, string from, Stream file, string filename, List<string> bcc)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage();
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment(file, filename));
            message.From = new MailAddress(from.Trim());
            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();

            foreach (string bc in bcc)
            {
                message.Bcc.Add(bc);
            }

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();
            
            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (int recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    SmtpStatusCode statusCode;

                    statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    if ((statusCode == SmtpStatusCode.MailboxBusy) ||
                        (statusCode == SmtpStatusCode.MailboxUnavailable))
                    {
                        Thread.Sleep(5000);
                        smtpClient.Send(message);
                    }
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                throw smtpExc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail(string to, string subject, string body, string from, Stream file, string filename)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage();
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment(file, filename));
            message.From = new MailAddress(from.Trim());
            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();


            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (int recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    SmtpStatusCode statusCode;

                    statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    if ((statusCode == SmtpStatusCode.MailboxBusy) ||
                        (statusCode == SmtpStatusCode.MailboxUnavailable))
                    {
                        Thread.Sleep(5000);
                        smtpClient.Send(message);
                    }
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                throw smtpExc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail(string to, List<string> cc, string subject, string body, string from, Stream file, string filename)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage();
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment(file, filename));
            message.From = new MailAddress(from.Trim());
            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();
            foreach (string item in cc)
            {
                message.CC.Add(new MailAddress(item.Trim()));
            }

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();


            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (int recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    SmtpStatusCode statusCode;

                    statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    if ((statusCode == SmtpStatusCode.MailboxBusy) ||
                        (statusCode == SmtpStatusCode.MailboxUnavailable))
                    {
                        Thread.Sleep(5000);
                        smtpClient.Send(message);
                    }
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                throw smtpExc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail(string to, string subject, string body, string from)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage();
            message.From = new MailAddress(from.Trim());
            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();
            message.IsBodyHtml = true;

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();
            
            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (int recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    SmtpStatusCode statusCode;

                    statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    if ((statusCode == SmtpStatusCode.MailboxBusy) ||
                        (statusCode == SmtpStatusCode.MailboxUnavailable))
                    {
                        Thread.Sleep(5000);
                        smtpClient.Send(message);
                    }
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                throw smtpExc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail(string to, string subject, string body, string from, List<string> bcc)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage();
            message.From = new MailAddress(from.Trim());
            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();
            message.IsBodyHtml = true;

            foreach (string bc in bcc)
            {
                message.Bcc.Add(bc);
            }
            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();
            
            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                throw smtpExc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail(string to, List<string> cc, string subject, string body, string from, List<string> cco = null)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage();
            message.From = new MailAddress(from.Trim());

            foreach (string item in cc)
            {
                message.CC.Add(new MailAddress(item.Trim()));
            }

            if (cco != null)
            {
                foreach (string email in cco)
                {
                    message.Bcc.Add(email);
                }
            }

            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();
            message.IsBodyHtml = true;

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();


            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (int recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    SmtpStatusCode statusCode;

                    statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    if ((statusCode == SmtpStatusCode.MailboxBusy) ||
                        (statusCode == SmtpStatusCode.MailboxUnavailable))
                    {
                        Thread.Sleep(5000);
                        smtpClient.Send(message);
                    }
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                throw smtpExc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail(string host, string user, string pass, string from, string to, List<string> cco, string titulo, string body, bool IsHtml)
        {
            var SMTPServer = new SmtpClient(host, 587);
            SMTPServer.Credentials = new NetworkCredential(user, pass);

            var MAILMessage = new MailMessage();

            MAILMessage.From = new MailAddress(from);
            MAILMessage.To.Add(to);
            foreach (string email in cco)
            {
                MAILMessage.Bcc.Add(email);
            }
            MAILMessage.Subject = titulo;
            MAILMessage.Body = body;
            MAILMessage.IsBodyHtml = IsHtml;
            SMTPServer.Send(MAILMessage);
        }

        public static void SendMail(string host, string user, string pass, string from, string to, string titulo, string body, bool IsHtml)
        {
            var SMTPServer = new SmtpClient(host, 587);
            SMTPServer.Credentials = new NetworkCredential(user, pass);

            var MAILMessage = new MailMessage();

            MAILMessage.From = new MailAddress(from);
            MAILMessage.To.Add(to);
            MAILMessage.Subject = titulo;
            MAILMessage.Body = body;
            MAILMessage.IsBodyHtml = IsHtml;
            SMTPServer.Send(MAILMessage);
        }
    }
}