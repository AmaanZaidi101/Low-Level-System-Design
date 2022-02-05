using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskPlanner
{
    class Program
    {
        static void Main(string[] args)
        {
            User user1 = new User("Bob");
            User user2 = new User("Mark");

            Task task1 = user1.CreateTask(TaskType.FEATURE);
            Task task11 = user1.CreateTask(TaskType.BUG);

            Task task2 = user2.CreateTask(TaskType.BUG);
            Task task22 = user2.CreateTask("This is a substract");

            Sprint sprint1 = user1.CreateSprint(22, 33, "Sprint1");
            Sprint sprint2 = user2.CreateSprint(44, 55, "Sprint2");

            Console.WriteLine(user1.ChangeStatus(task11, TaskStatus.IN_PROGRESS));

            Console.WriteLine(user1.AddToSprint(sprint1,task1));
            Console.WriteLine(user1.AddToSprint(sprint1, task11)); 
            Console.WriteLine(user1.AddToSprint(sprint2, task1));
            Console.WriteLine(user1.RemoveFromSprint(sprint1, task1));

            Console.WriteLine(user2.AddToSprint(sprint1,task1));
            Console.WriteLine(user2.RemoveFromSprint(sprint1, task2));
            Console.WriteLine(user2.AddToSprint(sprint2, task1));
            Console.WriteLine(user2.AddToSprint(sprint2, task2));

            Console.WriteLine(sprint1);

            user1.PrintAllTasks();
            user2.PrintAllTasks();

        }
    }

    public class Sprint
    {
        public string Name { get; set; }
        public int Begin { get; set; }
        public int End { get; set; }
        public List<Task> Tasks { get; set; }
        public Sprint(int Begin, int End, string Name)
        {
            this.Begin = Begin;
            this.End = End;
            this.Name = Name;
            Tasks = new List<Task>();
        }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        public override string ToString()
        {
            return $"Sprint Name: {Name}\nSprint Begins: {Begin}\nSprint Ends {End}\n";
        }
    }

    public enum TaskType
    {
         STORY, FEATURE, BUG
    }
    public enum TaskStatus
    {
        OPEN, IN_PROGRESS, RESOLVED, DELAYED, COMPLETED
    }

    public class Task
    {
        static int taskId = 1;
        public int Id { get; set; }
        public string Substract { get; set; }
        public User User { get; set; }
        public TaskType TaskType { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public Task()
        {
            Id = GetUniqueId();
            TaskStatus = TaskStatus.OPEN;
        }
        public int GetUniqueId()
        {
            return taskId++;
        }
        public override string ToString()
        {
            return $"Task Id: {Id}\nTask Type: {TaskType.ToString()}\nTask Status: {TaskStatus}\nUser: {User.Name}\n";
        }
    }

    public class User
    {
        public List<Task> Tasks { get; set; }
        public List<Sprint> Sprints { get; set; }
        public string Name { get; }

        public User(string name)
        {
            Tasks = new List<Task>();
            Sprints = new List<Sprint>();
            Name = name;
        }

        public Task CreateTask(TaskType taskType)
        {
            if(taskType == TaskType.STORY)
            {
                Console.WriteLine("Task Type of story is being created with no Substract");
            }
            Task task = new Task()
            {
                TaskType = taskType,
                User = this
            };

            Tasks.Add(task);
            return task;
        }
        public Task CreateTask(string substract)
        {
            Task task = new Task()
            {
                TaskType = TaskType.STORY,
                User = this,
                Substract = substract
            }; 

            Tasks.Add(task);
            return task;
        }

        public Sprint CreateSprint(int begin, int end, string name)
        {
            Sprint sprint = new Sprint(begin, end, name);
            Sprints.Add(sprint);
            return sprint;
        }

        public bool AddToSprint(Sprint sprint, Task task)
        {
            var s = Sprints.FirstOrDefault(x => x.Begin == sprint.Begin && x.End == sprint.End && x.Name == sprint.Name);
            
            if(s != null)
            {
                s.AddTask(task);
                return true;
            }

            return false;
        }
        public bool RemoveFromSprint(Sprint sprint, Task task)
        {
            var s = Sprints.FirstOrDefault(x => x.Begin == sprint.Begin && x.End == sprint.End && x.Name == sprint.Name);

            if (s != null)
            {
                var t = s.Tasks.FirstOrDefault(x => x.Id == task.Id);

                if(t != null)
                {
                    s.Tasks.Remove(t);
                    return true;
                }
            }
            return false;
        }  
        public void PrintAllTasks()
        {
            foreach (var task in Tasks)
            {
                Console.WriteLine(task);
            }
        }
        public bool ChangeStatus(Task task, TaskStatus taskStatus)
        {
            
             var t = Tasks.FirstOrDefault(x => x.Id == task.Id);

            if (t != null)
            {
                t.TaskStatus = taskStatus;
                return true;
            }
            
            return false;
        }
    }
}
