using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;    // Map Icon
using Windows.Storage.Streams;          // Marker Icon
using System.Threading;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GPS2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowMap : Page
    {
        public ShowMap()
        {
            this.InitializeComponent();
            Start_map();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void Start_map()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync();

                //With this 2 lines of code, the app is able to write on a Text Label the Latitude and the Longitude, given by {{Icode|geoposition}}
                geolocation.Text = "GPS:" + geoposition.Coordinate.Point.Position.Latitude.ToString("0.00") + ", " + geoposition.Coordinate.Point.Position.Longitude.ToString("0.00");
                Show_Map(geoposition.Coordinate.Point.Position.Latitude, geoposition.Coordinate.Point.Position.Longitude);
            }
            //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
            //The second is that the user doesn't turned on the Location Services
            catch (Exception ex)
            {
                //exception
            }
        }

        // function to show map
        private void Show_Map(double Lat, double Long)
        {
            AddMapIcon(Lat, Long);

            MapControl1.Center =
                new Geopoint(new BasicGeoposition()
                {
                    Latitude = Lat,
                    Longitude = Long
                });
            MapControl1.ZoomLevel = 12;
            MapControl1.LandmarksVisible = true;

        }

        // fucntion to show the map icon
        private void AddMapIcon(double Lat, double Long)
        {
            MapIcon MapIcon1 = new MapIcon();
            MapIcon1.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = Lat,
                Longitude = Long
            });
            MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            //MapIcon1.Image = RandomAccessStreamReference.CreateFromUri(new Uri("http://simpleicon.com/wp-content/uploads/map-marker-2.png"));
            MapIcon1.Title = "Space Needle";
            MapControl1.MapElements.Add(MapIcon1);
        }

        private void Update_Location(object sender, RoutedEventArgs e)
        {
            Start_map();
        }

        private void Return_Home(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Show_Weather(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GetWeather));
        }


    }
}
