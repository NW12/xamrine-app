
using System.Collections.Generic;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;
using xamarin.android.Db.Model;

namespace xamarin.android.Db
{
    public class Database
    {
        private static string DbPath { get { return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "FavoriteAddress.db"); } }

        public Message Create()
        {
            Message message = new Message();
            try
            {
                using (var connection = new SQLiteConnection(DbPath))
                {
                    connection.CreateTable<FavoriteAddress>();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                message.Success = false;
                message.Detail = " Unable to create database ";
            }

            return message;
        }

        public Message Insert(FavoriteAddress favoriteAddress)
        {
            Message message = new Message();
            try
            {
                using (var connection = new SQLiteConnection(DbPath))
                {
                    connection.Insert(favoriteAddress);
                }
                message.Detail = " Address added successfully ";
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                message.Success = false;
                message.Detail = " Unable to insert record ";
            }

            return message;
        }
        public Message List()
        {
            Message message = new Message();
            try
            {
                using (var connection = new SQLiteConnection(DbPath))
                {
                    message.Data = connection.Table<FavoriteAddress>().OrderByDescending(x=>x.Id).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                message.Success = false;
                message.Detail = " Unable to fetch records ";
            }

            return message;
        }

    } 
}