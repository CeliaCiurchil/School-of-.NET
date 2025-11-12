using DroneFleetConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFleetConsole.Services
{
    public class DroneFactory
    {
        public static Drone GetDrone(DroneType droneType)
        {
            switch (droneType)
            {
                case DroneType.Survey:
                    return new SurveyDrone();

                case DroneType.Delivery:
                    return CreateDeliveryDrone();
                    
                case DroneType.Racing:
                    return new RacingDrone();

                default:
                    throw new ArgumentException("Invalid drone type");
            }
        }

        private static Drone CreateDeliveryDrone()
        {
            Console.Write("Enter capacity in kg for Delivery Drone: ");
            if (!double.TryParse(Console.ReadLine(), out double capacityKg))
            {
                throw new ArgumentException("Invalid capacity. Please enter a valid number.");
            }
            return new DeliveryDrone(capacityKg);
        }
    }
}
