using System;
using System.Collections.Generic;
using System.Linq;

namespace RideSharing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test Case 1");
            Rider rider = new Rider("Bob");
            Driver driver = new Driver("Mark");

            rider.CreateRide(1, 50, 60, 1);
            Console.WriteLine(rider.CloseRide(1));

            rider.UpdateRide(1, 50, 60, 2);
            Console.WriteLine(rider.CloseRide(1));

            Console.WriteLine("Test Case 2");
            rider.CreateRide(2, 50, 60, 1);

            rider.UpdateRide(2, 50, 60, 2);
            Console.WriteLine(rider.CloseRide(2));
        }
    }

    public class Person
    {
        protected string Name { get; set; }
    }

    class Driver : Person
    {
        public Driver(string name)
        {
            this.Name = name;
        }
    }

    public class Rider : Person
    {
        List<Ride> allRides;
        public Rider(string name)
        {
            this.Name = name;
            allRides = new List<Ride>();
        }

        public void CreateRide(int id, int origin, int destination, int seats)
        {

            if (origin >= destination)
            {
                Console.WriteLine("Wrong values of Origin and Destination. Cannot create ride");
            }
            Ride currentRide = new Ride();
            currentRide.Origin = origin;
            currentRide.Destination = destination;
            currentRide.Id = id;
            currentRide.Seats = seats;
            currentRide.RideStatus = RideStatus.CREATED;
            allRides.Add(currentRide);
        }

        public void UpdateRide(int id, int origin, int destination, int seats)
        {
            int index = allRides.FindIndex(x => x.Id == id);
            
            if(index < 0)
                Console.WriteLine("Cannot update ride. Ride not in progress");

            Ride currentRide = allRides[index];

            if (currentRide != null && currentRide.RideStatus  != RideStatus.CREATED)
            {
                Console.WriteLine("Cannot update ride. Ride not in progress");
                return;
            }

            currentRide.Id = id;
            currentRide.Origin = origin;
            currentRide.Destination = destination;
            currentRide.Seats = seats;

            allRides[index] = currentRide;
        }

        public void WithdrawRide(int id)
        {
            int index = allRides.FindIndex(x => x.Id == id);
            
            if (index < 0)
                Console.WriteLine("Cannot withdraw ride. Ride not in progress"); 
            
            Ride currentRide = allRides[index];

            if (currentRide != null && currentRide.RideStatus != RideStatus.CREATED)
            {
                Console.WriteLine("Cannot withdraw ride. Ride not in progress");
                return;
            }

            allRides.RemoveAt(index);
        }

        public int CloseRide(int id)
        {
            int index = allRides.FindIndex(x => x.Id == id);

            if (index < 0)
            {
                Console.WriteLine("Cannot close ride. Ride not in progress");
                return 0;
            }

            Ride currentRide = allRides[index];

            if (currentRide != null && currentRide.RideStatus != RideStatus.CREATED)
            {
                Console.WriteLine("Cannot close ride. Ride not in progress");
                return 0;
            }

            currentRide.RideStatus = RideStatus.COMPLETED;
            allRides[index] = currentRide;
            return currentRide.CalculateFare(allRides.Count(x => x.RideStatus == RideStatus.COMPLETED) >= 100); 
        }
    }

    class Ride
    {
        private static readonly int AMT_PER_KM = 20;
        public int Id { get; set; }
        public int Origin { get; set; }
        public int Destination { get; set; }
        public int Seats { get; set; }
        public RideStatus RideStatus { get; set; }

        public Ride()
        {
            Id = Origin = Destination = Seats = 0;
            RideStatus = RideStatus.IDLE;
        }

        public int CalculateFare(bool isPriorityRider)
        {
            int dist = Destination = Origin;

            if (Seats < 2)
            {
                decimal rate = isPriorityRider ? 0.75M : 1.0M;
                return (int)(dist * AMT_PER_KM * rate);
            }
            else
            {
                decimal rate = isPriorityRider ? 0.5M : 0.75M;
                return (int)(dist * AMT_PER_KM * rate);
            }
        }

    }

    public enum RideStatus
    {
        IDLE, CREATED, WITHDRAWN, COMPLETED
    }
}
