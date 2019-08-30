using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace hearingapp_otc.Classes
{

    public class DatabaseHelper
    {
        public DatabaseHelper()
        {

        }

        // Add new Sessions to the database
        
        public static bool Insert<T>(ref T data, string db_path)
        {
            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
                conn.CreateTable<T>();

                if (conn.Insert(data) != 0)
                    return true;
            }

            return false;
        }
        

        /*
        public static bool Insert<Session>(hearingapp_otc.Classes.Session newSess, string db_path)
        {
            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
                conn.CreateTable<Session>();

                AWSHelper.RegisterNewSession(newSess);

                if (conn.Insert(newSess) != 0)
                    return true;
            }

            return false;
        }
        */

        // Grab all Sessions from the database and return them as a list
        public static List<Session> Read(string db_path)
        {
            List<Session> sessions = new List<Session>();

            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
                sessions = conn.Table<Session>().ToList();
            }

            return sessions;
        }

        // Given a session ID (primary key in database), retrieve and return the relevant session object
        public static Session GetSesionById(string db_path, int id)
        {
            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
               List<Session> test = Read(db_path);

               Session session = conn.Get<Session>(id);
               return session;
            }
        }

        public static Session SetDbInfo(string db_path, int id, float dBStart)
        {
            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
                Session session = conn.Get<Session>(id);

                session.dBStart = dBStart;

                conn.Update(session);
                conn.Commit();

                return session;
            }
        }

        public static Session SetSurveyAnswers(string db_path, int id, Session newValues)
        {
            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
                Session session = conn.Get<Session>(id);

                // Refresh all values because it's easier than figuring out what actually changed
                session.SurveQ1 = newValues.SurveQ1;
                session.SurveQ2 = newValues.SurveQ2;
                session.SurveQ3 = newValues.SurveQ3;
                session.SurveQ4 = newValues.SurveQ4;
                session.SurveQ5 = newValues.SurveQ5;
                session.SurveQ6 = newValues.SurveQ6;
                session.SurveQ7 = newValues.SurveQ7;

                // Update the database and commit
                conn.Update(session);
                conn.Commit();

                // Sync with AWS
                //AWSHelper.PerformSessionUpdate(session);

                return session;
            }
        }

        public static Session SetThresholdInfo(string db_path, int id, Session newValues)
        {
            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
                Session session = conn.Get<Session>(id);

                // Refresh all values because it's easier than figuring out what actually changed
                //NOTUSED:
                //session.LeftEarThreshold_125Hz = newValues.LeftEarThreshold_125Hz;
                //session.RightEarThreshold_125Hz = newValues.RightEarThreshold_125Hz;
                //session.LeftEarThreshold_250Hz = newValues.LeftEarThreshold_250Hz;
                //session.RightEarThreshold_250Hz = newValues.RightEarThreshold_250Hz;
                session.LeftEarThreshold_1000Hz = newValues.LeftEarThreshold_1000Hz;
                session.RightEarThreshold_1000Hz = newValues.RightEarThreshold_1000Hz;
                session.LeftEarThreshold_500Hz = newValues.LeftEarThreshold_500Hz;
                session.RightEarThreshold_500Hz = newValues.RightEarThreshold_500Hz;
                session.LeftEarThreshold_2000Hz = newValues.LeftEarThreshold_2000Hz;
                session.RightEarThreshold_2000Hz = newValues.RightEarThreshold_2000Hz;
                session.LeftEarThreshold_4000Hz = newValues.LeftEarThreshold_4000Hz;
                session.RightEarThreshold_4000Hz = newValues.RightEarThreshold_4000Hz;
                //session.LeftEarThreshold_8000Hz = newValues.LeftEarThreshold_8000Hz;
                //session.RightEarThreshold_8000Hz = newValues.RightEarThreshold_8000Hz;

                // Update the database and commit
                conn.Update(session);
                conn.Commit();

                return session;
            }
        }

        public static Session SetNoSaleInfo(string db_path, int id, bool exitNoSale)
        {
            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
                Session session = conn.Get<Session>(id);

                session.ExitNoSale = exitNoSale;

                conn.Update(session);
                conn.Commit();

                return session;
            }
        }

        public static Session SetSKUInfo(string db_path, int id, string SKU)
        {
            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
                Session session = conn.Get<Session>(id);

                session.SKUChosen = SKU;

                conn.Update(session);
                conn.Commit();

                return session;
            }
        }

        public static Session SetPhoneNumber(string db_path, int id, string phoneNumber)
        {
            using (var conn = new SQLite.SQLiteConnection(db_path))
            {
                Session session = conn.Get<Session>(id);

                session.PhoneNumber = phoneNumber;

                conn.Update(session);
                conn.Commit();

                return session;
            }
        }

    }

}
