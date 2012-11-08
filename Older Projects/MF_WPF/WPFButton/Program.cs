using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Presentation.Media;
using System;
using System.Threading;
using System.Net;
using ButtonNETMF.Windows;
using System.Net.Sockets;

namespace ButtonNETMF
{
    public class Program : Application
    {
        OutputPort Led;
        static Data HwDevices;
        Window CurrentWindow;
        static bool init = true;
        public static void Main()
        {
            HwDevices=new Data();
            //Utility.SetLocalTime(GetNetworkTime());
            Program myApplication = new Program();
            Thread readValues = new Thread(ValuesReader);
            readValues.Start();
            while (init) { };
            Touch.Initialize(myApplication);
            myApplication.Run(myApplication.CreateWindow());
            Thread.Sleep(Timeout.Infinite);
        }

        public Window CreateWindow()
        {
            Window window = new Window();
            window = new Window();
            window.Height = SystemMetrics.ScreenHeight;
            window.Width = SystemMetrics.ScreenWidth;
            window.Visibility = Visibility.Visible;
            CurrentWindow = new MainMenu(window, HwDevices);
            return window;
        }

        private static void ValuesReader()
        {
            double oldTemp = 0;
            double oldHum = 0;

            Get xml = new Get("http://weather.yahooapis.com/forecastrss?w="+HwDevices.WOEID+"&u=c.xml");
            HwDevices = xml.AnalyseXMLResults(HwDevices);

            while (true)
            {
                //reads the temperature
                double temp = HwDevices.ReadValue();

                //reads the humidity
                double hum = 0;

                if (oldHum - hum <= -5 || oldHum - hum >= 5 || oldTemp - temp <= -2 || oldTemp - temp >= 2 || oldTemp == oldHum)
                {
                    new Post(hum, temp, HwDevices);
                }

                //the current values are stored on the old values for the evaluation in the next 5 minutes
                oldHum = hum;
                oldTemp = temp;

                init = false;

                Thread.Sleep(HwDevices.getUpdateTime()*60*1000);
            }
        }

        public static DateTime GetNetworkTime()
        {
            Socket s;
            try
            {
                IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry("time-a.nist.gov").AddressList[0], 123);
                s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                s.Connect(ep);
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return new DateTime();
            }

            byte[] ntpData = new byte[48]; // RFC 2030
            ntpData[0] = 0x1B;
            for (int i = 1; i < 48; i++)
                ntpData[i] = 0;

            s.Send(ntpData);
            s.Receive(ntpData);

            byte offsetTransmitTime = 40;
            ulong intpart = 0;
            ulong fractpart = 0;
            for (int i = 0; i <= 3; i++)
                intpart = 256 * intpart + ntpData[offsetTransmitTime + i];

            for (int i = 4; i <= 7; i++)
                fractpart = 256 * fractpart + ntpData[offsetTransmitTime + i];

            ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);

            s.Close();

            TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);
            DateTime dateTime = new DateTime(1900, 1, 1);
            dateTime += timeSpan;

            TimeSpan offsetAmount = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
            DateTime networkDateTime = (dateTime + offsetAmount);

            Debug.Print(networkDateTime.ToString());

            return networkDateTime;
        }
    }
}
