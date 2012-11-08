using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Input;

namespace Microsoft.SPOT.Presentation.Controls
{
    public class SimpleButton : ContentControl
    {
        private bool pushed = false;

        public Color NormalBackgroundColor { get; set; }
        public Color OnBackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public event EventHandler Click;
        public string Text;

        public SimpleButton(Text child, int width, int height)
        {
           Text = child.TextContent;
           Child=child;
           Child.HorizontalAlignment = HorizontalAlignment.Center;
           Child.VerticalAlignment = VerticalAlignment.Center;
           this.Width = width;
           this.Height = height;
           NormalBackgroundColor = Colors.Cyan;
           OnBackgroundColor = Colors.Blue;
           BorderColor = Colors.DarkGray;
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.OnTouchDown(e);
            pushed = true;

            Invalidate();
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            base.OnTouchUp(e);
            pushed = false;
            if (Click != null)
            {
                Click(this, new EventArgs());
            }
            Invalidate();
        }


        public override void OnRender(Microsoft.SPOT.Presentation.Media.DrawingContext dc)
        {
            if (pushed)
            {
                dc.DrawRectangle(new SolidColorBrush(OnBackgroundColor), new Pen(BorderColor), 0, 0, ActualWidth, ActualHeight);
            }
            else
            {
                dc.DrawRectangle(new SolidColorBrush(NormalBackgroundColor), new Pen(BorderColor), 0, 0, ActualWidth, ActualHeight);
            }
        }
    }
}
