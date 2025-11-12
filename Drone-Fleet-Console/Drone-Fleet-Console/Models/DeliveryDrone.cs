using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneFleetConsole.Models.Interfaces;

namespace DroneFleetConsole.Models
{
    public class DeliveryDrone : Drone, ICargoCarrier, INavigable
    {
        public double CapacityKg { get; init; }
        public double CurrentLoadKg { get; private set; }
        public Coordinates? CurrentWaypoint { get; private set; }

        public DeliveryDrone(double capacityKg) : base()
        {
            Name = "Delivery Drone" + DroneId;
            CapacityKg = capacityKg;
        }

        public bool IsLoadValid(double kg, out string? message)
        {
            message = null;
            if (kg <= 0 || kg + CurrentLoadKg > CapacityKg)
            {
                message = "Cannot load cargo: exceeds capacity or invalid weight.";
                return false;
            }
            CurrentLoadKg += kg;
            message = $"Loaded {kg} kg. Current load: {CurrentLoadKg} kg.";
            BatteryPercentage -= 15; 
            return true;
        }

        public void UnloadAll(out string? message)
        {
            message = null;
            CurrentLoadKg = 0;
            message="All cargo unloaded. Current load: 0 kg.";
            BatteryPercentage -= 15;
        }

        public void SetWaypoint(Coordinates coordinates)
        {
            CurrentWaypoint = coordinates;
        }
    }
}
