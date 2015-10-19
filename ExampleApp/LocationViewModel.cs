namespace LevelUpExampleApp
{
    public class LocationViewModel
    {
        public LocationViewModel(LevelUp.Api.Client.Models.Responses.Location location)
        {
            string displayName = string.IsNullOrEmpty(location.Name) ? string.Empty : " : " + location.Name;

            Name = string.Format("{0}{1}", location.LocationId, displayName);

            LocationId = location.LocationId;
        }

        public string Name { get; private set; }

        public int LocationId { get; private set; }
    }
}
