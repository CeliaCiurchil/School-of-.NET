using DroneFleetConsole.Models;
using DroneFleetConsole.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFleetConsole.Services
{
    public class Menu
    {
        public static void PrintOptions()
        {
            Console.WriteLine("""
                1. List drones
                2. Add drone
                3. Pre-flight check
                4. Take off / Land
                5. Set waypoint
                6. Capability actions
                7. Charge battery
                8. Exit
            """);
            Console.Write("Enter an option: ");
        }

        public static void MenuLoop()
        {
            FleetManager fleetManager = new FleetManager();
            int option = 0;

            while (option != 8)
            {
                try
                {
                    PrintOptions();

                    if (!int.TryParse(Console.ReadLine(), out option))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    if (option == 8)
                    {
                        Console.WriteLine("You chose exiting the program!");
                        break;
                    }

                    switch (option)
                    {
                        case 1:
                            {
                                ListDrones(fleetManager);
                                break;
                            }
                        case 2:
                            {
                                AddDrone(fleetManager);
                                break;
                            }
                        case 3:
                            {
                                PreFlightCheck(fleetManager);
                                break;
                            }
                        case 4:
                            {
                                TakeOffLand(fleetManager);
                                break;
                            }
                        case 5:
                            {
                                SetWaypoint(fleetManager);
                                break;
                            }
                        case 6:
                            {
                                CapabilityActions(fleetManager);
                                break;
                            }
                        case 7:
                            {
                                ChargeBattery(fleetManager);
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Invalid option");
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("Press to continue");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        private static void ChargeBattery(FleetManager fleetManager)
        {
            Console.Write("Enter drone id: ");
            if (!int.TryParse(Console.ReadLine(), out int droneId))
            {
                Console.WriteLine("Invalid drone ID. Please enter a valid number.");
                return;
            }
            Drone? drone = fleetManager.GetDroneById(droneId);

            Console.Write("Chraging drone...(Enter Percent): ");
            if (!int.TryParse(Console.ReadLine(), out int batteryPercent))
            {
                Console.WriteLine("Invalid batteryPercent. Please enter a valid number.");
                return;
            }
            drone!.Charge(batteryPercent);
        }

        private static void CapabilityActions(FleetManager fleetManager)
        {
            Console.Write("Enter drone id: ");
            if (!int.TryParse(Console.ReadLine(), out int droneId))
            {
                Console.WriteLine("Invalid drone ID. Please enter a valid number.");
                return;
            }

            Drone? drone = fleetManager.GetDroneById(droneId);

            List<string> capabilities = new();
            if (drone is ICargoCarrier) { capabilities.Add("Load"); capabilities.Add("Unload"); }
            if (drone is IPhotoCapture) capabilities.Add("Photo");

            if (capabilities.Count == 0)
            {
                Console.WriteLine("No actions for this type of drone"); return;
            }

            Console.WriteLine("Available actions: ");
            for (int i = 0; i < capabilities.Count; i++)
                Console.WriteLine($"{i + 1}. {capabilities[i]}");

            Console.Write("Select Action: ");
            if (!int.TryParse(Console.ReadLine(), out int capabilityOption) || capabilityOption < 1 || capabilityOption > capabilities.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            string selectedAction = capabilities[capabilityOption - 1];
            string? message = null;
            switch (selectedAction)
            {
                case "Load":
                    {
                        ICargoCarrier carrierDrone = (ICargoCarrier)drone!;
                        Console.Write("Enter weight to load (kg): ");
                        if (double.TryParse(Console.ReadLine(), out double loadWeight))
                        {
                            carrierDrone.IsLoadValid(loadWeight, out message);
                            Console.WriteLine(message);
                        }
                        else
                        {
                            Console.WriteLine("Invalid weight input.");
                        }
                        break;
                    }
                case "Unload":
                    {
                        ICargoCarrier carrierDrone = (ICargoCarrier)drone!;
                        carrierDrone.UnloadAll(out message);
                        Console.WriteLine(message);
                        break;
                    }
                case "Photo":
                    {
                        SurveyDrone surveyDrone = (SurveyDrone)drone!;
                        surveyDrone.TakePhoto(out message);
                        Console.WriteLine(message);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid selected action");
                        break;
                    }

            }
        }

        private static void SetWaypoint(FleetManager fleetManager)
        {
            Console.Write("Enter drone id: ");
            if (!int.TryParse(Console.ReadLine(), out int droneId))
            {
                Console.WriteLine($"Invalid type. Please enter a valid ID.");
                return;
            }
            Drone? drone = fleetManager.GetDroneById(droneId);

            if (drone is INavigable navigableDrone)
            {
                Console.Write("Enter latitude: ");
                string? latInput = Console.ReadLine();
                if (!double.TryParse(latInput, out double latitude))
                {
                    Console.WriteLine("Invalid latitude. Please enter a valid number.");
                    return;
                }

                Console.Write("Enter longitude: ");
                string? lonInput = Console.ReadLine();
                if (!double.TryParse(lonInput, out double longitude))
                {
                    Console.WriteLine("Invalid longitude. Please enter a valid number.");
                    return;
                }

                Coordinates coordinates = new Coordinates(latitude, longitude);
                navigableDrone.SetWaypoint(coordinates);
                Console.WriteLine($"Waypoint set to ({latitude}, {longitude}) for Drone ID {drone.DroneId}");
            }
            else
            {
                Console.WriteLine("This drone type does not support navigation.");
            }
        }

        private static void PreFlightCheck(FleetManager fleetManager)
        {
            fleetManager.TestDrones();
        }

        private static void ListDrones(FleetManager fleetManager)
        {
            fleetManager.DisplayDrones();
        }

        private static void AddDrone(FleetManager fleetManager)
        {
            Console.Write("Type (Survey/Delivery/Racing): ");
            string? type = Console.ReadLine();
            if (!Enum.TryParse(type, true, out DroneType droneType))
            {
                Console.WriteLine($"Invalid drone type: '{type}'. Please enter Survey, Delivery, or Racing.");
                return;
            }
            fleetManager.AddDrone(droneType);
        }

        private static void TakeOffLand(FleetManager fleetManager)
        {
            Console.Write("Enter drone id: ");
            if (!int.TryParse(Console.ReadLine(), out int droneId))
            {
                Console.WriteLine($"Invalid type. Please enter a valid ID.");
                return;
            }

            Drone? drone = fleetManager.GetDroneById(droneId);
            if (drone!.isAirborne) drone.Land();
            else drone.TakeOff();
        }
    }
}
