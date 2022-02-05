using System;
using System.Collections.Generic;

namespace SalesManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int> foodSupply = new Dictionary<int, int>();
            Dictionary<int, int> beverageSupply = new Dictionary<int, int>();

            for (int i = 0; i < 3; i++)
            {
                foodSupply.Add(i, i + 1);
                beverageSupply.Add(i, i + 1);  
            }

            foodSupply.Add(3, 4);

            Store store1 = new Store(foodSupply, beverageSupply);
            City city1 = new City(foodSupply, beverageSupply);
            city1.AddStore(store1);

            State state1 = new State();
            state1.AddCity(city1);

            System system = new System();
            system.AddState(state1);

            system.PurchaseFood("State1", "City1", "Store1", FoodItems.Burger, 2);

            var store = system.States.Find(x => x.Id == "State1").Cities.Find(y => y.Id == "City1").Stores.Find(z => z.Id == "Store1");

            foreach (var unit in store.FoodUnitsSold)
            {
                Console.WriteLine($"{unit.Key} {unit.Value}");
            }

            system.PurchaseFood("State1", "City1", "Store1", FoodItems.Burger, 2);
            system.PurchaseFood("State1", "City1", "Store1", FoodItems.Burger, 1);
        }
    }

    enum FoodItems { Sandwich, Poha, Vada, Burger};
    enum BeverageItems { Tea, Coffee, Water};
    class Store
    {
        public Dictionary<int,int> FoodSupply { get; set; }
        public Dictionary<int, int> BeverageSupply { get; set; }
        public Dictionary<int, int> FoodUnitsSold { get; set; }
        public Dictionary<int, int> BeverageUnitsSold { get; set; }
        public Dictionary<int, int> FoodRates { get; set; }
        public Dictionary<int, int> BeverageRates { get; set; }
        public string Id { get; set; }
        private static int id = 1;
        public Store(Dictionary<int,int> foods, Dictionary<int, int> beverages)
        {
            Id = getUniqueId();
            
            FoodSupply = new Dictionary<int, int>();
            BeverageSupply = new Dictionary<int, int>();
            FoodUnitsSold = new Dictionary<int, int>();
            BeverageUnitsSold = new Dictionary<int, int>();
            
            foreach (var key in foods.Keys)
            {
                FoodSupply.Add(key, foods[key]);
            }

            foreach (var key in beverages.Keys)
            {
                BeverageSupply.Add(key, beverages[key]);
            }
        }

        private string getUniqueId()
        {
            return "Store" + id++;
        }

        internal void PurchaseFood(FoodItems foodItem, int quantity)
        {
            int fi = (int)foodItem;
            if(FoodSupply[fi] < quantity)
            {
                Console.WriteLine("Not enough stock");
            }
            else
            {
                Console.WriteLine("Purchasing...");
                int soldUnits;
                FoodUnitsSold.TryGetValue(fi, out soldUnits);

                if(soldUnits == 0)
                {
                    FoodUnitsSold.Add(fi, soldUnits);
                }

                Console.WriteLine($"Before purchase, food units sold in Store {Id}: {FoodUnitsSold[fi]}");
                FoodSupply[fi] -= quantity;
                
                FoodUnitsSold[fi] += quantity;
                Console.WriteLine($"After purchase, food units sold in Store {Id}: {FoodUnitsSold[fi]}");
            }
        }
        internal void PurchaseBeverage(BeverageItems beverageItem, int quantity)
        {
            int bi = (int)beverageItem;
            if (BeverageSupply[bi] < quantity)
            {
                Console.WriteLine("Not enough stock");
            }
            else
            {
                int soldUnits;
                BeverageUnitsSold.TryGetValue(bi, out soldUnits);

                if (soldUnits == 0)
                {
                    BeverageUnitsSold.Add(bi, soldUnits);
                }

                BeverageSupply[bi] -= quantity;
                BeverageUnitsSold[bi] += quantity;
            }
        }
    }
    class City
    {
        public static int id = 1;
        public string Id { get; set; }
        public Dictionary<int,int> FoodPrices { get; set; }
        public Dictionary<int,int> BeveragePrices { get; set; }
        public List<Store> Stores { get; set; }
        public City(Dictionary<int,int> foodPrices, Dictionary<int,int> beveragePrices)
        {
            Id = getUniqueId();

            FoodPrices = new Dictionary<int, int>();
            BeveragePrices = new Dictionary<int, int>();
            Stores = new List<Store>();

            foreach (var key in foodPrices.Keys)
            {
                FoodPrices.Add(key, foodPrices[key]);
            }

            foreach (var key in beveragePrices.Keys)
            {
                BeveragePrices.Add(key, beveragePrices[key]);
            }
        }

        private string getUniqueId()
        {
            return "City" + id++;
        }

        internal void AddStore(Store store)
        {
            store.FoodRates = FoodPrices;
            store.BeverageRates = BeveragePrices;
            Stores.Add(store);
        }
        
        internal void PurchaseFood(string storeId, FoodItems foodItem, int quantity)
        {
            var store = Stores.Find(x => x.Id == storeId);
            if(store != null)
            {
                store.PurchaseFood(foodItem, quantity);
            }
        }
        internal void PurchaseBeverage(string storeId, BeverageItems beverageItem, int quantity)
        {
            var store = Stores.Find(x => x.Id == storeId);
            if (store != null)
            {
                store.PurchaseBeverage(beverageItem, quantity);
            }
        }
    }
    class State
    {
        private static int id = 1;
        public string Id { get; set; }
        public List<City> Cities { get; set; }
        public State()
        {
            Id = getUniqueId();
            Cities = new List<City>();
        }

        internal void AddCity(City city)
        {
            Cities.Add(city);
        }
        internal void PurchaseFood(string cityId, string storeId, FoodItems foodItem, int quantity)
        {
            var city = Cities.Find(x => x.Id == cityId);
            if (city != null)
            {
                city.PurchaseFood(storeId, foodItem, quantity);
            }
        }
        internal void PurchaseBeverage(string cityId, string storeId, BeverageItems beverageItem, int quantity)
        {
            var city = Cities.Find(x => x.Id == cityId);
            if (city != null)
            {
                city.PurchaseBeverage(storeId, beverageItem, quantity);
            }
        }

        private string getUniqueId()
        {
            return "State" + id++;
        }
    }

    class System
    {
        public List<State> States { get; set; }
        public System()
        {
            States = new List<State>();
        }
        internal void AddState(State state)
        {
            States.Add(state);
        }
        internal void PurchaseFood(string stateId, string cityId, string storeId, FoodItems foodItem, int quantity)
        {
            var state = States.Find(x => x.Id == stateId);
            if (state != null)
            {
                state.PurchaseFood(cityId, storeId, foodItem, quantity);
            }
        }
        internal void PurchaseBeverage(string stateId, string cityId, string storeId, BeverageItems beverageItem, int quantity)
        {
            var state = States.Find(x => x.Id == stateId);
            if (state != null)
            {
                state.PurchaseBeverage(cityId, storeId, beverageItem, quantity);
            }
        }
    }
}
