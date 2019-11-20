using ImageUpload.Models;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ImageUpload.Controllers
{
    [System.Web.Http.Route("api/OCRrequest")]
    public class UploadController : ApiController
    {
        static string responsetext;
        private static ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string Subscriptionkey()
        {
            return System.Configuration.ConfigurationManager
               .AppSettings["Subscription-Key"];
        }
        static string RequestParameters()
        {
            return System.Configuration.ConfigurationManager
               .AppSettings["RequestParameters"];
        }

        static string ReadURI()
        {
            return System.Configuration.ConfigurationManager
               .AppSettings["APIuri"];
        }
        static string Contenttypes()
        {
            return System.Configuration.ConfigurationManager
               .AppSettings["Contenttypes"];
        }

        [System.Web.Http.HttpPost]
        public async Task<object> Imgmodel(string filePath)
        {
            Retailprice result = new Retailprice();
           
                var httpRequest = HttpContext.Current.Request;

                //if (httpRequest.Files.Count > 0)
                //{
                //    foreach (string file in httpRequest.Files)
                //    {
                //        var postedFile = httpRequest.Files[file];

                //        var fileName = postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();

                //        var filePath = HttpContext.Current.Server.MapPath("~/Files/" + fileName);


                //        postedFile.SaveAs(filePath);

                //        Log.Debug("File is saved at"+filePath);

               var ComputerVisionAPIclient = new HttpClient();

                        try
                        {
                //var fileName = Path.GetFileName(filePath);
                //var imgPath = HttpContext.Current.Server.MapPath("~/Files/" + fileName);
                //            File.SaveAs(imgPath);
                            ComputerVisionAPIclient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Subscriptionkey());
                            string requestParameters = RequestParameters();
                            string APIuri = ReadURI() + requestParameters;
                            HttpResponseMessage myresponse;

                            byte[] byteData = GetByteArray(filePath);
                            var content = new ByteArrayContent(byteData);
                            content.Headers.ContentType = new MediaTypeHeaderValue(Contenttypes());
                            myresponse = await ComputerVisionAPIclient.PostAsync(APIuri, content);
                            myresponse.EnsureSuccessStatusCode();
                            responsetext = await myresponse.Content.ReadAsStringAsync();
                            result = ExtractPrintedWords(responsetext);
                            
                        }
                        catch (Exception e)
                        {
                            Log.Error(e);
                            return new HttpResponseException(HttpStatusCode.NotFound);
                           

                        }

            return result;
        }
       
        static byte[] GetByteArray(string LocalimageFilePath)
        {
            FileStream ImagefileStream = new FileStream(LocalimageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader ImagebinaryReader = new BinaryReader(ImagefileStream);
            byte[] bytedata = ImagebinaryReader.ReadBytes((int)ImagefileStream.Length);
            ImagefileStream.Close();
            return bytedata;
        }
        static Retailprice ExtractPrintedWords(string jsonResponse)
        {
            //List<string> listDistinctWords = new List<string>();
            //Retailprice objrp = new Retailprice();
            //dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

            //foreach (var region in jsonObj.regions)
            //{
            //    foreach (var line in region.lines)
            //    {
            //        foreach (var word in line.words)
            //        {

            //            listDistinctWords.Add(Convert.ToString(word.text));
            //        }
            //    }
            //}

            //if (listDistinctWords.Contains("Yogurt"))
            //{
            //    string productname1 = listDistinctWords[0] + " " + listDistinctWords[1] + " " + listDistinctWords[2] + " " + listDistinctWords[3];
            //    string productname2 = listDistinctWords[10] + " " + listDistinctWords[11] + " " + listDistinctWords[12] + " " + listDistinctWords[13];
            //    string RetailPrice1 = listDistinctWords[9];
            //    string RetailPrice2 = listDistinctWords[19];
            //    objrp.Price = RetailPrice1;
            //    objrp.Productname = productname1;

            //}
            //else if (listDistinctWords.Contains("Beefsteak") && listDistinctWords.Contains("Tomatoes,"))
            //{
            //    string productname1 = listDistinctWords[0] + " " + listDistinctWords[1] + " " + listDistinctWords[2];
            //    productname1 = productname1.Replace(",", "");
            //    string price = listDistinctWords[7];
            //    string RetailPrice1 = listDistinctWords[6] + price;
            //    objrp.Price = RetailPrice1;
            //    objrp.Productname = productname1;


            //}

            //else if (listDistinctWords.Contains("Red") && listDistinctWords.Contains("Potatoes,"))
            //{
            //    string productname1 = listDistinctWords[0] + " " + listDistinctWords[1];

            //    string RetailPrice1 = listDistinctWords[6];
            //    productname1 = productname1.Replace(",", "");
            //    objrp.Price = RetailPrice1;
            //    objrp.Productname = productname1;

            //}

            //else if (listDistinctWords.Contains("Vine,") && listDistinctWords.Contains("Tomatoes"))
            //{
            //    string productname1 = listDistinctWords[0] + " " + listDistinctWords[1] + " " + listDistinctWords[2] + " " + listDistinctWords[3];

            //    string RetailPrice1 = listDistinctWords[9];
            //    productname1 = productname1.Replace(",", "");
            //    objrp.Price = RetailPrice1;
            //    objrp.Productname = productname1;

            //}

            //else if (listDistinctWords.Contains("Russet") && listDistinctWords.Contains("Potatoes,"))
            //{
            //    string productname1 = listDistinctWords[0] + " " + listDistinctWords[1] + " " + listDistinctWords[2];

            //    string RetailPrice1 = listDistinctWords[6];
            //    productname1 = productname1.Replace(",", "");
            //    objrp.Price = RetailPrice1;
            //    objrp.Productname = productname1;

            //}

            //else if (listDistinctWords.Contains("Roma") && listDistinctWords.Contains("Tomatoes,"))
            //{
            //    string productname1 = listDistinctWords[0] + " " + listDistinctWords[1];

            //    string RetailPrice1 = listDistinctWords[5];
            //    productname1 = productname1.Replace(",", "");
            //    objrp.Price = RetailPrice1;
            //    objrp.Productname = productname1;

            //}

            //else if (listDistinctWords.Contains("White") && listDistinctWords.Contains("Potatoes,"))
            //{
            //    string productname1 = listDistinctWords[0] + " " + listDistinctWords[1];

            //    string RetailPrice1 = listDistinctWords[5];
            //    productname1 = productname1.Replace(",", "");
            //    objrp.Price = RetailPrice1;
            //    objrp.Productname = productname1;

            //}


            //return objrp;
           
            string product = null;
            string price = null;

            Retailprice obj = new Retailprice();

            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
            dynamic jsonobj1 = new ExpandoObject();
            dynamic jsonobj2 = new ExpandoObject();

            foreach (var region in jsonObj.regions)
            {
                foreach (var line in region.lines)
                {
                    
                    jsonobj1 = region.lines[0];
                    jsonobj2 = region.lines.Count > 2? region.lines[2]:null;

                }
            }
            foreach (var words in jsonobj1.words)
            {
                string word = Convert.ToString(words.text);
                if (word.Contains("each") || word.Contains("Pound") || word.Contains("per") || word.Contains("1") || word.Contains("lb") || word.Contains("bag"))
                    continue;
                else
                    product += word + " ";

            }
            //var output = JsonConvert.DeserializeObject<dynamic>(jsonobj2);
            //var myCommandMessage = JsonConvert.DeserializeObject<dynamic>(jsonobj2);
           // if (!myCommandMessage.HasValues)
           // {
                // The object is empty
            //}
            if ((jsonobj2 is null))
            {
                price += "Price not found on image";
            }
            else
            {
                foreach (var words in jsonobj2.words)
                {

                    price += Convert.ToString(words.text);
                }

            }
            
            int length = product.Length;
            if (product.Contains(","))
            {
                product = product.Remove(length - 2);

            }


            obj.Price = price;
            obj.Productname = product;

            return obj;

        }
    }
}
