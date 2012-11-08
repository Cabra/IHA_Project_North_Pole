using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Input;

namespace Microsoft.SPOT.Presentation.Controls
{
    public class ImageButton : ContentControl
    {
        private bool pushed = false;

        public ImageBrush NormalBackgroundColor { get; set; }
        public ImageBrush OnBackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public event EventHandler Click;
        public string Text;

        public ImageButton(ImageBrush child, int width, int height)
        {
           this.Width = width;
           this.Height = height;
           NormalBackgroundColor = OnBackgroundColor = child;
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
                dc.DrawRectangle(OnBackgroundColor, new Pen(BorderColor), 0, 0, ActualWidth, ActualHeight);
            }
            else
            {
                dc.DrawRectangle(NormalBackgroundColor, new Pen(BorderColor), 0, 0, ActualWidth, ActualHeight);
            }
        }
    }
}
