using DroneFleetConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFleetConsole.Services
{
    public class FleetManager
    {
        private Dictionary<int,Drone> droneFleet;

        public FleetManager()
        {
            droneFleet = new Dictionary<int,Drone>();
        }

        public void AddDrone(DroneType droneType)
        {
            Drone drone = DroneFactory.GetDrone(droneType);
            droneFleet[drone.DroneId]=drone;
            Console.WriteLine($"Added {drone.Name} with ID {drone.DroneId}");
        }

        public Drone? GetDroneById(int droneId)
        {
            if (!droneFleet.TryGetValue((droneId), out Drone? drone))
                throw new ArgumentException($"Drone with ID {droneId} not found.");
            return drone;
        }

        public void DisplayDrones()
        { 
            if (droneFleet.Count == 0)
            {
                Console.WriteLine("No drones in your fleet");
                return;
            }

            foreach (var (key,drone) in droneFleet)
            {
                drone.DisplayDrone();
            }
        }

        public void TestDrones()
        {
            if (droneFleet.Count == 0)
            {
                Console.WriteLine("No drones to test.");
                return;
            }

            foreach (var (key, drone) in droneFleet)
            {
                bool ok = drone.RunSelfTest();
                string message = ok ? "Pass" : "Fail";
                Console.WriteLine($"Drone ID {drone.DroneId} Pre-flight check: {message}");
            }
        }
    }
}
