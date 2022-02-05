using System;
using System.Collections.Generic;
using System.Linq;

namespace BookCatalogSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Book book = new Book("HP and PS", "J.K. Rowling", "Bloomsburry", 1997, Category.FICTION, 200, 80);
            Book book1 = new Book("HP and COS", "J.K. Rowling", "Bloomsburry", 1998, Category.FICTION, 1000, 100);
            Book book2 = new Book("HP and POA", "J.K. Rowling", "Bloomsburry", 1999, Category.FICTION, 2000, 500);
            Book book3 = new Book("HP and HBP", "J.K. Rowling", "Bloomsburry", 2005, Category.FICTION, 3000, 700);
            Book book4 = new Book("The Immortals of Meluha", "Amish", "Westland", 2010, Category.MYTHOLOGY, 1500, 600);
            Book book5 = new Book("The Secret of the Nagas", "Amish", "Westland", 2011, Category.MYTHOLOGY, 2500, 400);
            Book book6 = new Book("The Oath of Vayuputras", "Amish", "Westland", 2013, Category.MYTHOLOGY, 3500, 200);
            Book book7 = new Book("Do Androids dream of Electric Sheep", "Philip K Dick", "DoubleDay", 1968, Category.SCI_FI, 30, 20);

            Catalog catalog = new Catalog();
            catalog.AddBookToCatalog(book);
            catalog.AddBookToCatalog(book1);
            catalog.AddBookToCatalog(book2);
            catalog.AddBookToCatalog(book3);
            catalog.AddBookToCatalog(book4);
            catalog.AddBookToCatalog(book5);
            catalog.AddBookToCatalog(book6);
            catalog.AddBookToCatalog(book7);

            string s = new string('*', 50);

            List<Book> list = catalog.GetMostSoldBooksByAuthor("Amish", 2);
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Name}\t{item.Count}");
            }
            Console.WriteLine(s);

            list = catalog.GetMostSoldBooksByCategory(Category.FICTION, 2);
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Name}\t{item.Count}");
            }
            Console.WriteLine(s);

            list = catalog.SearchBookByAuthor("Amish");
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Name}\t{item.Count}");
            }
            Console.WriteLine(s);

            list = catalog.SearchBookByName("Do");
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Name}\t{item.Count}");
            }
            Console.WriteLine(s);
        }
    }
    enum Category { FICTION,SCI_FI, MYSTERY,FABLE,MYTHOLOGY};
    class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int PublishYear { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public Book(string name, string author, string publisher, int publishYear, Category category, double price, int count)
        {
            Name = name;
            Author = author;
            Publisher = publisher;
            PublishYear = publishYear;
            Category = category;
            Price = price;
            Count = count;
        }
    }
    class Catalog
    {
        public List<Book> Books { get; set; }
        public Catalog()
        {
            Books = new List<Book>();
        }
        internal void AddBookToCatalog(Book book)
        {
            Books.Add(book);
        }
        internal List<Book> SearchBookByName(string prefix)
        {
            return Books.FindAll(x => x.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }
        internal List<Book> SearchBookByAuthor(string prefix)
        {
            return Books.FindAll(x => x.Author.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }
        internal List<Book> GetMostSoldBooksByAuthor(string author, int limit)
        {
            return Books.FindAll(x => x.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.Count).Take(limit).ToList();
        }

        internal List<Book> GetMostSoldBooksByCategory(Category category, int limit)
        {
            return Books.FindAll(x => x.Category == category).OrderByDescending(x => x.Count)
                .Take(limit).ToList();
        }


    }
}
