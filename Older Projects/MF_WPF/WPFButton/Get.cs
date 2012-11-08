using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Input;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace ButtonNETMF
{
    class Get
    {
        string result;
        byte[] byteData = new byte[1596]; //trimmed down for FEZ Panda II

        public Get(string Url)
        {
            using (var request = (HttpWebRequest)WebRequest.Create(Url))
            {
                request.KeepAlive = false;
                request.Method = "GET";
                Thread.Sleep(500);
                WebResponse response = null;
                response = request.GetResponse();
                Thread.Sleep(500);
                if (response != null)
                {
                    Stream respStream = response.GetResponseStream();
                    Thread.Sleep(500);
                    
                    char[] charData = new char[1596];
                    int bytesRead = 0;
                    Decoder UTF8decoder = System.Text.Encoding.UTF8.GetDecoder();
                    int totalBytes = 0;

                    respStream.ReadTimeout = 5000;

                    if (response.ContentLength != -1)
                    {
                        for (int dataRem = (int)response.ContentLength; dataRem > 0; )
                        {
                            Thread.Sleep(500);
                            bytesRead = respStream.Read(byteData, 0, byteData.Length);
                            if (bytesRead == 0)
                            {
                                Debug.Print("Error: Received " + (response.ContentLength - dataRem) + " Out of " + response.ContentLength);
                                break;
                            }
                            dataRem -= bytesRead;

                            // Convert from bytes to chars, and add to the page string.
                            int byteUsed, charUsed;
                            bool completed = false;
                            totalBytes += bytesRead;
                            UTF8decoder.Convert(byteData, 0, bytesRead, charData, 0, bytesRead, true, out byteUsed, out charUsed, out completed);
                            result = result + new String(charData, 0, charUsed);
                        }
                    }
                    else
                    {
                        // Read until the end of the data is reached.
                        while (true)
                        {
                            // If the Read method times out, it throws an exception, 
                            // which is expected for Keep-Alive streams because the 
                            // connection isn't terminated.
                            try
                            {
                                Thread.Sleep(500);
                                bytesRead =
                                    respStream.Read(byteData, 0, byteData.Length);
                            }
                            catch (Exception)
                            {
                                bytesRead = 0;
                            }

                            // Zero bytes indicates the connection has been closed by the server.
                            if (bytesRead == 0)
                                break;

                            int byteUsed, charUsed;
                            bool completed = false;
                            totalBytes += bytesRead;
                            UTF8decoder.Convert(byteData, 0, bytesRead, charData, 0, bytesRead, true, out byteUsed, out charUsed, out completed);
                            result = result + new String(charData, 0, charUsed);
                        }
                    }
                }
                response.Close();
            }
        }

        public Data AnalyseXMLResults(Data hw)
        {
            byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(result);
            MemoryStream strm = new MemoryStream(data);
            XmlReaderSettings ss = new XmlReaderSettings();
            ss.IgnoreWhitespace = false;
            ss.IgnoreComments=false;
            XmlReader xml = XmlReader.Create(strm, ss);
            while (!xml.EOF)
            {
                xml.Read();
                if (xml.Name == "yweather:location")
                {
                    while (xml.MoveToNextAttribute())
                    {
                        if (xml.Name == "city")
                        {
                            hw.location = xml.Value;
                        }
                        if (xml.Name == "country")
                        {
                            hw.location += ", " + xml.Value;
                            Debug.Print("Location:" + hw.location);
                        }
                    }
                }
                if (xml.Name == "yweather:astronomy")
                {
                    while (xml.MoveToNextAttribute())
                    {
                        if (xml.Name == "sunrise")
                        {
                            string[] aux = xml.Value.Split(':');
                            hw.sunrise.hour = int.Parse(aux[0]);
                            aux = aux[1].Split(' ');
                            hw.sunrise.minute = int.Parse(aux[0]);
                            Debug.Print("Sunrise:" + hw.sunrise.hour + ":" + hw.sunrise.minute);
                        }
                        if (xml.Name == "sunset")
                        {
                            string[] aux = xml.Value.Split(':');
                            hw.sunset.hour = int.Parse(aux[0]);
                            aux = aux[1].Split(' ');
                            hw.sunset.minute = int.Parse(aux[0]);
                            Debug.Print("Sunset:" + hw.sunset.hour + ":" + hw.sunset.minute);
                        }
                    }
                }
                if (xml.Name == "yweather:atmosphere")
                {
                    while (xml.MoveToNextAttribute())
                    {
                        if (xml.Name == "humidity")
                        {
                            Debug.Print("Humidity:" + xml.Value);
                            hw.humidity = int.Parse(xml.Value);
                        }
                    }
                }
                if (xml.Name == "yweather:condition")
                {
                    while (xml.MoveToNextAttribute())
                    {
                        if (xml.Name == "code")
                        {
                            hw.setWeatherType(int.Parse(xml.Value));
                            Debug.Print("Code:" + xml.Value);
                        }
                    }
                }
                //Gets the current date from the xml file
                if (xml.NodeType==XmlNodeType.Comment)
                {
                    string date = xml.Value;
                    string [] aux=date.Split(' ');
                    aux = aux[5].Split(':');
                    hw.current.hour = 2+int.Parse(aux[0]);
                    hw.current.minute = int.Parse(aux[1]);
                    Debug.Print("Time: " + hw.current.hour+":"+hw.current.minute);
                }
            }
            strm.Close();
            strm.Dispose();
            xml.Close();
            xml.Dispose();
            return hw;
        }

        public DayData[] AnalyseTemperatureResults()
        {
            DayData [] results = new DayData [31];
            if (result == null)
                return results;
            string [] values=result.Split(',');
            for (int i = 1; i < values.Length; i++)
            {
                string[] aux = values[i].Split('}');
                string[] aux2 = aux[0].Split('{');
                if (aux2[0] != "")
                {
                    string[] aux3 = aux2[0].Split(':');
                    values[i] = aux3[1];
                }
            }

            int num = 0;
            for (int i = 1; i < values.Length; i = i + 3)
            {
                results[num] = new DayData(values[i], values[i + 1]);
                num++; 
            }
            return results;
        }
    }
}
