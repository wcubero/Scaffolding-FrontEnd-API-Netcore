using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Empresa.Proyecto.Models
{
    public static class Extensions
    {
        static int _Retry = 5;

        public static string Encode64AndEncrypt(this string value)
        {
            return Base64Encode(Encryption.ToEncrypt(value));
        }

        public static string Decode64AndDecrypt(this string value)
        {
            string res = Base64Decode(value);

            return Encryption.ToDecrypt(res);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static IEnumerable<T> Execute_Query<T>(string query, string connectionString = "")
        {
            IEnumerable<T> items = null;

            int Contador = 0;

            while (Contador <= _Retry)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        items = connection.Query<T>(query);
                    }

                    Contador = _Retry + 1;
                }
                catch (Exception ex)
                {
                    if (Contador == _Retry)
                    {
                        throw ex;
                    }

                    Contador++;
                }
            }

            return items;
        }

        public static IEnumerable<T> Execute_Query<T>(string spName, Dictionary<string, object> dParameters, string connectionString = "")
        {
            var dbArgs = new DynamicParameters();

            int Contador = 0;

            while (Contador <= _Retry)
            {
                try
                {
                    foreach (var pair in dParameters)
                    {
                        dbArgs.Add(pair.Key, pair.Value);
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        return (connection.Query<T>(spName, new DynamicParameters(dbArgs), commandType: CommandType.StoredProcedure));
                    }
                }
                catch (Exception ex)
                {
                    if (Contador == _Retry)
                    {
                        throw ex;
                    }

                    Contador++;
                }
            }

            return null;
        }

        private static string ApplicationExeDirectory()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            return appRoot;
        }

        public static string ClearPrm(this string value)
        {
            try
            {
                if (String.IsNullOrEmpty(value))
                {
                    return String.Empty;
                }

                return value.Trim();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public static bool SendEmail(string subject, string body, string emailTo, List<Attachment> attachment, AppSettings appSettings)
        {
            int Contador = 0;

            while (Contador <= _Retry)
            {
                try
                {
                    var fromAddress = new MailAddress(appSettings.EmailSenderAccount);
                    var toAddress = new MailAddress(emailTo.Trim());
                    string fromPassword = appSettings.EmailSenderPassword.Trim();

                    var smtp = new SmtpClient
                    {

                        Host = appSettings.EmailSenderHost.Trim(),
                        Port = 25,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };

                    var message = new MailMessage(fromAddress, toAddress);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    foreach (var item in attachment)
                    {
                        message.Attachments.Add(item);
                    }

                    smtp.Send(message);

                    return true;
                }
                catch (Exception ex)
                {
                    if (Contador == _Retry)
                    {
                        throw ex;
                    }

                    Contador++;
                }
            }

            return false;
        }



        //public static void SendEmail_R10PST147(Documento model, string urlFile, string nameFile, string correo)
        //{
        //    string cedula = string.Empty, campana = string.Empty;
        //    cedula = Extensions.Decode64AndDecrypt(model.Cedula);
        //    campana = Extensions.Decode64AndDecrypt(model.Campana);
        //    //cedula = "107140124";
        //    //campana = "3969";

        //    var campa = DocumentoBL.TraeCampana(new Documento() { Cedula = cedula, Campana = campana });
        //    foreach (var item in campa)
        //    {
        //        string Cuenta = item.NombreCuenta.ToString();
        //        string NombreClie = item.NombreCliente.ToString();
        //        string NombreCampana = item.NombreCampana.ToString();
        //        List<Attachment> ListAttachment = new List<Attachment>();

        //        using (var client = new WebClient())
        //        {
        //            var content = client.DownloadData(urlFile);

        //            Attachment att = new Attachment(new MemoryStream(content), cedula + "-" + Cuenta + ".pdf");
        //            //Attachment att = new Attachment(new MemoryStream(content), $"{model.Cedula}-{nameFile}");               

        //            ListAttachment.Add(att);
        //        }

        //        //foreach (var currrentEmail in System.Configuration.ConfigurationManager.AppSettings["EmailCopiaPdf"].ToString().Split(';'))
        //        foreach (var currrentEmail in correo.ToString().Split(';'))
        //        {
        //            //string subject = $"({model.Cedula}) BANCO POPULAR Y DESARROLLO COMUNAL/BPO Documento Formalización R-10-P-ST-147 Versión 2.0";
        //            string subject = $"{cedula}-{Cuenta}/ Documento Formalización R-10-P-ST-147 Versión 2.0";

        //            string body = @"                   
        //            <b>[cliente]</b>                      <br><br>         
        //            Se adjunta documento del cliente:                   <br>
        //            Nombre: <b>[nombre]</b>                              <br>
        //            Cédula: <b>[cedula]</b>                              <br>
        //            Campaña: <b>[campana]</b>                              <br>
        //            Para continuación del trámite de formalización.     <br><br>                    
        //            ";
        //            body = body.Replace("[cedula]", cedula.Trim().ToUpper());
        //            body = body.Replace("[campana]", NombreCampana.Trim().ToUpper());
        //            body = body.Replace("[nombre]", NombreClie.Trim().ToUpper());
        //            body = body.Replace("[cliente]", Cuenta.Trim().ToUpper());

        //            SendEmail(subject, body, currrentEmail, ListAttachment);
        //        }
        //    }

        //}

        //public static void SendEmail_R10PST147Adj(Documento model, string urlFile, string nameFile, string correo, string adj)
        //{

        //    string cedula = string.Empty, campana = string.Empty;
        //    cedula = Extensions.Decode64AndDecrypt(model.Cedula);
        //    campana = Extensions.Decode64AndDecrypt(model.Campana);
        //    //cedula = "107140124";
        //    //campana = "3969";

        //    var campa = DocumentoBL.TraeCampana(new Documento() { Cedula = cedula, Campana = campana });
        //    foreach (var item in campa)
        //    {
        //        string Cuenta = item.NombreCuenta.ToString();
        //        string NombreClie = item.NombreCliente.ToString();
        //        string NombreCampana = item.NombreCampana.ToString();
        //        List<Attachment> ListAttachment = new List<Attachment>();

        //        using (var client = new WebClient())
        //        {
        //            var content = client.DownloadData(urlFile);

        //            Attachment att = new Attachment(new MemoryStream(content), cedula + "-" + Cuenta + ".pdf");

        //            if (adj != "0")
        //            {
        //                var Adjunto = DocumentoBL.TraeAdjunto(new Documento() { Cedula = cedula, Campana = campana }, adj);
        //                foreach (var resul in Adjunto)
        //                {
        //                    string DESCRIPCION = resul.Descripcion.ToString();
        //                    string URL = resul.Url.ToString();
        //                    string PROPIETARIO = resul.Cedula.ToString();
        //                    using (var cliente = new WebClient())
        //                    {
        //                        var Adjunt = client.DownloadData(URL);
        //                        Attachment att2 = new Attachment(new MemoryStream(Adjunt), PROPIETARIO + "-" + DESCRIPCION);

        //                        ListAttachment.Add(att2);
        //                    }
        //                }
        //            }

        //            //Attachment att = new Attachment(new MemoryStream(content), $"{model.Cedula}-{nameFile}");               

        //            ListAttachment.Add(att);
        //        }

        //        //foreach (var currrentEmail in System.Configuration.ConfigurationManager.AppSettings["EmailCopiaPdf"].ToString().Split(';'))
        //        foreach (var currrentEmail in correo.ToString().Split(';'))
        //        {
        //            //string subject = $"({model.Cedula}) BANCO POPULAR Y DESARROLLO COMUNAL/BPO Documento Formalización R-10-P-ST-147 Versión 2.0";
        //            string subject = $"{cedula}-{Cuenta}/ Documento Formalización R-10-P-ST-147 Versión 2.0";

        //            string body = @"                   
        //            <b>[cliente]</b>                      <br><br>         
        //            Se adjunta documento del cliente:                   <br>
        //            Nombre: <b>[nombre]</b>                              <br>
        //            Cédula: <b>[cedula]</b>                              <br>
        //            Campaña: <b>[campana]</b>                              <br>
        //            Para continuación del trámite de formalización.     <br><br>                    
        //            ";
        //            body = body.Replace("[cedula]", cedula.Trim().ToUpper());
        //            body = body.Replace("[campana]", NombreCampana.Trim().ToUpper());
        //            body = body.Replace("[nombre]", NombreClie.Trim().ToUpper());
        //            body = body.Replace("[cliente]", Cuenta.Trim().ToUpper());

        //            SendEmail(subject, body, currrentEmail, ListAttachment);
        //        }
        //    }

        //}

        public static string RestSharpRequest(string url, string parameter, Method verb = Method.POST)
        {
            int _Retry = 5;

            int Contador = 0;

            while (Contador <= _Retry)
            {
                try
                {
                    var client = new RestClient(url);

                    client.Timeout = -1;

                    var request = new RestRequest(verb);

                    request.AddHeader("Authorization", "Basic ZmVycmV0b29sRWNvbW1lcmNlQXBpOk14NGJBSVJBQVFqemVRMg==");

                    request.AddParameter("application/json", parameter, ParameterType.RequestBody);

                    IRestResponse response = client.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return response.Content;
                    }

                    throw new Exception("Error el consultar API");
                }
                catch (Exception ex)
                {
                    if (Contador == _Retry)
                    {
                        throw ex;
                    }

                    Contador++;
                }
            }

            return string.Empty;

        }


        //public static string ApiRequest(Blob model)
        //{
        //    var body = JsonConvert.SerializeObject(model);

        //    string response = BaseApi("https://api-innovacion.excelteccr.com/api/blob/upload", body);

        //    return response;
        //}


    }
}
