using System;
using System.Collections.Generic;
using System.Linq;

namespace TextEditorApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Notepad notepad = new Notepad("At the starting of the week\nAt summit talks you'll hear them speak\nIt's only Monday");
            string stars = new string('*', 50);
            int i = 0;notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Displaying firt two lines");
            notepad.Display(1, 2);
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Inserting yeah to the first line");
            notepad.Insert(1, "Yeah");
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Undoin last move");
            notepad.Undo();
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Redoin last move");
            notepad.Redo();
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Redoin last move");
            notepad.Redo();
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Deleting first line");
            notepad.Delete(1);
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Undoin last move");
            notepad.Undo();
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Undoin last move");
            notepad.Undo();
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("After deletion of lines 1 to 2");
            notepad.Delete(1, 2);
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Undoin last move");
            notepad.Undo();
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Copying line 1 to 2 and pasting them to third");
            notepad.Copy(1, 2);
            notepad.Paste(3);
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Undoin last move");
            notepad.Undo();
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
            Console.WriteLine("Redoin last move");
            notepad.Redo();
            notepad.Display();
            Console.WriteLine($"{stars}{i++}{stars}");
        }
    }
    class Notepad
    {
        public List<string> AllContent { get; set; }
        public List<string> Buffer { get; set; }
        public Stack<List<string>> UndoStack { get; set; }
        public Stack<List<string>> RedoStack { get; set; }
        public Notepad(string text)
        {
            AllContent = text.Split('\n').ToList();
            UndoStack = new Stack<List<string>>();
            RedoStack = new Stack<List<string>>();
        }
        internal void Display()
        {
            foreach (var line in AllContent)
            {
                Console.WriteLine(line);
            }
        }
        internal bool Display(int n, int m)
        {
            if (n >= AllContent.Count || n > m || m >= AllContent.Count)
                return false;
            for (int i = n-1; i <= m-1; i++)
            {
                Console.WriteLine(AllContent[i]);
            }
            return true;
        }
        internal bool Insert(int n,string text)
        {
            if (n >= AllContent.Count)
                return false;

            UndoStack.Push(AllContent.ToList());
            AllContent[n - 1] += " " + text;
            return true;
        }
        internal bool Delete(int n)
        {
            if (n > AllContent.Count)
                return false;

            UndoStack.Push(AllContent.ToList());
            AllContent.RemoveRange(0, n);
            return true;
        }
        internal bool Delete (int n,int m)
        {
            if (n >= AllContent.Count || m >= AllContent.Count)
                return false;

            UndoStack.Push(AllContent.ToList());
            AllContent.RemoveRange(n-1, m - n + 1);
            return true;
        }
        internal bool Copy(int n, int m)
        {
            if (n >= AllContent.Count || m >= AllContent.Count || n > m)
                return false;

            Buffer = new List<string>();
            for (int i = n-1; i <= m-1; i++)
            {
                Buffer.Add(AllContent[i].ToString());
            }
            return true;
        }

        internal bool Paste(int n)
        {
            if (n > AllContent.Count)
                return false;

            UndoStack.Push(AllContent.ToList());
            AllContent.InsertRange(n - 1, Buffer);
            return true;
        }

        internal bool Undo()
        {
            if(UndoStack.Count == 0)
            {
                Console.WriteLine("nothing to undo");
                return false;
            }
            RedoStack.Push(AllContent.ToList());
            AllContent = UndoStack.Pop();
            return true;
        }
        internal bool Redo()
        {
            if (RedoStack.Count == 0)
            {
                Console.WriteLine("nothing to redo");
                return false;
            }
            UndoStack.Push(AllContent.ToList());
            AllContent = RedoStack.Pop();
            return true;
        }
    }
}
