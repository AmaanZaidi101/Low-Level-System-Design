using System;
using System.Collections.Generic;
using System.Linq;

namespace Splitwise
{
    class Program
    {
        static void Main(string[] args)
        {
            User u1 = new User("Ash");
            User u2 = new User("Misty");
            User u3 = new User("Brock");
            User u4 = new User("Tracey");

            List<User> users = new List<User>()
            {
                u1,u2,u3,u4
            };

            Splitwise sp = new Splitwise();
            sp.RegisterUser(u1);
            sp.RegisterUser(u2);
            sp.RegisterUser(u3);
            sp.RegisterUser(u4);

            Expense expense1 = new Expense(u1, Split.EQUAL, users, 2000);
            sp.AddExpense(expense1);
            
            sp.PrintBalanceForAllUsers();
            Console.WriteLine();
            

            List<User> users2 = new List<User>() { u2, u3 };

            Expense expense2 = new Expense(u1, Split.EXACT, users2, 1400);
            List<double> db = new List<double>() { 500, 900 };
            expense2.ExactDistribution = db;
            sp.AddExpense(expense2);
            sp.PrintBalanceForAllUsers();
            Console.WriteLine();
            List<User> users3 = new List<User>() { u1, u2, u3, u4 };
            List<double> db2 = new List<double>() { 40, 20, 20, 20 };

            Expense expense3 = new Expense(u4, Split.PERCENT, users3, 1200);
            expense3.PercentDistribution = db2;
            sp.AddExpense(expense3);
            sp.PrintBalanceForAllUsers();
            Console.WriteLine();

            //foreach (var user in sp.Users)
            //{
            //    foreach (var u in user.UserExpenseSheet)
            //    {
            //        if (u.Value > 0)
            //            Console.WriteLine($"{user.Name} owes a total of {u.Value} to {u.Key.Name}");
            //        else
            //            Console.WriteLine($"{user.Name} gets back a total of {-1 * u.Value} from {u.Key.Name}");
            //    }
            //}
            foreach (var user in sp.Users)
            {
                foreach (var u in user.UserExpenseSheet)
                {
                    if (u.Value > 0)
                        Console.WriteLine($"{user.Name} owes a total of {u.Value} to {u.Key.Name}");
                }
            }
        }
    }

    class User
    {
        public static int id = 1;
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<User,double> UserExpenseSheet { get; set; }
        public double TotalExpenseSoFar { get; set; }
        public User(string name)
        {
            Name = name;
            Id = GetUniqueId();
            TotalExpenseSoFar = 0;
            UserExpenseSheet = new Dictionary<User, double>();
        }
        private int GetUniqueId()
        {
            return id++;
        }
        internal void AddToUserExpenseSheet(User user, double value)
        {
            if (this == user)
                return;
            TotalExpenseSoFar += value;

            if(UserExpenseSheet.ContainsKey(user))
            {
                UserExpenseSheet[user] += value;
                return;
            }

            UserExpenseSheet.Add(user, value);
        }
        internal void PrintTotalBalance()
        {
            if(TotalExpenseSoFar > 0)
                Console.WriteLine($"{this.Name} owes a total of {this.TotalExpenseSoFar}");
            else
                Console.WriteLine($"{this.Name} gets back a total of {this.TotalExpenseSoFar * -1}");
        }

    }

    class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Split SplitType { get; set; }
        public List<User> Defaulters { get; set; }
        public User Creditor { get; set; }
        public List<double> PercentDistribution { get; set; }
        public List<double> ExactDistribution { get; set; }
        public double ExactTotalAmount { get; set; }

