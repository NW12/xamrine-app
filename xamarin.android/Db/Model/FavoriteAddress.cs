using SQLite;

namespace xamarin.android.Db.Model
{
    public class FavoriteAddress
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}