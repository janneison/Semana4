using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ControlMapa.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media.Imaging;
using Windows.Devices.Geolocation;
using System.Device.Location;

namespace ControlMapa
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

            CreateLocator();
            CreatePushpins();
        }

        private async void CreateLocator()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.MovementThreshold = 10;
            //geolocator.DesiredAccuracy = PositionAccuracy.High;
            geolocator.DesiredAccuracyInMeters = 5;
            geolocator.PositionChanged += geolocator_PositionChanged;
            
            await geolocator.GetGeopositionAsync();
        }

        

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (App.IsRunningInBackground)
                {
                    ShellToast toast = new ShellToast();
                    toast.Content = string.Format("Lat: {0} Long: {1}", args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
                    toast.Title = "Cambio de ubicación";
                    toast.Show();
                }
                else
                {
                    map.SetView(new GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude), 15, MapAnimationKind.Parabolic);
                    //map.Center = new GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
                }
            });
        }


        private void CreatePushpins()
        {
            var overlay = new MapOverlay();
            overlay.GeoCoordinate = new System.Device.Location.GeoCoordinate(19.433481, -99.134065);
            BitmapImage bmp = new BitmapImage(new Uri("/mexico.jpg", UriKind.Relative));
            overlay.Content = new Image() { Source = bmp, Width = 50 };

            var overlay2 = new MapOverlay();
            overlay2.GeoCoordinate = new System.Device.Location.GeoCoordinate(-12.115488, -77.044122);
            BitmapImage bmp2 = new BitmapImage(new Uri("/peru.jpg", UriKind.Relative));
            overlay2.Content = new Image() { Source = bmp2, Width=50 };

            var layer = new MapLayer();
            layer.Add(overlay);
            layer.Add(overlay2);

            map.Layers.Add(layer);
        }


        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarButtonText;
            appBarButton.Click += (s, a) =>
            {
                map.ColorMode = map.ColorMode == Microsoft.Phone.Maps.Controls.MapColorMode.Light ? Microsoft.Phone.Maps.Controls.MapColorMode.Dark :
                    Microsoft.Phone.Maps.Controls.MapColorMode.Light;
            };
            ApplicationBar.Buttons.Add(appBarButton);

            
            ApplicationBarIconButton appBarButton2 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton2.Text = "Lima";
            appBarButton2.Click += (s, a) =>
            {
                map.SetView(new System.Device.Location.GeoCoordinate()
                {
                    Latitude = -12.115488,
                    Longitude = -77.044122
                }, 15, Microsoft.Phone.Maps.Controls.MapAnimationKind.Parabolic);
            };
            ApplicationBar.Buttons.Add(appBarButton2);

            
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem("Road");
            appBarMenuItem.Click += (s, a) => map.CartographicMode = Microsoft.Phone.Maps.Controls.MapCartographicMode.Road;

            ApplicationBarMenuItem appBarMenuItem2 = new ApplicationBarMenuItem("Aerial");
            appBarMenuItem2.Click += (s, a) => map.CartographicMode = Microsoft.Phone.Maps.Controls.MapCartographicMode.Aerial;

            ApplicationBarMenuItem appBarMenuItem3 = new ApplicationBarMenuItem("Hybrid");
            appBarMenuItem3.Click += (s, a) => map.CartographicMode = Microsoft.Phone.Maps.Controls.MapCartographicMode.Hybrid;

            ApplicationBarMenuItem appBarMenuItem4 = new ApplicationBarMenuItem("Terrain");
            appBarMenuItem4.Click += (s, a) => map.CartographicMode = Microsoft.Phone.Maps.Controls.MapCartographicMode.Terrain;

            ApplicationBar.MenuItems.Add(appBarMenuItem);
            ApplicationBar.MenuItems.Add(appBarMenuItem2);
            ApplicationBar.MenuItems.Add(appBarMenuItem3);
            ApplicationBar.MenuItems.Add(appBarMenuItem4);
        }
    }
}