﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using xamarin.android.Db;
using xamarin.android.Db.Model;
using Newtonsoft.Json;
using Android.Views.InputMethods;
using Android;
using System.Threading.Tasks;

namespace xamarin.android
{
    [Activity(Label = "New Address")]
    public class AddNewAddressActivity : AppCompatActivity
    {
        private Database Db = Static.Db;
        AutoCompleteTextView autoCompleteTextView;
        private static List<Result> results;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NewAddress);
            Button btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            btnAdd.Click += BtnAdd_Click;

            autoCompleteTextView = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteAddress); 
            autoCompleteTextView.AfterTextChanged += AutoCompleteTextView_AfterTextChanged;
            autoCompleteTextView.Threshold = 1;
            Button btnClose = FindViewById<Button>(Resource.Id.btnClose);
            btnClose.Click += StartMainActivity;

        }

        private void AutoCompleteTextView_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                PopulateAddressesAsync();
            }); 
        }

        private void StartMainActivity(object sender, EventArgs eventArgs)
        {
            this.Finish();
            StartActivity(typeof(MainActivity));
        }
        
        // calling google api to fetch locations if quota is exceeded than seeded locally, pushing data to autocomplete adapter we can 
        // also make custom adapter to push objects
        private async Task PopulateAddressesAsync()
        {
            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage responseMessage = await client.GetAsync("https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + autoCompleteTextView.Text + "&key=AIzaSyC63lRyNMGXGvetx1o_fhsPZUhT1OEHC_0");

                string resultString = await responseMessage.Content.ReadAsStringAsync();

                GoogleSearch googleSearch = JsonConvert.DeserializeObject<GoogleSearch>(resultString);
                if (googleSearch.results.Count == 0)
                {
                    resultString = "{ \"html_attributions\" : [], \"next_page_token\" : \"CuQB3QAAAMyBf1oj0lJmyWMpR3C0fTPnEZYu_yA8lkk3LXDikg4QDF7t5yw7Ovh6WSHPWHqezBoIfx-kqFMD9786ICb3gdf9jDuMGqWT7rQFrDCrY7FFZ9KYeLu0QIJkvV7y_2xXquipyrQXIZT1vyYj7o1t4dSMrLyuqwh2kDjQ82naFzwXItNm8kWvih7NVB1rS-rNBR0KDYEqjF59vrK8Gr7OYTi1LabArAl5sVJKQKE-6NcWu55FnzusbBH4KjQ_aT6mdkYzz2V0pwJjOjkrqUYhfnkTtZyxg-V2WEnMfeoaNRyiEhAxEDgPvQns-D5AfBdAP6CrGhRI7arLdWZDvGJlkw1pzUXz9P6FhQ\", \"results\" : [ { \"formatted_address\" : \"39-A Main Blvd, Block R 1 Phase 2 Johar Town, Lahore, Punjab 54000, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4587052, \"lng\" : 74.27607949999999 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.46004727989272, \"lng\" : 74.27739352989272 }, \"southwest\" : { \"lat\" : 31.45734762010728, \"lng\" : 74.27469387010727 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"633f108c3810e98dbf62b1ebbb71ef20fc904b52\", \"name\" : \"CNS Rent a Car Lahore - Johar Town\", \"opening_hours\" : { \"open_now\" : true }, \"place_id\" : \"ChIJObCg8E8BGTkRtfwylcwmbe0\", \"plus_code\" : { \"compound_code\" : \"F75G+FC Lahore\", \"global_code\" : \"8J3PF75G+FC\" }, \"rating\" : 5, \"reference\" : \"ChIJObCg8E8BGTkRtfwylcwmbe0\", \"types\" : [ \"car_rental\", \"car_repair\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 1 }, { \"formatted_address\" : \"Office no.7, Al-Rehman Center, Hakim Chowk, PIA Main Boulevard, Block A 1 Pia Housing Scheme, Lahore, Punjab 54000, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4505388, \"lng\" : 74.2855529 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45191392989272, \"lng\" : 74.28687082989272 }, \"southwest\" : { \"lat\" : 31.44921427010728, \"lng\" : 74.28417117010727 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"4063cb16ff34117eba39c5a0955fb1a33162b3e2\", \"name\" : \"Azaan Motors & Rent A Car\", \"opening_hours\" : { \"open_now\" : true }, \"place_id\" : \"ChIJORfwgmkBGTkRGJYDZhKePUA\", \"plus_code\" : { \"compound_code\" : \"F72P+66 Lahore\", \"global_code\" : \"8J3PF72P+66\" }, \"rating\" : 4.5, \"reference\" : \"ChIJORfwgmkBGTkRGJYDZhKePUA\", \"types\" : [ \"car_rental\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 2 }, { \"formatted_address\" : \"5 F-1, Block F 1 Phase 1 Johar Town, Lahore, Punjab 54000, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.457814, \"lng\" : 74.27901 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45909217989272, \"lng\" : 74.28042832989273 }, \"southwest\" : { \"lat\" : 31.45639252010728, \"lng\" : 74.27772867010728 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"400dfe8213b668564f89b86295c904c521ac7915\", \"name\" : \"4 Wheels Rent a Car\", \"opening_hours\" : { \"open_now\" : true }, \"photos\" : [ { \"height\" : 1529, \"html_attributions\" :  [], \"photo_reference\" : \"CmRaAAAAEAj50lCha7Ez77UdsdDwxVwTLpurotK8nKlarWQj7lCmuRzLX82rT25Q946HtO_RtyF-knWvNSGIsDZVqZ_4pWHo_uDmVJ4XH6TTqt0YtNC91hGdlvcKMQtNTxM5uGvuEhCocJGBpSxaK_YwKB2_zrAbGhSsTR3baRVe9xz-eUOIXbCPvvM5ww\", \"width\" : 2048 } ], \"place_id\" : \"ChIJSx5oCWABGTkR4MoBH7SfPiQ\", \"plus_code\" : { \"compound_code\" : \"F75H+4J Lahore\", \"global_code\" : \"8J3PF75H+4J\" }, \"rating\" : 4.4, \"reference\" : \"ChIJSx5oCWABGTkR4MoBH7SfPiQ\", \"types\" : [ \"car_rental\", \"real_estate_agency\", \"travel_agency\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 29 }, { \"formatted_address\" : \"Plot 95, Block R3 Block R 3 Phase 2 Johar Town, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4499642, \"lng\" : 74.27226809999999 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45132177989272, \"lng\" : 74.27361837989272 }, \"southwest\" : { \"lat\" : 31.44862212010728, \"lng\" : 74.27091872010728 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"f2cca05f07f6b4d49787120a6d4ca64e3b6dbce7\", \"name\" : \"Baba Fareed Rent A Car\", \"opening_hours\" : { \"open_now\" : true }, \"photos\" : [ { \"height\" : 5248, \"html_attributions\" :  [], \"photo_reference\" : \"CmRaAAAAwKZJFx2f2gm09zzdnlwz6KgRzZwalmPrhzCkXO21vePM6mGNIgJKnTesPjEM14qVntMxPbHflUAZPt3oKnJPEXRXELaJZQk-ZLOxB9w1kJvb401VKwpIhuyC8E9V3T6FEhA1DdFdu4zFFkakpJxFs9-qGhQY7mhG4N0G-vO3abQBfv75awLavg\", \"width\" : 2952 } ], \"place_id\" : \"ChIJdc1-bzMBGTkRyfkqMmqV0r4\", \"plus_code\" : { \"compound_code\" : \"C7XC+XW Lahore\", \"global_code\" : \"8J3PC7XC+XW\" }, \"rating\" : 5, \"reference\" : \"ChIJdc1-bzMBGTkRyfkqMmqV0r4\", \"types\" : [ \"car_rental\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 5 }, { \"formatted_address\" : \"82، Block D 1 Phase 1 Johar Town, Lahore, Punjab 54000, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.454063, \"lng\" : 74.28460299999999 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45540502989272, \"lng\" : 74.28594242989273 }, \"southwest\" : { \"lat\" : 31.45270537010727, \"lng\" : 74.28324277010728 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"9871d94e93a1604e3ff3c3c18b8ec89870d65050\", \"name\" : \"Haider Travels & Rent A Car\", \"opening_hours\" : { \"open_now\" : true }, \"photos\" : [ { \"height\" : 721, \"html_attributions\" :  [], \"photo_reference\" : \"CmRaAAAABgnLnML_ioIE6d45P-yKNUcO_nJ09gp5Iv4vXlBSb0hrvwYwoAG4HuR4o_6Luea9Bhj96rzM7oJM-EeHIdL7N0MohjwZIyRLisKxZpIKm5u8VO9lbMqbDkxs4bUEnfqQEhCIDuNvcncFgApfpT38IovsGhTgYkslnNtG3Q6gcR1-DFzcaslK2g\", \"width\" : 1280 } ], \"place_id\" : \"ChIJCarjKZEBGTkRbKnK4imoeZ4\", \"plus_code\" : { \"compound_code\" : \"F73M+JR Lahore\", \"global_code\" : \"8J3PF73M+JR\" }, \"rating\" : 5, \"reference\" : \"ChIJCarjKZEBGTkRbKnK4imoeZ4\", \"types\" : [ \"car_rental\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 10 }, { \"formatted_address\" : \"7-GCP Housing Scheem، Block R 1 Phase 2 Johar Town, Lahore, Punjab 54000, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.450815, \"lng\" : 74.26876399999999 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45216602989272, \"lng\" : 74.27011932989272 }, \"southwest\" : { \"lat\" : 31.44946637010728, \"lng\" : 74.26741967010729 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"09199e0af23c9ac2dac17f2ecef2c989821a6931\", \"name\" : \"Lahore Rent A Car (Johar Town)\", \"opening_hours\" : { \"open_now\" : true }, \"photos\" : [ { \"height\" : 960, \"html_attributions\" :  [], \"photo_reference\" : \"CmRaAAAAPO2SHZsCA3NVpuUg6BKztL3Fn_oWF5jAhCe9mr0MRIfID7GHAtmdTV5cTp3tI9Bt9tNKyeVYLmo8e4ClWq6Y7Hzslpn_c82-Jt1nvBbZDllDWBT1lqIxZ1PeFARavVq2EhBLEGxgUxoU3AhfcJRlj3o_GhQmYXmr3nOMdci8IHov3U47y5h4Rw\", \"width\" : 960 } ], \"place_id\" : \"ChIJcw04HXUBGTkR3HwajbA6B2c\", \"plus_code\" : { \"compound_code\" : \"F729+8G Lahore\", \"global_code\" : \"8J3PF729+8G\" }, \"rating\" : 5, \"reference\" : \"ChIJcw04HXUBGTkR3HwajbA6B2c\", \"types\" : [ \"car_rental\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 2 }, { \"formatted_address\" : \"Block A Revenue Society Block A Revenue Employees Cooperative Housing Society, Lahore, Punjab 54770, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4533072, \"lng\" : 74.28158499999999 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45459207989272, \"lng\" : 74.28293062989272 }, \"southwest\" : { \"lat\" : 31.45189242010728, \"lng\" : 74.28023097010727 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/shopping-71.png\", \"id\" : \"b172dfd96144cb0a941fb08d9ea0cae6094894dc\", \"name\" : \"A Block market\", \"opening_hours\" : { \"open_now\" : true }, \"place_id\" : \"ChIJf_3naEgBGTkRYD8V_YdHfWI\", \"plus_code\" : { \"compound_code\" : \"F73J+8J Lahore\", \"global_code\" : \"8J3PF73J+8J\" }, \"rating\" : 0, \"reference\" : \"ChIJf_3naEgBGTkRYD8V_YdHfWI\", \"types\" : [ \"shopping_mall\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 0 }, { \"formatted_address\" : \"G, 30 Lane 4, Block A Abdalians Cooperative Housing Society, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.456548, \"lng\" : 74.2723209 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45792812989272, \"lng\" : 74.27367102989271 }, \"southwest\" : { \"lat\" : 31.45522847010727, \"lng\" : 74.27097137010726 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"92eee8504a4fbbbf194781e9ce9644d9ff0eeacf\", \"name\" : \"A ONE Media\", \"opening_hours\" : { \"open_now\" : true }, \"place_id\" : \"ChIJKQjtSnoBGTkR0SkzeILiYyo\", \"plus_code\" : { \"compound_code\" : \"F74C+JW Lahore\", \"global_code\" : \"8J3PF74C+JW\" }, \"rating\" : 3.3, \"reference\" : \"ChIJKQjtSnoBGTkR0SkzeILiYyo\", \"types\" : [ \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 4 }, { \"formatted_address\" : \"311, near Shaukat Khanum Hospital, Block R3 Block R 3 BR-3 Johar Town, Lahore, Punjab 54000, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4477453, \"lng\" : 74.27041659999999 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.44909512989273, \"lng\" : 74.27176642989271 }, \"southwest\" : { \"lat\" : 31.44639547010728, \"lng\" : 74.26906677010727 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"b05a3acc8c1599768194e6a29cd24e548ade15e6\", \"name\" : \"Fraz Rent A Car - Rent a Car In Lahore\", \"opening_hours\" : { \"open_now\" : true }, \"photos\" : [ { \"height\" : 952, \"html_attributions\" :  [], \"photo_reference\" : \"CmRaAAAAl-hjX9FpVnNPSKbnejJkkcWmNRQ4j0smHjQ-Tx7eJlIE0Bmzlk1etxJQZDTODJjRHJm_Z59jegGXhgeYCiuH-FJAmi7pSQzVh7RwSuQVfxYPHe-fWnUqMUlIp8g3xZAIEhANoAzAupYAgw7OLW8Kz9UqGhTux2dJY1jq6N48GuXPLNXNYt1-Kw\", \"width\" : 960 } ], \"place_id\" : \"ChIJwUxWw7oBGTkR2XIQSGmjmnA\", \"plus_code\" : { \"compound_code\" : \"C7XC+35 Lahore\", \"global_code\" : \"8J3PC7XC+35\" }, \"rating\" : 5, \"reference\" : \"ChIJwUxWw7oBGTkR2XIQSGmjmnA\", \"types\" : [ \"car_rental\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 2 }, { \"formatted_address\" : \"Block R1 Block R 1 Phase 2 Johar Town, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4553665, \"lng\" : 74.27585239999999 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45667432989272, \"lng\" : 74.27724532989271 }, \"southwest\" : { \"lat\" : 31.45397467010728, \"lng\" : 74.27454567010727 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"801b95a265f65c99135bdc96d1ab0a429b63b3b8\", \"name\" : \"Muhammad Saeed's Traders Jdmmst Motorsports A Place Where Ubcan Get Performance Parts And Complete Mechanical Work\", \"opening_hours\" : { \"open_now\" : true }, \"photos\" : [ { \"height\" : 2432, \"html_attributions\" :  [], \"photo_reference\" : \"CmRaAAAAqfyuLHWaPsZmKlOhp4yRYs2RNZ7Nxo_7HBHCCNXI5HmP5AYGJbPO1GuZEvNi1MJ-xoQtCqw0lJSIP1NI5ZwwnMKkElNyB7gBikMJRDT7cGevHFiANBxNxZjxdz6Ot4kYEhBRiMSu-1N-p715KAFvIbptGhTfuBU3P6D2nnf0cC_YjsPFhEWatA\", \"width\" : 4320 } ], \"place_id\" : \"ChIJ5VsCunoBGTkROOWhaQRkr2c\", \"plus_code\" : { \"compound_code\" : \"F74G+48 Lahore\", \"global_code\" : \"8J3PF74G+48\" }, \"rating\" : 4, \"reference\" : \"ChIJ5VsCunoBGTkROOWhaQRkr2c\", \"types\" : [ \"car_repair\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 8 }, { \"formatted_address\" : \"Block B Pia Housing Scheme, Lahore, Punjab 54770, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4523902, \"lng\" : 74.27825469999999 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45376877989272, \"lng\" : 74.27960607989272 }, \"southwest\" : { \"lat\" : 31.45106912010727, \"lng\" : 74.27690642010727 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"ebd47b74a7c7ad5f2c7e22adc7c8c2c6171dcd24\", \"name\" : \"71-A\", \"place_id\" : \"ChIJV32pt28BGTkRjFVJii-kbqs\", \"plus_code\" : { \"compound_code\" : \"F72H+X8 Lahore\", \"global_code\" : \"8J3PF72H+X8\" }, \"rating\" : 0, \"reference\" : \"ChIJV32pt28BGTkRjFVJii-kbqs\", \"types\" : [ \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 0 }, { \"formatted_address\" : \"Block A Pia Housing Scheme, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.45300709999999, \"lng\" : 74.2791857 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.4560178, \"lng\" : 74.2844518 }, \"southwest\" : { \"lat\" : 31.4516995, \"lng\" : 74.277162 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/geocode-71.png\", \"id\" : \"863814eab9b9069698e4650943b705184b477ec6\", \"name\" : \"Block A\", \"place_id\" : \"ChIJCz7Oa28BGTkRfaF8WyjWGeE\", \"reference\" : \"ChIJCz7Oa28BGTkRfaF8WyjWGeE\", \"types\" : [ \"sublocality_level_2\", \"sublocality\", \"political\" ] }, { \"formatted_address\" : \"Block A Revenue Employees Cooperative Housing Society, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4531645, \"lng\" : 74.2832196 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.459579, \"lng\" : 74.2857853 }, \"southwest\" : { \"lat\" : 31.451708, \"lng\" : 74.2768454 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/geocode-71.png\", \"id\" : \"d9d9db0951ada27e7ede2cb76bb398a7e22fdf21\", \"name\" : \"Block A\", \"place_id\" : \"ChIJKQQU8mUBGTkRkvchujaedIE\", \"reference\" : \"ChIJKQQU8mUBGTkRkvchujaedIE\", \"types\" : [ \"sublocality_level_2\", \"sublocality\", \"political\" ] }, { \"formatted_address\" : \"Block A Abdalians Cooperative Housing Society, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4570781, \"lng\" : 74.2700174 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.4584869, \"lng\" : 74.2728594 }, \"southwest\" : { \"lat\" : 31.4556774, \"lng\" : 74.2661779 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/geocode-71.png\", \"id\" : \"06d06b7d32bf08f1a0b42f55260a0e5492691393\", \"name\" : \"Block A\", \"place_id\" : \"ChIJnc-WbXkBGTkR8WdwgZkNOXQ\", \"reference\" : \"ChIJnc-WbXkBGTkR8WdwgZkNOXQ\", \"types\" : [ \"neighborhood\", \"political\" ] }, { \"formatted_address\" : \"Block A Revenue Society Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4539788, \"lng\" : 74.281386 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.4571897, \"lng\" : 74.2858868 }, \"southwest\" : { \"lat\" : 31.4516572, \"lng\" : 74.27794489999999 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/geocode-71.png\", \"id\" : \"8e825b33e853180efa5bfcdef7e202f81f0d40d8\", \"name\" : \"Block A Revenue Society\", \"place_id\" : \"ChIJ5y8sUG8BGTkRhtajnKymMsY\", \"reference\" : \"ChIJ5y8sUG8BGTkRhtajnKymMsY\", \"types\" : [ \"neighborhood\", \"political\" ] }, { \"formatted_address\" : \"Block A Abdalians Cooperative Housing Society, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4570781, \"lng\" : 74.2700174 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.4585715, \"lng\" : 74.27292899999999 }, \"southwest\" : { \"lat\" : 31.45565419999999, \"lng\" : 74.2660379 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/geocode-71.png\", \"id\" : \"7213919f30ece2256103f4542d9a1901fdda3d04\", \"name\" : \"Block A\", \"place_id\" : \"ChIJkROhbXkBGTkRAeDl45-b_g0\", \"reference\" : \"ChIJkROhbXkBGTkRAeDl45-b_g0\", \"types\" : [ \"sublocality_level_2\", \"sublocality\", \"political\" ] }, { \"formatted_address\" : \"14f3, Main Blvd, Block R 1 Phase 2 punjab society, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.45868219999999, \"lng\" : 74.2760864 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.46003977989272, \"lng\" : 74.27747202989272 }, \"southwest\" : { \"lat\" : 31.45734012010727, \"lng\" : 74.27477237010729 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"dae5f8efe647df07034897b2e89fa32b809160b7\", \"name\" : \"A/C DOCTORS LAHORE\", \"opening_hours\" : { \"open_now\" : true }, \"photos\" : [ { \"height\" : 960, \"html_attributions\" :  [], \"photo_reference\" : \"CmRaAAAA7HM3IaHi1jEPj-rS2hPUozR1yxn4Mp17pVtIdX3V0Xivy5Ae9BIEymhUKVOISp3x1CpqB46YPrzG5Mi6seJedsoB2MT_aLQ1-7WufbZy8aPYFDHDScMHK6k6pZ3aY4EAEhBEVjOhh3j94nlc7PX55JpgGhRtDfXRfkc7syGuuaR8FMHdSh6J_Q\", \"width\" : 720 } ], \"place_id\" : \"ChIJzxw7r8MBGTkR1WxFNKSKVvQ\", \"plus_code\" : { \"compound_code\" : \"F75G+FC Lahore\", \"global_code\" : \"8J3PF75G+FC\" }, \"rating\" : 0, \"reference\" : \"ChIJzxw7r8MBGTkR1WxFNKSKVvQ\", \"types\" : [ \"general_contractor\", \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 0 }, { \"formatted_address\" : \"Off No.1 2nd Floor, Chaudhry Plaza Meer Daad Chowk, Main Blvd, Block N Phase 2 Johar Town, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4610916, \"lng\" : 74.2758108 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.46240962989273, \"lng\" : 74.27719407989272 }, \"southwest\" : { \"lat\" : 31.45970997010728, \"lng\" : 74.27449442010727 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"612172aa504a0eefdbc7fd973020735176ebae33\", \"name\" : \"Ibrahim Rent A Car\", \"opening_hours\" : { \"open_now\" : true }, \"photos\" : [ { \"height\" : 720, \"html_attributions\" :  [], \"photo_reference\" : \"CmRaAAAA2QGDLHy51RuIIoztzMTiRm95CSRMGwYaPDV8PrZfOaiatagRXzRLccfyqGH3LcVh2YpEh--Fw4jSnpAoHjdq0afsVQFivVBFhyitzaVa_Bq4qmLMFYCexiVyieLKoDsmEhD0Kwv__kUot1-mY6FTyJYeGhSZDxWUNdaNV8MaCE0sly9mFcu6Cw\", \"width\" : 1280 } ], \"place_id\" : \"ChIJVwwVJH0BGTkRh0tljo1Gqc4\", \"plus_code\" : { \"compound_code\" : \"F76G+C8 Lahore\", \"global_code\" : \"8J3PF76G+C8\" }, \"rating\" : 5, \"reference\" : \"ChIJVwwVJH0BGTkRh0tljo1Gqc4\", \"types\" : [ \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 4 }, { \"formatted_address\" : \"7-GCP scheem, Phase 2, Block R1 Block R 1 Phase 2 Johar Town, Lahore, Punjab, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4508146, \"lng\" : 74.26877709999999 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.45216292989272, \"lng\" : 74.27012022989273 }, \"southwest\" : { \"lat\" : 31.44946327010727, \"lng\" : 74.26742057010728 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"982883c3670274932aa0e9d8b887e105a2102c83\", \"name\" : \"Lahore Rent A Car\", \"opening_hours\" : { \"open_now\" : true }, \"place_id\" : \"ChIJZfKuWDYBGTkRjog2VklqgWg\", \"plus_code\" : { \"compound_code\" : \"F729+8G Lahore\", \"global_code\" : \"8J3PF729+8G\" }, \"rating\" : 5, \"reference\" : \"ChIJZfKuWDYBGTkRjog2VklqgWg\", \"types\" : [ \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 1 }, { \"formatted_address\" : \"F Block Block D 2 Phase 1 Johar Town, Lahore, Punjab 54000, Pakistan\", \"geometry\" : { \"location\" : { \"lat\" : 31.4616142, \"lng\" : 74.2846804 }, \"viewport\" : { \"northeast\" : { \"lat\" : 31.46296317989272, \"lng\" : 74.28603112989272 }, \"southwest\" : { \"lat\" : 31.46026352010728, \"lng\" : 74.28333147010729 } } }, \"icon\" : \"https://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\", \"id\" : \"02627de20368499482d5b41969c5065764a85b82\", \"name\" : \"Lahore Rent A Car\", \"place_id\" : \"ChIJC-cu5sIBGTkRtHkbzlryKjg\", \"plus_code\" : { \"compound_code\" : \"F76M+JV Lahore\", \"global_code\" : \"8J3PF76M+JV\" }, \"rating\" : 0, \"reference\" : \"ChIJC-cu5sIBGTkRtHkbzlryKjg\", \"types\" : [ \"point_of_interest\", \"establishment\" ], \"user_ratings_total\" : 0 } ], \"status\" : \"OK\" }";
                    googleSearch = JsonConvert.DeserializeObject<GoogleSearch>(resultString);
                    Toast toast = Toast.MakeText(this, " Google api usage limit exceed, loading from seeded data, type j or f ", ToastLength.Long);
                    toast.View.SetBackgroundColor(Android.Graphics.Color.LightBlue);
                    toast.Show();
                }
                results = googleSearch.results;
                autoCompleteTextView.Adapter = new ArrayAdapter<String>(this, Resource.Layout.support_simple_spinner_dropdown_item,
                   results.Select(x => x.formatted_address).ToArray());

            }
            catch (Exception ex)
            {
                Toast toast = Toast.MakeText(this, " Something went wrong ", ToastLength.Long);
                toast.View.SetBackgroundColor(Android.Graphics.Color.Red);
                toast.Show();
            }

        }
        //insert address to local database
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string address = FindViewById<EditText>(Resource.Id.autoCompleteAddress).Text;
            results = ReferenceEquals(results, null) ? new List<Result>() : results;
            Result foundResult = results.FirstOrDefault(x => x.formatted_address.ToLower() == address.ToLower());

            if (!string.IsNullOrEmpty(address) && !ReferenceEquals(foundResult, null))
            {
                Message message = Db.Insert(new FavoriteAddress()
                {
                    Address = address,
                    Longitude = foundResult.geometry.location.lng,
                    Latitude = foundResult.geometry.location.lat
                });
                Toast toast = Toast.MakeText(this, message.Detail, ToastLength.Long);
                toast.View.SetBackgroundColor(message.Success ? Android.Graphics.Color.Green : Android.Graphics.Color.Red);
                toast.Show();
                this.Finish();
                StartActivity(typeof(MainActivity));
            }
            else
            {
                Toast toast = Toast.MakeText(this, " Invalid address, please select valid address ", ToastLength.Long);
                toast.View.SetBackgroundColor(Android.Graphics.Color.LightBlue);
                toast.Show();
            }

        }

    }

}