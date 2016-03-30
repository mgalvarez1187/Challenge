using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Newtonsoft;
using System.Runtime.Serialization;

namespace ChallengeMVC.Controllers
{
    public class HomeController : Controller
    {

        private const string vBaseURL = "http://localhost:56140/";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        //public ActionResult ResetDB(string result)
        //{
        //    return 
        //}
    


        [HttpGet]
        public ActionResult Upload(string result)
        {

            if(result!=null && result != "")
            {
                result = result.Replace("\"", "");                
            }
           

            ViewBag.result = result;
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file1)
        {

            string str = "";

            if (file1 != null && file1.ContentLength > 0)
            {
                    
                byte[] data = new byte[] { };
                using (var binaryReader = new BinaryReader(file1.InputStream))
                {
          
                    char ch;
                    try//try avoids the EOF
                    {
                        // Get file's data
                        while ((int)(ch = binaryReader.ReadChar()) != 0)
                        {
                            str = str + ch;
                        }
                    }
                    catch { }
                }                
            }

            //request
            if (str != "")
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(str), System.Text.Encoding.UTF8, "application/json");
                
                var response = client.PostAsync(vBaseURL+"championship/new?val="+str , content);
                response.Wait();

                var res = response.Result.Content.ReadAsStringAsync();
                res.Wait();

                return Upload(res.Result);

            }


            return View();
        }

        [HttpGet]
        public ActionResult DocsInfo()
        {
            return View();
        }

    }


}