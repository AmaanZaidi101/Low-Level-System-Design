using System;
using System.Collections.Generic;

namespace RideSharing
{
    class RideSharing
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test Case 1");
            Rider rider = new Rider(1,"Bob");
            Driver driver = new Driver("Mark");

            rider.CreateRide(1, 50, 60, 1);
            Console.WriteLine(rider.CloseRide());

            rider.UpdateRide(1, 50, 60, 2);
            Console.WriteLine(rider.CloseRide());

            Console.WriteLine("Test Case 2");
            rider.CreateRide(1, 50, 60, 1);

            rider.UpdateRide(1, 50, 60, 2);
            Console.WriteLine(rider.CloseRide());
        }
    }

    public class Person
    {
        protected string Name { get; set; }
    }

    public class Driver: Person
    {
        public Driver(string name)
        {
            this.Name = name;
        }
    }

    public class Rider: Person
    {
        public int Id { get; set; }
        List<Ride> completedRides;
        Ride currentRide;
        public Rider(int id, string name)
        {
            this.Id = id;
            this.Name = name;
            currentRide = new Ride();
            completedRides = new List<Ride>();
        }

        public void CreateRide (int id, int origin, int destination, int seats)
        {

            if(origin >= destination)
            {
                Console.WriteLine("Wrong values of Origin and Destination. Cannot create ride");
            }
            currentRide.Origin = origin;
            currentRide.Destination = destination;
            currentRide.Id = id;
            currentRide.Seats = seats;
            currentRide.RideStatus = RideStatus.CREATED; 
        }

        public void UpdateRide(int id, int origin, int destination, int seats)
        {
            if (currentRide.RideStatus == RideStatus.WITHDRAWN)
            {
                Console.WriteLine("Cannot update rider. Ride was withdrawn");
                return;
            }
            if (currentRide.RideStatus == RideStatus.COMPLETED)
            {
                Console.WriteLine("Cannot update rider. Ride was completed");
                return;
            }

            CreateRide(id, origin, destination, seats);
        }

        public void WithdrawRide(int id)
        {
            if(currentRide.Id != id)
            {
                Console.WriteLine("Wrong id passed. Cannot withdraw ride");
                return;
            }

            if(currentRide.RideStatus != RideStatus.CREATED)
            {
                Console.WriteLine("Ride wasn't in progress. Cannot withdraw ride");
                return;
            }

            currentRide.RideStatus = RideStatus.WITHDRAWN;
        }

        public int CloseRide()
        {
            if (currentRide.RideStatus != RideStatus.CREATED)
            {
                Console.WriteLine("Ride wasn't in progress. Cannot close ride");
                return 0;
            }

            currentRide.RideStatus = RideStatus.COMPLETED;
            completedRides.Add(currentRide);
            return currentRide.CalculateFare(completedRides.Count >= 100);
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
