using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;
using Microsoft.SPOT.Input;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ButtonNETMF.Windows
{
    class Stats : Window
    {
        Window spWindow;
        Data HwDevices;
        Text humtext, temptext, minText, maxText;
        Canvas canvas, graph;
        Ellipse circle;
        bool tempMode = true;
        DayData[] weatherData;

        public Stats(Window mw, Data h)
        {
            spWindow = new Window();
            spWindow.Height = SystemMetrics.ScreenHeight;
            spWindow.Width = SystemMetrics.ScreenWidth;

            HwDevices = h;

            canvas = new Canvas();

            weatherData = new DayData [30];

            weatherData=get_Data("http://picktheoutfit.com/api/measurements/last/30", weatherData);

            Text title = new Text();
            title.Font = Resources.GetFont(Resources.FontResources.NinaB);
            title.ForeColor = Colors.DarkGray;
            title.TextContent = "Statistics (Last 30 days)";
            title.SetMargin(5);

            Text backtext = new Text();
            backtext.Font = Resources.GetFont(Resources.FontResources.NinaB);
            backtext.ForeColor = Colors.DarkGray;
            backtext.TextContent = "Back";
            backtext.SetMargin(5);
            SimpleButton back = new SimpleButton(backtext, 60, 25);
            back.Click += new EventHandler(back_Click);

            temptext = new Text();
            temptext.Font = Resources.GetFont(Resources.FontResources.small);
            temptext.ForeColor = Colors.DarkGray;
            temptext.TextContent = "Temperature";
            temptext.TouchDown += new TouchEventHandler(TempTextClick);
            temptext.SetMargin(5);

            humtext = new Text();
            humtext.Font = Resources.GetFont(Resources.FontResources.small);
            humtext.ForeColor = Colors.DarkGray;
            humtext.TextContent = "Humidity";
            humtext.TouchDown += new TouchEventHandler(HumTextClick);
            humtext.SetMargin(5);
           

            canvas.Children.Add(title);

            Canvas.SetTop(back, 175);
            Canvas.SetLeft(back, 220);

            Canvas.SetTop(temptext, 175);

            Canvas.SetTop(humtext, 175);
            Canvas.SetLeft(humtext, 125);

            canvas.Children.Add(temptext);
            canvas.Children.Add(humtext);
            canvas.Children.Add(back);

            graph = new Canvas();

            graph.Width = 275;
            graph.Height = 150;
            Canvas.SetTop(graph, 25);
            canvas.Children.Add(graph);

            DrawSlider(canvas);

            DrawGraph(graph);

            canvas.SetMargin(20, 20, 20, 20);
            spWindow.Child = canvas;
            spWindow.Visibility = Visibility.Visible;
        }

        private void DrawSlider(Canvas canvas)
        {
            Line horizontal = new Line(50, 0);
            circle = new Ellipse(7, 7);
            horizontal.Stroke = new Pen(Colors.Black);
            circle.Stroke = new Pen(Colors.DarkGray);
            circle.Fill = new SolidColorBrush(Colors.Blue);
            circle.TouchDown+=new TouchEventHandler(slider_TouchDown);
            Canvas.SetTop(circle, 180);
            Canvas.SetLeft(circle, 80);
            Canvas.SetTop(horizontal, 187);
            Canvas.SetLeft(horizontal, 75);
            canvas.Children.Add(horizontal);
            canvas.Children.Add(circle);
        }

        private void DrawGraph(Canvas canvas)
        {
            canvas.Children.Clear();
            if (tempMode) //Temperature mode
            {
                //Determine the maximum and minimum registered values
                int min=666, max=-666;
                if(weatherData[0]!=null)
                    for (int i = 0; i < weatherData.Length; i++)
                    {
                        if (weatherData[i].temperature != -666 && weatherData[i].temperature < min)
                            min = (int)weatherData[i].temperature;
                        if (weatherData[i].temperature != -666 && weatherData[i].temperature >max)
                            max = (int)weatherData[i].temperature;
                    }

                if (HwDevices.fahrenheit)
                {
                    min = (int)(min * 1.8 + 32);
                    max = (int)(max * 1.8 + 32);
                }

                minText = new Text("" + min);
                minText.Font=Resources.GetFont(Resources.FontResources.small);
                maxText = new Text("" + max);
                maxText.Font=Resources.GetFont(Resources.FontResources.small);

                Line horizontal = new Line(0, canvas.Height-10);
                horizontal.Stroke = new Pen(Colors.Black);

                Canvas.SetTop(maxText, 5);
                Canvas.SetTop(minText, 130);
                Canvas.SetTop(horizontal, 5);
                Canvas.SetLeft(horizontal, 20);

                int px = 20;
                int py = 135;
                int x = 25;
                if (weatherData[0] != null)
                    for (int i = 0; i < 30; i++)
                    {
                        if (weatherData[i].temperature != -666)
                        {
                            int t;
                            if (HwDevices.fahrenheit)
                                t = (int)(weatherData[i].temperature * 1.8 + 32);
                            else
                                t = (int)(weatherData[i].temperature);
                            int y = (int)(t * 135);
                            y = 135-(y / max-min ) +5;
                            Ellipse auxCircle = new Ellipse(3, 3);
                            auxCircle.Stroke = new Pen(Colors.Orange);
                            auxCircle.Fill = new SolidColorBrush(Colors.Red);
                            Canvas.SetTop(auxCircle, y);
                            Canvas.SetLeft(auxCircle, x);
                            graph.Children.Add(auxCircle); 
                            px = x;
                            py = y;
                        }
                        x += 6;
                    }

                canvas.Children.Add(minText);
                canvas.Children.Add(maxText);
                canvas.Children.Add(horizontal);
            }
            else //Humidity mode
            {
                //Determine the maximum and minimum registered values
                int min = 666, max = -666;
                for (int i = 0; i < weatherData.Length; i++)
                {
                    if (weatherData[i].humidity != -666 && weatherData[i].humidity < min)
                        min = (int)weatherData[i].humidity;
                    if (weatherData[i].humidity != -666 && weatherData[i].humidity > max)
                        max = (int)weatherData[i].humidity;
                }

                minText = new Text("" + min);
                minText.Font = Resources.GetFont(Resources.FontResources.small);
                maxText = new Text("" + max);
                maxText.Font = Resources.GetFont(Resources.FontResources.small);

                Line horizontal = new Line(0, canvas.Height - 10);
                horizontal.Stroke = new Pen(Colors.Black);

                Canvas.SetTop(maxText, 5);
                Canvas.SetTop(minText, 130);
                Canvas.SetTop(horizontal, 5);
                Canvas.SetLeft(horizontal, 20);

                int px = 20;
                int py = 135;
                int x = 25;
                for (int i = 0; i < 30; i++)
                {
                    if (weatherData[i].humidity != -666)
                    {
                        int y = (int)(weatherData[i].humidity * 135);
                        y = 135 - (y / max) + 5;
                        Ellipse auxCircle = new Ellipse(3, 3);
                        auxCircle.Stroke = new Pen(Colors.Blue);
                        auxCircle.Fill = new SolidColorBrush(Colors.Cyan);
                        Canvas.SetTop(auxCircle, y);
                        Canvas.SetLeft(auxCircle, x);
                        graph.Children.Add(auxCircle);

                        px = x;
                        py = y;
                    }
                    x += 6;
                }

                canvas.Children.Add(minText);
                canvas.Children.Add(maxText);
                canvas.Children.Add(horizontal);

            }
        }

        DayData [] get_Data(string Url, DayData [] data)
        {
            Get get=new Get(Url);
            data = get.AnalyseTemperatureResults();
            return data;
        }

        void back_Click(object sender, EventArgs e)
        {
            weatherData = null;
            spWindow = new MainMenu(spWindow, HwDevices);
        }

        void slider_TouchDown(object sender, EventArgs e)
        {

        }

        void HumTextClick(object sender, EventArgs e)
        {
            if (tempMode)
            {
                Canvas.SetLeft(circle, 100);
                tempMode = !tempMode;
                DrawGraph(graph);
            }
        }

        void TempTextClick(object sender, EventArgs e)
        {
            if (!tempMode)
            {
                Canvas.SetLeft(circle, 80);
                tempMode = !tempMode;
                DrawGraph(graph);
            }
        }

    }
}
