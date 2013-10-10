
namespace LevelUpApi
{
    public class GeographicLocation
    {
        public GeographicLocation(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public override string ToString()
        {
            return string.Format("({0}, {1})", Latitude, Longitude);
        }
    }
}