        private static int id = 1;
        public Expense(User creditor, Split split, List<User> defaulters, double exactTotalAmount)
        {
            Creditor = creditor;
            Defaulters = defaulters;
            SplitType = split;
            ExactTotalAmount = exactTotalAmount;
            Id = GetUniqueId();
        }
        private int GetUniqueId()
        {
            return id++;
        }
    }

    enum Split { EQUAL,EXACT,PERCENT}

    class Splitwise
    {
        public List<User> Users { get; set; }
        public List<int> UserIds { get; set; }
        public Splitwise()
        {
            Users = new List<User>();
            UserIds = new List<int>();
        }
        internal void RegisterUser(User user)
        {
            if (!UserIds.Contains(user.Id))
            {
                Users.Add(user);
                UserIds.Add(user.Id);
            }
        }

        internal void AddExpense(Expense expense)
        {
            if(!VerifyUsers(expense.Creditor, expense.Defaulters))
            {
                Console.WriteLine("Can't process expense, kindly register all users");
                return;
            }

            CalculateExpenses(expense);
        }

        private bool VerifyUsers(User creditor, List<User> defaulters)
        {
            if (!Users.Contains(creditor))
                return false;

            foreach (var defaulter in defaulters)
            {
                if (!Users.Contains(defaulter))
                    return false;
            }
            return true;
        }

        internal bool CalculateExpenses(Expense expense)
        {
            List<double> amountPerHead = new List<double>();
            switch (expense.SplitType)
            {   
                case Split.EQUAL:
                    amountPerHead = divideEqually(expense.ExactTotalAmount, Users.Count);
                    for (int i = 0; i < expense.Defaulters.Count; i++)
                    {
                        expense.Defaulters.Remove(expense.Creditor);
                        expense.Creditor.AddToUserExpenseSheet(expense.Defaulters[i], (-1) * amountPerHead[i]);
                        expense.Defaulters[i].AddToUserExpenseSheet(expense.Creditor, amountPerHead[i]);
                    }
                    break;
                case Split.EXACT:
                    amountPerHead = expense.ExactDistribution;
                    if (expense.ExactTotalAmount != expense.ExactDistribution.Sum())
                    { 
                        Console.WriteLine("Amount Mismatch");
                        return false;
                    }
                    if(amountPerHead.Count != expense.Defaulters.Count)
                    {
                        Console.WriteLine("Expenses dont match number of defaulters");
                        return false;
                    }
                    for (int i = 0; i < expense.Defaulters.Count; i++)
                    {
                        expense.Creditor.AddToUserExpenseSheet(expense.Defaulters[i], (-1) * amountPerHead[i]);
                        expense.Defaulters[i].AddToUserExpenseSheet(expense.Creditor, amountPerHead[i]);
                    }
                    break;
                case Split.PERCENT:
                    amountPerHead = expense.PercentDistribution;
                    if(amountPerHead.Sum() != 100)
                    {
                        Console.WriteLine("Percentages dont add up to 100!");
                        return false;
                    }
                    for (int i = 0; i < expense.Defaulters.Count; i++)
                    {
                        double amount = Math.Round(Math.Floor(((amountPerHead[i]/100.00) * expense.ExactTotalAmount) + 0.5), 2);
                        expense.Creditor.AddToUserExpenseSheet(expense.Defaulters[i], (-1) * amount);
                        expense.Defaulters[i].AddToUserExpenseSheet(expense.Creditor, amount);
                    } 
                    break;
                default:
                    return false;
                    break;
            }
            return true;
        }

        private List<double> divideEqually(double exactTotalAmount, int count)
        {
            double[] parts = new double[count];
            double part = exactTotalAmount / count;
            for (int i = 0; i < count-1; i++)
            {
                parts[i] = part;
                exactTotalAmount -= part;
            }
            parts[count - 1] = exactTotalAmount;
            return parts.ToList();
        }

        internal void PrintBalanceForAllUsers()
        {
            foreach (var user in Users)
            {
                user.PrintTotalBalance();
            }
        }

        internal void SimplifyExpenses()
        {
            for (int i = 0; i < Users.Count; i++)
            {
                foreach(var user in Users[i].UserExpenseSheet.Keys)
                {
                    double moneyGetting = user.UserExpenseSheet[Users[i]];
                    double moneyGiving = Users[i].UserExpenseSheet[user];

                    if(moneyGiving > moneyGetting)
                    {
                        moneyGetting = 0;
                    }
                    else
                    {
                        moneyGiving = 0;
                    }

                    user.UserExpenseSheet[Users[i]] = moneyGetting;
                    Users[i].UserExpenseSheet[user] = moneyGiving;
                }
            }
        }
    }

}
