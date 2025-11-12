using DroneFleetConsole.Models.Interfaces;

namespace DroneFleetConsole.Models
{
    public class SurveyDrone : Drone, IPhotoCapture, INavigable
    {
        public int PhotoCount { get; private set; }
        public Coordinates? CurrentWaypoint { get; private set; }

        public SurveyDrone() : base()
        {
            PhotoCount = 0;
            Name = "Survey Drone " + DroneId;
        }

        public void SetWaypoint(Coordinates coordinates)
        {
            CurrentWaypoint = coordinates;
        }

        public void TakePhoto(out string? message)
        {
            message = null;
            if (!isAirborne)
            {
                message = "Cannot take photo: Drone is not airborne. Please take off!";
                return;
            }
            PhotoCount++;
            message = $"Photo taken. Total photos: {PhotoCount}";
            BatteryPercentage -= 5;
        }
    }
}
