using System;
using Microsoft.SPOT;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ButtonNETMF
{
    class Post
    {
        public Post(double hum, double temp, Data hw)
        {
            HttpWebRequest request = (HttpWebRequest)
                 WebRequest.Create("http://picktheoutfit.com/api/measurements");
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";

            string json = "{\"humidity\":" + hw.humidity + "," +
              "\"temperature\":" + temp + ","+"\"weather_type\":" + hw.getWeatherType()+","+"\"location\": \"" + hw.location + "\"}";

            // turn our request string into a byte stream
            byte[] postBytes = Encoding.UTF8.GetBytes(json);

            // this is important - make sure you specify type this way
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.ContentLength = postBytes.Length;
            Stream requestStream = request.GetRequestStream();

            // now send it
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            // grab te response and print it out to the console along with the status code
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result;
            using (StreamReader rdr = new StreamReader(response.GetResponseStream()))
            {
                result = rdr.ReadToEnd();
                Debug.Print(result);
            }

            request.Dispose();
            response.Close();
            requestStream.Dispose();
        }
    }
}
