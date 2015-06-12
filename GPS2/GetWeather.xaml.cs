using System;
using System.Collections.Generic;
using System.Net.Http;
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
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;    // Map Icon
using Windows.Storage.Streams;          // Marker Icon
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq; // using json.net
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GPS2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GetWeather : Page
    {
        public GetWeather()
        {
            this.InitializeComponent();
            startWeather();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void startWeather()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync();
                double Lat, Long;
                Lat = geoposition.Coordinate.Point.Position.Latitude;
                Long = geoposition.Coordinate.Point.Position.Longitude;

                var client = new HttpClient(); // Add: using System.Net.Http;
                var response = await client.GetAsync(new Uri("http://api.openweathermap.org/data/2.5/weather?lat="+Lat.ToString()+"&lon="+Long.ToString()));
                var resultJson = await response.Content.ReadAsStringAsync();

                var result = JObject.Parse(resultJson); // convert to json net object
                double temp = Convert.ToDouble(result["main"]["temp"].ToString()) - 273.15;
                double humidity = Convert.ToDouble(result["main"]["humidity"].ToString());
                var cloudiness = (string)result["weather"][0]["description"];
                string city = result["sys"]["country"].ToString();
                string icon = "http://openweathermap.org/img/w/"+ result["weather"][0]["icon"] + ".png";
                
                
                //iconWeather.Source = ;
                this.temp.Text = temp.ToString("0.0") + " °C";
                this.humidity.Text = humidity.ToString("0.0") + "%";
                this.cloudiness.Text = cloudiness;
                this.city.Text = city;
                
                // Load bitmap image
                BitmapImage bitmapImage = new BitmapImage();
                Uri uri = new Uri(icon);
                bitmapImage.UriSource = uri;

                this.iconWeather.Source = bitmapImage;
                // Display data read from web site
                Debug.WriteLine(result);
            }
            //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
            //The second is that the user doesn't turned on the Location Services
            catch (Exception ex)
            {
                //exception
            }
        }

        private async void getCityName(string Lat, string Long)
        {
            var client = new HttpClient(); // Add: using System.Net.Http;
            var response = await client.GetAsync(new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + Lat.ToString() + "," + Long.ToString()));
            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(resultJson); // convert to json net object

            //string city = result["results"][0]["long_name"].Where(s => s["types"][0].ToString() == "administrative_area_level_1").ToString();
            //this.city.Text = city;         
        }
        private void Return_Home(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Show_Map(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShowMap));
        }

        private void Update_Weather(object sender, RoutedEventArgs e)
        {
            startWeather();
        }
    }
}
