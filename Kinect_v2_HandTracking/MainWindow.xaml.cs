using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Add Kinect dependencies
using Microsoft.Kinect;
using LightBuzz.Vitruvius;


namespace Kinect_v2_HandTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies = null;

        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get a reference of the kinect sensor
            _sensor = KinectSensor.GetDefault();

            // Check if Sensor is not null
            if (_sensor != null)
            {
                // Get reference for multi source frame reader
                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Body);
                // Whenever data is arrived at the multi source frame reader we need a callback function to perform some actions
                _reader.MultiSourceFrameArrived += _reader_MultiSourceFrameArrived;

                // Open the kinect sensor
                try {
                    _sensor.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

        }

        void _reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            // Get a reference for the frame arrived
            var reference = e.FrameReference.AcquireFrame();


            // Processing Color frame
            using(var frame = reference.ColorFrameReference.AcquireFrame()){

                if (frame != null) { 
                 // Set image to Image view by converting the image into bitmap
                    this.image.Source = frame.ToBitmap();
                }
            }



            // Processing Body Frame
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // Remove any existing child controls from the canvas
                    this.canvas.Children.Clear();
                    
                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    // Get the most recent data from the acquired frame
                    frame.GetAndRefreshBodyData(_bodies);

                    // Itereate through each body present in the frame
                    foreach (Body body in _bodies) {

                        if (body != null) { 
                          // Check if currently the sensor is tracking any body
                            if (body.IsTracked) {

                                //Right hand and thumb
                                Joint right_hand = body.Joints[JointType.HandRight];
                                Joint right_thumb = body.Joints[JointType.ThumbRight];

                                //Left hand and thumb
                                Joint left_hand = body.Joints[JointType.HandLeft];
                                Joint left_thumb = body.Joints[JointType.ThumbLeft];

                                if (right_hand.TrackingState == TrackingState.Tracked)
                                {
                                    canvas.DrawHand(right_hand, _sensor.CoordinateMapper);
                                    canvas.DrawThumb(right_thumb, _sensor.CoordinateMapper);
                                }
                                if (left_hand.TrackingState == TrackingState.Tracked)
                                {
                                    canvas.DrawHand(left_hand, _sensor.CoordinateMapper);
                                    canvas.DrawThumb(left_thumb, _sensor.CoordinateMapper);
                                }

                            }
                        }
                    }



                }
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null) {
                _reader.Dispose();
            }
            if (_sensor != null)
            {
                _sensor.Close();
            }
        }
    }
}
