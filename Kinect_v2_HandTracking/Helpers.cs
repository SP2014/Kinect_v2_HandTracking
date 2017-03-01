using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Kinect_v2_HandTracking
{
    public static class Helpers
    {
        public static Point Scale(this Joint joint, CoordinateMapper mapper)
        {
            Point point = new Point();

            ColorSpacePoint colorPoint = mapper.MapCameraPointToColorSpace(joint.Position);
            point.X = float.IsInfinity(colorPoint.X) ? 0.0 : colorPoint.X;
            point.Y = float.IsInfinity(colorPoint.Y) ? 0.0 : colorPoint.Y;

            return point;
        }


        public static void DrawThumb(this Canvas canvas, Joint thumb, CoordinateMapper mapper)
        {
            if (thumb.TrackingState == TrackingState.NotTracked) return;

            Point point = thumb.Scale(mapper);

            Ellipse ellipse = new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = new SolidColorBrush(Colors.LightBlue),
                Opacity = 0.7
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }


        public static void DrawHand(this Canvas canvas, Joint hand, CoordinateMapper mapper)
        {
            if (hand.TrackingState == TrackingState.NotTracked) return;

            Point point = hand.Scale(mapper);

            Ellipse ellipse = new Ellipse
            {
                Width = 100,
                Height = 100,
                Stroke = new SolidColorBrush(Colors.LightBlue),
                StrokeThickness = 4
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

    }
}
