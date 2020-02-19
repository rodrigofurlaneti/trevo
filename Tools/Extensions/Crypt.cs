using System;
using System.Configuration;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Core.Extensions
{
    public class Crypt
    {
        public Crypt(string NewPass)
        {
            //Usa saltos para evitar atack de dicionário
            var pdb = new PasswordDeriveBytes(NewPass, SALTBYTEARRAY);
            //--------------------------
            /*Encoder tipo DES*/
            //DES.Create();
            //--------------------------
            /*Encoder tipo RC2*/
            //RC2.Create();
            //--------------------------
            /*Encoder tipo Rijndael*/
            //Rijndael.Create();
            //--------------------------
            /*Encoder tipo Triple-DES*/
            //TripleDES.Create();
            //--------------------------
            var algo = TripleDES.Create();
            //--------------------------
            MKEY = pdb.GetBytes(algo.KeySize / 8);
            MIV = pdb.GetBytes(algo.BlockSize / 8);
        }

        public static object EnCrypt(string v, object p)
        {
            throw new NotImplementedException();
        }

        private byte[] MKEY = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private byte[] MIV = { 65, 110, 68, 26, 69, 178, 200, 219 };
        private byte[] SALTBYTEARRAY = { 0x49, 0x76, 0xee, 0x6e, 0x20, 0x4d, 0xff, 0x64, 0x76, 0x65, 0x64, 0x01, 0x76 };

        private byte[] EncryptDecrypt(byte[] inputBytes, bool encrpyt)
        {
            //--------------------------
            /*Encoder tipo DES*/
            //DES.Create();
            //--------------------------
            /*Encoder tipo RC2*/
            //RC2.Create();
            //--------------------------
            /*Encoder tipo Rijndael*/
            //Rijndael.Create();
            //--------------------------
            /*Encoder tipo Triple-DES*/
            //TripleDES.Create();
            //--------------------------
            var SA = TripleDES.Create();
            //--------------------------
            SA.Key = MKEY;
            SA.IV = MIV;
            //Transformação correta baseado no opção so usuario
            var transform = encrpyt ? SA.CreateEncryptor() : SA.CreateDecryptor();
            //Memory stream para saida
            var memStream = new MemoryStream();
            //Array de bytes para saida
            byte[] output;
            //Configura o encriptador e escreve no MemoryStream a saida
            var cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
            //Escreve as informações no mecanismo do encriptador
            cryptStream.Write(inputBytes, 0, inputBytes.Length);
            //Finaliza e escreve todas as informações necessárias na memoria
            cryptStream.FlushFinalBlock();
            //Resgata o array de bytes
            output = memStream.ToArray();
            //Finaliza o mecanismo de criptografia e fecha o canal de comunicação de memoria
            cryptStream.Close();
            return output;
        }

        public static string DeCrypt(string texto)
        {
            return DeCrypt(texto, ConfigurationManager.AppSettings["CryptKey"]);
        }
        
        public static string DeCrypt(string texto, string pass)
        {
            var crypt = new Crypt(pass);
            return crypt.Decrypt(texto);
        }

        public static string EnCrypt(string texto)
        {
            return EnCrypt(texto, ConfigurationManager.AppSettings["CryptKey"]);
        }
        
        public static string EnCrypt(string texto, string pass)
        {
            var crypt = new Crypt(pass);
            return crypt.Encrypt(texto);
        }
        
        public string Encrypt(string inputText)
        {
            //Declarando encoder
            var UTF8Encoder = new UTF8Encoding();
            //Converte em base64 e envia para a função de encriptar e desencriptar depois retorna e converte para string novamente
            string Result = Convert.ToBase64String(EncryptDecrypt(UTF8Encoder.GetBytes(inputText), true));
            //Retorno
            return Result;
        }

        public string Decrypt(string inputText)
        {
            //Declarando o encoder
            var utf8Encoder = new UTF8Encoding();
            try
            {
                //Converte em base64 e envia para a função de encriptar e desencriptar depois retorna e converte para string novamente
                return utf8Encoder.GetString(EncryptDecrypt(Convert.FromBase64String(inputText.Trim()), false));
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
