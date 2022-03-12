using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    class APIAccessLayer
    {
        public APIAccessLayer()
        {
            // Empty Constructor
        }

        // GET request for JSON given API URL
        public String getJsonFromUrl(String url)
        {
            String result = "";

            // Create a HTTP request object with the URL
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            // Verify that the HTTP request only accpets JSON
            httpRequest.Accept = "application/json";

            // Create HTTP response object
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            // Wait for response stream and assign to result
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }


            Console.WriteLine(httpResponse.StatusCode);
            // Return JSON grabbed from the API
            return result;
        }


    }
}
