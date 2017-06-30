using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace EconFileUploadThroughRest
{
    class Program
    {
        private static int _journalId = 1;
        private static string _accountingYear = "2017";
        private static int _voucherNumber = 76;

        private static string _appSecretToken = "APP SECRET TOKEN";
        private static string _agreementGrantToken = "AGREEMENT GRANT TOKEN";
        static void Main(string[] args)
        {
            var filePath = "apps-e-conomic-regnskabsprogram_2.png";
            var fileUploadUrl = $"https://restapi.e-conomic.com/journals/{_journalId}/vouchers/{_accountingYear}-{_voucherNumber}/attachment/file";
            var file = new FileInfo(filePath).FullName;

            NameValueCollection files = new NameValueCollection();
            files.Add("", file);

            SendHttpRequest(fileUploadUrl, "PATCH", files);
        }

        

        private static string SendHttpRequest(string url,  string httpMethod, NameValueCollection files = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");

            byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = httpMethod;
            request.KeepAlive = true;
            request.Headers.Add("x-appsecrettoken", _appSecretToken);
            request.Headers.Add("x-agreementgranttoken", _agreementGrantToken);

            Stream requestStream = request.GetRequestStream();

           if (files != null)
            {
                foreach (string key in files.Keys)
                {
                    if (File.Exists(files[key]))
                    {
                        int bytesRead = 0;
                        byte[] buffer = new byte[2048];
                        byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", key, files[key]));
                        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        requestStream.Write(formItemBytes, 0, formItemBytes.Length);

                        using (FileStream fileStream = new FileStream(files[key], FileMode.Open, FileAccess.Read))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                // Write file content to stream, byte by byte
                                requestStream.Write(buffer, 0, bytesRead);
                            }

                            fileStream.Close();
                        }
                    }
                }
            }

            // Write trailer and close stream
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return reader.ReadToEnd();
            };
        }
    }
}
