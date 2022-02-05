using System;
using System.Collections.Generic;

namespace MeetingScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Room room1 = new Room("Apollo");
            Room room2 = new Room("Orion");
            Room room3 = new Room("Neptune");

            List<Room> rooms = new List<Room>()
            {
                room1,room2,room3
            };

            Scheduler scheduler = new Scheduler(rooms);
            Console.WriteLine("Same Day\n");
            Console.WriteLine(scheduler.Book(15, 2, 5));
            Console.WriteLine(scheduler.Book(15, 5, 8));
            Console.WriteLine(scheduler.Book(15, 4, 8));
            Console.WriteLine(scheduler.Book(15, 2, 5));
            Console.WriteLine(scheduler.Book(15, 3, 6));
            Console.WriteLine(scheduler.Book(15, 7, 8));
            Console.WriteLine(scheduler.Book(15, 6, 9));

            Console.WriteLine("Different Days");
            Console.WriteLine(scheduler.Book(16, 6, 9));
            Console.WriteLine(scheduler.Book(16, 5, 8));

        }

    }

    public class Room : IRoom
    {
        public Room(string name)
        {
            Name = name;
            Calendar = new Dictionary<int,List<Meeting>>();
        }
        public string Name { get; set; }
        public Dictionary<int,List<Meeting>> Calendar { get ; set ; }

        public bool Book(int day, int start, int end)
        {
            if (Calendar.ContainsKey(day))
            {
                foreach (var m in Calendar[day])
                {
                    if (m.End > start && end > m.Start)
                        return false;
                }
                Meeting meeting = new Meeting(day, start, end, this);
                Calendar[day].Add(meeting);
            }
            else
            {
                Meeting meeting = new Meeting(day, start, end, this);
                Calendar.Add(day,new List<Meeting>(){ meeting });
            }

            
            return true;
        }
    }
    public class Meeting
    {
        public int Day { get; }
        public int Start { get; set; }
        public int End { get; set; }
        private Room room;

        public Meeting(int day, int start, int end, Room room)
        {
            Day = day;
            this.Start = start;
            this.End = end;
            this.room = room;
        }
    }

    public class Scheduler
    {
        private  List<Room> Rooms;
        public Scheduler(List<Room> rooms)
        {
            Rooms = rooms;
        }
        public string Book(int day, int start, int end)
        {
            foreach (var room in Rooms)
            {
                bool flag = room.Book(day, start, end);
                if (flag)
                    return room.Name;
            }

            return "No rooms available";
        }
    }

    interface IRoom
    {
        public string Name { get; set; }
        public Dictionary<int,List<Meeting>> Calendar { get; set; }
        bool Book(int day, int start, int end);
        
    }
}
