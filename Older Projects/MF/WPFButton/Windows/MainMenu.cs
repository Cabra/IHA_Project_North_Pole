using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;
using System;

namespace ButtonNETMF.Windows
{
    class MainMenu : Window
    {
        Window spWindow;
        static Data HwDevices;
        static Text TemperatureData;
        static Text HumidityData;
        static Dispatcher dispatcher;
        

        public MainMenu(Window mw, Data h)
        {
            spWindow = new Window();
            spWindow.Height = SystemMetrics.ScreenHeight;
            spWindow.Width = SystemMetrics.ScreenWidth;

            HwDevices = h;

            Get xml = new Get("http://weather.yahooapis.com/forecastrss?w=" + HwDevices.WOEID + "&u=c.xml");
            HwDevices = xml.AnalyseXMLResults(HwDevices);

            Bitmap aux = Resources.GetBitmap(Resources.BitmapResources.Title);
            Image logo = new Image(aux);
            logo.Width = 250;
            logo.Height = 75;

            TemperatureData = new Text();
            HumidityData = new Text();

            TemperatureData.Font = Resources.GetFont(Resources.FontResources.NinaB);
            TemperatureData.ForeColor = Colors.DarkGray;
            TemperatureData.TextContent = "Temperature: " + HwDevices.ReadValue();
            if (HwDevices.fahrenheit)
                TemperatureData.TextContent += "F";
            else
                TemperatureData.TextContent += "C";

            HumidityData.Font = Resources.GetFont(Resources.FontResources.NinaB);
            HumidityData.ForeColor = Colors.DarkGray;
            HumidityData.TextContent = "Humidity: " + HwDevices.humidity+"%";
            aux = null;

            switch (HwDevices.getWeatherType())
            {
                case 0:
                    aux = Resources.GetBitmap(Resources.BitmapResources.warning);
                    break;
                case 1:
                    aux = Resources.GetBitmap(Resources.BitmapResources.sun);
                    break;
                case 2:
                    aux = Resources.GetBitmap(Resources.BitmapResources.cloudy);
                    break;
                case 3:
                    aux = Resources.GetBitmap(Resources.BitmapResources.moon);
                    break;
                case 4:
                    aux = Resources.GetBitmap(Resources.BitmapResources.cloud);
                    break;
                case 5:
                    aux = Resources.GetBitmap(Resources.BitmapResources.lightning__1_);
                    break;
                case 6:
                    aux = Resources.GetBitmap(Resources.BitmapResources.rainy);
                    break;
                case 7:
                    aux = Resources.GetBitmap(Resources.BitmapResources.rainy__1_);
                    break;
                case 8:
                    aux = Resources.GetBitmap(Resources.BitmapResources.snowy);
                    break;
                case 9:
                    aux = Resources.GetBitmap(Resources.BitmapResources.cloud__1_);
                    break;
                case 10:
                    aux = Resources.GetBitmap(Resources.BitmapResources.snowy__1_);
                    break;
            }
            Image weather = new Image(aux);
            weather.Width = weather.Height = 50;

            Text LocationText = new Text("Location: "+HwDevices.location);
            LocationText.Font = Resources.GetFont(Resources.FontResources.NinaB);
            LocationText.ForeColor = Colors.DarkGray;

            Text text2 = new Text();
            text2.Font = Resources.GetFont(Resources.FontResources.NinaB);
            text2.ForeColor = Colors.DarkGray;
            text2.TextContent = "Stats";
            text2.SetMargin(5);
            SimpleButton sb2 = new SimpleButton(text2, 130, 30);
            sb2.Click += new EventHandler(sb2_Click);

            Text text3 = new Text();
            text3.Font = Resources.GetFont(Resources.FontResources.NinaB);
            text3.ForeColor = Colors.DarkGray;
            text3.TextContent = "Settings";
            text3.SetMargin(5);
            SimpleButton sb3 = new SimpleButton(text3, 130, 30);
            sb3.Click += new EventHandler(sb3_Click);

            ImageButton refresh = new ImageButton(new ImageBrush(Resources.GetBitmap(Resources.BitmapResources.refresh)),30,30);
            refresh.Click += new EventHandler(refresh_Click);

            Canvas canvas = new Canvas();

            Canvas.SetTop(LocationText, 70);
            canvas.Children.Add(LocationText);

            Canvas.SetTop(TemperatureData, 100);
            canvas.Children.Add(TemperatureData);

            Canvas.SetTop(HumidityData, 130);
            canvas.Children.Add(HumidityData);
            Canvas.SetTop(sb2, 170);
            canvas.Children.Add(sb2);
            Canvas.SetTop(sb3, 170);
            Canvas.SetLeft(sb3, 150);
            canvas.Children.Add(sb3);
            Canvas.SetTop(weather, 100);
            Canvas.SetLeft(weather, 150);
            Canvas.SetTop(refresh, 130);
            Canvas.SetLeft(refresh, 250);

            canvas.Children.Add(weather);
            canvas.Children.Add(logo);
            canvas.Children.Add(refresh);


            spWindow.Child = canvas;
            canvas.SetMargin(20, 20, 20, 20);
            spWindow.Visibility = Visibility.Visible;
            //dispatcher = spWindow.Dispatcher;

            //Thread t0 = new Thread(new ThreadStart(run));
            //t0.Start();
            
        }

        void sb2_Click(object sender, EventArgs e)
        {
            spWindow = new Stats(spWindow, HwDevices);
        }

        void refresh_Click(object sender, EventArgs e)
        {
            TemperatureData.TextContent="Temperature: "+HwDevices.ReadValue();
            if(HwDevices.fahrenheit)
                TemperatureData.TextContent+="F";
            else
                TemperatureData.TextContent+="C";
        }

        void sb3_Click(object sender, EventArgs e)
        {
            spWindow = new Settings(spWindow, HwDevices);
        }

        private static void run()
        {
            while (true)
            {
                Thread.Sleep(10000);
                dispatcher.BeginInvoke(new DispatcherOperationCallback(ValuesUpdater), null);
            }
        }

        private static object ValuesUpdater(object arg)
        {
            TemperatureData.TextContent = "Temperature: " + HwDevices.ReadValue();
            if (HwDevices.fahrenheit)
                TemperatureData.TextContent += "F";
            else
                TemperatureData.TextContent += "C";
            return null;
        }
    }
}
