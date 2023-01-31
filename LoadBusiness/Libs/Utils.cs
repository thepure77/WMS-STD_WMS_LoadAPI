using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PlanGIBusiness.Libs
{
    public class Utils
    {
        public string saveReport(byte[] file, string name, string rootPath)
        {
            var saveLocation = PhysicalPath(name, rootPath);
            FileStream fs = new FileStream(saveLocation, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                try
                {
                    bw.Write(file);
                }
                finally
                {
                    fs.Close();
                    bw.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return VirtualPath(name);
            //throw new NotImplementedException();
        }

        private string ReportPath
        {
            get
            {
                //var url = System.Configuration.ConfigurationManager.AppSettings["SERVICE-REPORT"].ToString();
                var url = "\\ReportGenerator\\";
                return url;
            }
        }

        public string VirtualPath(string name)
        {
            var filename = name;
            var vPath = ReportPath;
            vPath = vPath.Replace("~", "");
            return vPath + filename;
        }

        public string PhysicalPath(string name, string rootPath)
        {

            //rootPath = rootPath.Replace("\\TOP_ReportAPI", "\\TOP_WMS");
            //rootPath = rootPath.Replace("\\ReportAPI", "\\TOP_WMS");
            var filename = name;
            var vPath = ReportPath;
            //string path = @"C:\tmp\ReportGenerator\";

            //var path = HttpContext.Current.Server.MapPath(vPath);
            var path = rootPath + vPath;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            var saveLocation = System.IO.Path.Combine(path, filename);
            return saveLocation;
        }

        public static T SendDataApi<T>(string url, string data, string authorization = "")
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(authorization))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
                }

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                var contentResult = result.Content.ReadAsStringAsync().Result;
                T model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contentResult);
                return model;
            }
        }
    }
}
