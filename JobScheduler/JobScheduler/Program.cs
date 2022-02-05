using System;
using System.Collections.Generic;
using System.Linq;

namespace JobScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Job j1 = new Job("J1", 10, 0, 10, User.ROOT);
            Job j2 = new Job("J2", 20, 0, 40, User.ADMIN);
            Job j3 = new Job("J3", 15, 2, 40, User.ROOT);
            Job j4 = new Job("J4", 30, 1, 40, User.USER);
            Job j5 = new Job("J5", 10, 2, 30, User.USER);

            Scheduler scheduler = new Scheduler();
            scheduler.Addjob(j1);
            scheduler.Addjob(j2);
            scheduler.Addjob(j3);
            scheduler.Addjob(j4);
            scheduler.Addjob(j5);

            string s = new string('*', 50);
            
            Console.WriteLine($"{s} FCFS {s}");
            var res = scheduler.GetSchedulingSequence(SchedulingAlgorithms.FCFS, 2);
            foreach (var thread in res)
            {
                foreach (var r in thread)
                {
                    Console.Write(r.Name+" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"{s} SJF {s}");
            res = scheduler.GetSchedulingSequence(SchedulingAlgorithms.SJF, 2);
            foreach (var thread in res)
            {
                foreach (var r in thread)
                {
                    Console.Write(r.Name+" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"{s} FPS {s}");
            res = scheduler.GetSchedulingSequence(SchedulingAlgorithms.FPS, 2);
            foreach (var thread in res)
            {
                foreach (var r in thread)
                {
                    Console.Write(r.Name+" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"{s} EDF {s}");
            res = scheduler.GetSchedulingSequence(SchedulingAlgorithms.EDF, 2);
            foreach (var thread in res)
            {
                foreach (var r in thread)
                {
                    Console.Write(r.Name+" ");
                }
                Console.WriteLine();
            }
        }
    }
    enum User {ROOT, ADMIN, USER };   
    class Job
    {
        
        private static int id = 1;
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public int Priority { get; set; }
        public int Deadline { get; set; }
        public User User { get; set; }
        public Job(string name, int duration, int priority, int deadline, User user)
        {
            Id = getUniqueId();
            Name = name;
            Duration = duration;
            Priority = priority;
            Deadline = deadline;
            User = user;
        }

        private int getUniqueId()
        {
            return id++;
        }

    }
    enum SchedulingAlgorithms {FCFS, SJF, FPS, EDF } //First come first serve, shortest job first, fixed priority scheduling, earliest deadline first
    class Scheduler
    {
        public List<Job> Jobs { get; set; }
        public Queue<Job> FCFSJobs { get; set; }
        public Queue<Job> SJFJobs { get; set; }
        public Queue<Job> FPSJobs { get; set; }
        public Queue<Job> EDFJobs { get; set; }
        public Scheduler()
        {
            Jobs = new List<Job>();
        }

        internal void Addjob(Job job)
        {
            Jobs.Add(job);
        }

        internal List<List<Job>> GetSchedulingSequence(SchedulingAlgorithms scheduling, int threads)
        {
            List<List<Job>> result = new List<List<Job>>();
            for (int i = 0; i < threads; i++)
            {
                result.Add(new List<Job>());
            }
            int[] threadCapacity = new int[threads];
            int[] totalTimeTaken = new int[threads];

            FCFSJobs = new Queue<Job>(Jobs.OrderBy(x => x.Id).ToList());
            SJFJobs = new Queue<Job>(Jobs.OrderBy(x => x.Duration).ThenBy(x => x.Priority).ToList());
            FPSJobs = new Queue<Job>(Jobs.OrderBy(x => x.Priority).ThenBy(x => x.User)
                        .ThenByDescending(x => x.Duration).ToList());
            EDFJobs = new Queue<Job>(Jobs.OrderBy(x => x.Deadline).ThenBy(x => x.Priority).ThenBy(x => x.Duration)
                        .ToList());

            switch (scheduling)
            {
                case SchedulingAlgorithms.FCFS:
                    while(FCFSJobs.Count > 0)
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            if(threadCapacity[i] == 0)
                            {
                                if(FCFSJobs.Count > 0)
                                {
                                    Job job = FCFSJobs.Dequeue();
                                    result[i].Add(job);
                                    threadCapacity[i] += job.Duration;
                                }
                            }
                        }
                        processThreads(threadCapacity);
                    }
                    break;
                case SchedulingAlgorithms.SJF:
                    while (SJFJobs.Count > 0)
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            if (threadCapacity[i] == 0)
                            {
                                if (SJFJobs.Count > 0)
                                {
                                    Job job = SJFJobs.Dequeue();
                                    result[i].Add(job);
                                    threadCapacity[i] += job.Duration;
                                }
                            }
                        }
                        processThreads(threadCapacity);
                    }
                    break;
                case SchedulingAlgorithms.FPS:
                    while (FPSJobs.Count > 0)
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            if (threadCapacity[i] == 0)
                            {
                                if (FPSJobs.Count > 0)
                                {
                                    Job job = FPSJobs.Dequeue();
                                    result[i].Add(job);
                                    threadCapacity[i] += job.Duration;
                                }
                            }
                        }
                        processThreads(threadCapacity);
                    }
                    break;
                case SchedulingAlgorithms.EDF:
                    while (EDFJobs.Count > 0)
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            if (threadCapacity[i] == 0)
                            {
                                if (EDFJobs.Count > 0)
                                {
                                    Job job = EDFJobs.Dequeue();
                                    totalTimeTaken[i] += job.Duration; // sum of total time for this thread for all jobs

                                    if (totalTimeTaken[i] <= job.Deadline)
                                        result[i].Add(job);
                                    else
                                        totalTimeTaken[i] -= job.Duration;

                                    threadCapacity[i] += job.Duration;
                                }
                            }
                        }
                        processThreads(threadCapacity);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        private void processThreads(int[] threadCapacity)
        {
            int min = threadCapacity.Min();
            for (int i = 0; i < threadCapacity.Length; i++)
            {
                threadCapacity[i] -= min;
            }
        }
    }
}
