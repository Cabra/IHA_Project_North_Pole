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
    class Keyboard : Window 
    {
        Window spWindow;
        Data HwDevices;
        Text Value;

        public Keyboard(Window mw, Data h)
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

            Text OkText = new Text("OK");
            OkText.Font = Resources.GetFont(Resources.FontResources.NinaB);
            OkText.ForeColor = Colors.DarkGray;
            SimpleButton OkButton = new SimpleButton(OkText, 100, 50);
            OkButton.NormalBackgroundColor = Colors.Green;
            OkButton.Click += new EventHandler(Ok_Click);

            Text CancelText = new Text("Cancel");
            CancelText.Font = Resources.GetFont(Resources.FontResources.NinaB);
            CancelText.ForeColor = Colors.DarkGray;
            SimpleButton CancelButton = new SimpleButton(CancelText, 100, 50);
            CancelButton.NormalBackgroundColor = Colors.Red;
            CancelButton.Click += new EventHandler(Cancel_Click);

            SimpleButton[] digits = new SimpleButton[10];
            for (int i = 0; i < digits.Length; i++)
            {
                Text t = new Text(i.ToString());
                t.Font = Resources.GetFont(Resources.FontResources.NinaB);
                t.ForeColor = Colors.DarkGray;
                digits[i] = new SimpleButton(t, 50, 50);
                digits[i].Click += new EventHandler(Value_Click);
            }

            Rectangle TextBox = new Rectangle(130, 50);
            TextBox.Stroke = new Pen(Colors.Gray);

            Value = new Text("");
            Value.Font = Resources.GetFont(Resources.FontResources.NinaB);
            Value.ForeColor = Colors.DarkGray;

            Canvas.SetLeft(OkButton, 160);
            Canvas.SetTop(CancelButton, 50);
            Canvas.SetLeft(CancelButton, 160);

            Canvas.SetTop(TextBox, 25);
            Canvas.SetLeft(TextBox, 10);
            Canvas.SetTop(Value, 45);
            Canvas.SetLeft(Value, 20);

            Canvas.SetTop(digits[1], 100);
            Canvas.SetLeft(digits[1], 10);
            Canvas.SetTop(digits[2], 100);
            Canvas.SetLeft(digits[2], 60);
            Canvas.SetTop(digits[3], 100);
            Canvas.SetLeft(digits[3], 110);
            Canvas.SetTop(digits[4], 100);
            Canvas.SetLeft(digits[4], 160);
            Canvas.SetTop(digits[5], 100);
            Canvas.SetLeft(digits[5], 210);
            Canvas.SetTop(digits[6], 150);
            Canvas.SetLeft(digits[6], 10);
            Canvas.SetTop(digits[7], 150);
            Canvas.SetLeft(digits[7], 60);
            Canvas.SetTop(digits[8], 150);
            Canvas.SetLeft(digits[8], 110);
            Canvas.SetTop(digits[9], 150);
            Canvas.SetLeft(digits[9], 160);
            Canvas.SetTop(digits[0], 150);
            Canvas.SetLeft(digits[0], 210);

            canvas.Children.Add(TextBox);
            canvas.Children.Add(Value);
            canvas.Children.Add(OkButton);
            canvas.Children.Add(CancelButton);
            foreach (SimpleButton s in digits)
                canvas.Children.Add(s);

            canvas.SetMargin(20, 20, 20, 20);
            spWindow.Child = canvas;
            spWindow.Visibility = Visibility.Visible;
        }

        void Value_Click(object sender, EventArgs e)
        {
            SimpleButton m=sender as SimpleButton;
            Value.TextContent += m.Text;
        }

        void Ok_Click(object sender, EventArgs e)
        {
            HwDevices.WOEID = Value.TextContent;
            spWindow = new Settings(spWindow, HwDevices);
        }

        void Cancel_Click(object sender, EventArgs e)
        {
            spWindow = new Settings(spWindow, HwDevices);
        }
    }
}
