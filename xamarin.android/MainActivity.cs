using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using xamarin.android.Db;
using xamarin.android.Db.Model;

namespace xamarin.android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        ListView listView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Create Database  
            Static.Db.Create();

            //finding list view and loading addresses
            listView = FindViewById<ListView>(Resource.Id.listView);
            LoadAddresses();

            //Events
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            FloatingActionButton mpclk = FindViewById<FloatingActionButton>(Resource.Id.Btnmap);
            mpclk.Click += MaponClick;


        }
        private void LoadAddresses()
        {
            Message message = Static.Db.List();
            if (!message.Success)
            {
                Toast toast = Toast.MakeText(this, message.Detail, ToastLength.Long);
                toast.View.SetBackgroundColor(Android.Graphics.Color.Red);
                toast.Show();
            }
            else
            {
                var address = message.Data.Select(x => x.Address).ToList();
                listView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, address);

            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            StartActivity(typeof(AddNewAddressActivity));
        }
        private void MaponClick(object sender, EventArgs eventArgs)
        {
            SetContentView(Resource.Layout.Map);
            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.mapFragment);
            mapFragment.GetMapAsync(this);
            Button mpclkClose = FindViewById<Button>(Resource.Id.btnCloseMap);
            mpclkClose.Click += StartMainActivity;

        }
        private void StartMainActivity(object sender, EventArgs eventArgs)
        {
            StartActivity(typeof(MainActivity));
        }
        //loading markers on map from db
        public void OnMapReady(GoogleMap map)
        {
            map.UiSettings.ZoomControlsEnabled = true;
            BitmapDescriptor bitmapDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan);
            MarkerOptions markerOptions;
            Message message = Static.Db.List();
            message.Data.ForEach(x =>
            {
                try
                {
                    markerOptions = new MarkerOptions();
                    markerOptions.SetPosition(new LatLng(x.Latitude, x.Longitude));
                    markerOptions.SetTitle(x.Address);
                    markerOptions.SetIcon(bitmapDescriptor);
                    map.AddMarker(markerOptions);
                }
                catch { }
            });


        }
    }
}

