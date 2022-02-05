using RideSharing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RideSharingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Rider rider1 = new Rider(1, "Bob");
            Driver driver1 = new Driver("Mark");
            Rider rider2 = new Rider(2, "Chloe");
            Rider rider3 = new Rider(3, "Macy");

            List<Rider> riders = new List<Rider>();
            riders.Add(rider1);
            riders.Add(rider2);
            riders.Add(rider3);

            RideSharingSystem rSystem = new RideSharingSystem(3, riders);

            rSystem.CreateRide(1, 1, 50, 60, 1);
            rSystem.WithdrawRide(1, 1);
            rSystem.UpdateRide(1, 1, 50, 60, 2);
            Console.WriteLine(rSystem.CloseRide(1));

            rSystem.CreateRide(1, 2, 50, 60, 1);
            rSystem.UpdateRide(1, 2, 50, 60, 2);
            Console.WriteLine(rSystem.CloseRide(1));

            rSystem.CreateRide(2, 1, 50, 60, 1);
            rSystem.UpdateRide(2, 1, 50, 60, 2);
            Console.WriteLine(rSystem.CloseRide(2));

        }
    }

    public class RideSharingSystem
    {
        private int drivers;
        private List<Rider> riders;

        public RideSharingSystem(int drivers, List<Rider> riders)
        {
            if(drivers < 2 || riders.Count() < 2)
            {
                Console.WriteLine("Not enough riders or drivers");
            }

            this.riders = riders;
            this.drivers = drivers;
        }

        public void CreateRide(int riderId, int rideId, int origin, int destination, int seats)
        {
            if(drivers == 0)
            {
                Console.WriteLine("No drivers available");
                return;
            }

            var rider = riders.FirstOrDefault(x => x.Id == riderId);

            if(rider == null)
            {
                Console.WriteLine("Rider not found");
                return;
            }

            rider.CreateRide(rideId, origin, destination, seats);
            drivers--; 
        }

        public void UpdateRide(int riderId, int rideId, int origin, int destination, int seats)
        {
            var rider = riders.FirstOrDefault(x => x.Id == riderId);

            if (rider == null)
            {
                Console.WriteLine("Rider not found");
                return;
            }

            rider.CreateRide(rideId, origin, destination, seats);
        }

        public void WithdrawRide(int riderId, int rideId)
        {
            var rider = riders.FirstOrDefault(x => x.Id == riderId);

            if (rider == null)
            {
                Console.WriteLine("Rider not found");
                return;
            }

            rider.WithdrawRide(rideId);
        }

        public int CloseRide(int riderId) 
        {
            var rider = riders.FirstOrDefault(x => x.Id == riderId);

            if (rider == null)
            {
                Console.WriteLine("Rider not found");
                return 0;
            }
            drivers++;
            return rider.CloseRide();
        }
    }
}
