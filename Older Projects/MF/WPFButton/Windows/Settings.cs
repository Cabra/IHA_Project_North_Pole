using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using System.Threading;

namespace ButtonNETMF.Windows
{
    class Settings : Window
    {
        Window spWindow;
        Data HwDevices;
        Text updatetext;
        SimpleButton FButton, CButton;

        public Settings(Window mw, Data h)
        {
            spWindow = new Window();
            spWindow.Height = SystemMetrics.ScreenHeight;
            spWindow.Width = SystemMetrics.ScreenWidth;

            HwDevices = h;

            DrawCanvas();

        }

        public void DrawCanvas()
        {

            Canvas canvas = new Canvas();

            Text title = new Text();
            title.Font = Resources.GetFont(Resources.FontResources.NinaB);
            title.ForeColor = Colors.DarkGray;
            title.TextContent = "Settings";
            title.SetMargin(5);

            Text backtext = new Text();
            backtext.Font = Resources.GetFont(Resources.FontResources.NinaB);
            backtext.ForeColor = Colors.DarkGray;
            backtext.TextContent = "Back";
            backtext.SetMargin(5);
            SimpleButton back = new SimpleButton(backtext, 60, 25);
            back.Click += new EventHandler(back_Click);

            Text updateTimeInfo = new Text("Update Time");
            updateTimeInfo.Font = Resources.GetFont(Resources.FontResources.NinaB);
            updateTimeInfo.ForeColor = Colors.DarkGray;

            Text plustext = new Text("+");
            plustext.Font = Resources.GetFont(Resources.FontResources.NinaB);
            plustext.ForeColor = Colors.DarkGray;
            SimpleButton plusButton = new SimpleButton(plustext, 60, 25);
            plusButton.Width = plusButton.Height;
            plusButton.Click += new EventHandler(plus_Click);

            Text minustext = new Text("-");
            minustext.Font = Resources.GetFont(Resources.FontResources.NinaB);
            minustext.ForeColor = Colors.DarkGray;
            SimpleButton minusButton = new SimpleButton(minustext, 60, 25);
            minusButton.Width = plusButton.Height;
            minusButton.Click += new EventHandler(minus_Click);

            Text metricInfo = new Text("Measurement");
            metricInfo.Font = Resources.GetFont(Resources.FontResources.NinaB);
            metricInfo.ForeColor = Colors.DarkGray;

            Text metricC = new Text("Celsius");
            metricC.Font = Resources.GetFont(Resources.FontResources.NinaB);
            metricC.ForeColor = Colors.DarkGray;
            CButton = new SimpleButton(metricC, 60, 25);
            if (!HwDevices.fahrenheit)
                CButton.NormalBackgroundColor = Colors.Blue;
            CButton.Click += new EventHandler(C_Click);

            Text metricF = new Text("Fahrenheit");
            metricF.Font = Resources.GetFont(Resources.FontResources.NinaB);
            metricF.ForeColor = Colors.DarkGray;
            FButton = new SimpleButton(metricF, 80, 25);
            if(HwDevices.fahrenheit)
                FButton.NormalBackgroundColor=Colors.Blue;
            FButton.Click += new EventHandler(F_Click);

            Text LocationIdInfo = new Text("Location ID:" + HwDevices.WOEID);
            LocationIdInfo.Font = Resources.GetFont(Resources.FontResources.NinaB);
            LocationIdInfo.ForeColor = Colors.DarkGray;

            Text ChangeLocationText = new Text("Change");
            ChangeLocationText.Font = Resources.GetFont(Resources.FontResources.NinaB);
            ChangeLocationText.ForeColor = Colors.DarkGray;
            SimpleButton ChangeLocationButton = new SimpleButton(ChangeLocationText, 60, 25);
            ChangeLocationButton.Click += new EventHandler(Location_Click);


            Rectangle valueRectangle = new Rectangle(2 * minusButton.Height, minusButton.Height);
            valueRectangle.Stroke = new Pen(Colors.Gray);

            updatetext = new Text(""+HwDevices.getUpdateTime());
            updatetext.Font = Resources.GetFont(Resources.FontResources.NinaB);
            updatetext.ForeColor = Colors.DarkGray;

            Canvas.SetTop(updatetext, 83);
            Canvas.SetLeft(updatetext, 150);
            Canvas.SetTop(valueRectangle, 80);
            Canvas.SetLeft(valueRectangle, 130);
            Canvas.SetTop(plusButton, 80);
            Canvas.SetLeft(plusButton, 187);
            Canvas.SetTop(minusButton, 80);
            Canvas.SetLeft(minusButton, 100);
            Canvas.SetTop(updateTimeInfo, 80);
            Canvas.SetTop(back, 175);
            Canvas.SetLeft(back, 220);
            Canvas.SetTop(CButton, 110);
            Canvas.SetLeft(CButton, 100);
            Canvas.SetTop(FButton, 110);
            Canvas.SetLeft(FButton, 170);
            Canvas.SetTop(metricInfo, 110);
            Canvas.SetTop(LocationIdInfo, 140);
            Canvas.SetTop(ChangeLocationButton, 140);
            Canvas.SetLeft(ChangeLocationButton, 170);

            canvas.Children.Add(title);
            canvas.Children.Add(back);
            canvas.Children.Add(updateTimeInfo);
            canvas.Children.Add(plusButton);
            canvas.Children.Add(minusButton);
            canvas.Children.Add(valueRectangle);
            canvas.Children.Add(updatetext);
            canvas.Children.Add(metricInfo);
            canvas.Children.Add(CButton);
            canvas.Children.Add(FButton);
            canvas.Children.Add(LocationIdInfo);
            canvas.Children.Add(ChangeLocationButton);

            canvas.SetMargin(20, 20, 20, 20);
            spWindow.Child = canvas;
            spWindow.Visibility = Visibility.Visible;
        }

        void back_Click(object sender, EventArgs e)
        {
            spWindow = new MainMenu(spWindow, HwDevices);
        }

        void Location_Click(object sender, EventArgs e)
        {
            spWindow = new Keyboard(spWindow, HwDevices);
        }

        void plus_Click(object sender, EventArgs e)
        {
            HwDevices.setUpdateTime(HwDevices.getUpdateTime() + 1);
            updatetext.TextContent = "" + HwDevices.getUpdateTime();
        }

        void minus_Click(object sender, EventArgs e) 
        {
            if (HwDevices.getUpdateTime() > 0)
            {
                HwDevices.setUpdateTime(HwDevices.getUpdateTime() - 1);
                updatetext.TextContent = "" + HwDevices.getUpdateTime();
            }
        }

        void C_Click(object sender, EventArgs e)
        {
            HwDevices.fahrenheit = false;
            DrawCanvas();
        }

        void F_Click(object sender, EventArgs e)
        {
            HwDevices.fahrenheit = true;
            DrawCanvas();
        }
    }
}
