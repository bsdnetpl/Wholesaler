using Azure;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using Wholesaler.DB;


namespace Wholesaler.Models
{
    public class CSV
    {


        public bool DownloadFile(string url)
        {
            try
            {
                string fileName = System.IO.Path.GetFileName(url);
                using (WebClient myWebClient = new WebClient())
                {
                    myWebClient.DownloadFile(url, fileName);
                }
            }
            catch (Exception ex)
            {
                // If there is an exception, return false
                return false;
            }
            // If the download is successful, return true
            return true;


        }
    }
}
