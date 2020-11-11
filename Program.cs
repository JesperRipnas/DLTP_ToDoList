using System;
using System.Collections.Generic;
using System.IO;

namespace ToDoList
{
    class Activity
    {
        public string date, status, log;
        public Activity(string D, string S, string L)
        {
            date = D;
            status = S;
            log = L;
        }
    }
    class Program
    {
        private static List<Activity> ToDoLists = new List<Activity>();
        static void Main(string[] args)
        {
            bool quit = false;
            //ReadDefaultFile();
            string filePath = "";
            Console.WriteLine("ToDoList 2020");
            do
            {
                Console.WriteLine("Type 'help' to display commands");
                Console.Write("> ");
                string userInput = Console.ReadLine().ToLower();
                string[] words = userInput.Split(' ');
                switch (words[0])
                {
                    case "help":
                        HelpMenu();
                        break;
                    case "load":
                        Console.Clear();
                        ToDoLists.Clear();
                        filePath = words[1];
                        Console.WriteLine($"File loaded: {filePath}");
                        string[] fileText = File.ReadAllLines(filePath);
                        foreach (string row in fileText)
                        {
                            string[] splittedRoW = row.Split('#');
                            Activity N = new Activity(splittedRoW[0], splittedRoW[1], splittedRoW[2]);
                            ToDoLists.Add(N);
                        }
                        PrintCurrentFile();
                        break;
                    case "list":
                        PrintCurrentFile();
                        break;
                    case "add":
                        AddToList(words, userInput);
                        break;
                    case "delete":
                        RemoveFromList(words, filePath);
                        break;
                    case "move":
                        MoveActivity(words);
                        break;
                    case "save":
                        SaveToFile(filePath);
                        break;
                    case "quit":
                        Console.WriteLine("Goodbye");
                        System.Threading.Thread.Sleep(1000);
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Incorrect input");
                        break;
                }
            } while (!quit);
        }
        static void MoveActivity(string[] words)
        {
            int inputActivity = Convert.ToInt32(words[1]);
            int index = inputActivity - 1;
            int newIndexUp = index - 1;
            int newIndexDown = index + 1;
            var item = ToDoLists[index];

            if (words[2] == "up")
            {
                ToDoLists.RemoveAt(index);
                ToDoLists.Insert(newIndexUp, item);
            }
            else if (words[2] == "down")
            {
                ToDoLists.RemoveAt(index);
                ToDoLists.Insert(newIndexDown, item);
            }
            Console.WriteLine("Activity moved!, remember to save before ending program");
            PrintCurrentFile();
        }
        static void RemoveFromList(string[] words, string filePath)
        {
            int inputActivity = Convert.ToInt32(words[1]);
            int index = inputActivity - 1;
            ToDoLists.RemoveAt(index);
            PrintCurrentFile();
            Console.WriteLine("If you want to save file, type 'save'");
        }
        static void SaveToFile(string filePath)
        {
            Console.WriteLine("Are you sure you want to save (y/n)");
            string input = Console.ReadLine().ToLower();
            if (input == "y")
            {
                File.WriteAllText(filePath, string.Empty);
                foreach(Activity i in ToDoLists)
                {
                    File.AppendAllText(filePath, $"{i.date}#{i.status}#{i.log}\n");
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("File saved!\n");
                System.Threading.Thread.Sleep(1000);
                Console.ResetColor();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("No changes were made!");
            }
        }
        static void AddToList(string[] words, string userInput)
        {
            string defaultStatus = "v";
            int startIndex = 11;
            int endIndex = userInput.Length - 11;
            string message = userInput.Substring(startIndex, endIndex);
            Activity N = new Activity(words[1], defaultStatus, message);
            ToDoLists.Add(N);
            Console.WriteLine("Activity added!, remember to save before ending program");
            PrintCurrentFile();
            Console.WriteLine("If you want to save file, type 'save'");
        }
        static void PrintCurrentFile()
        {
            int rowCount = 1;
            Console.WriteLine("\nN  Datum  S Titel");
            Console.WriteLine("-------------------------------------------");
            for (int i = 0; i < ToDoLists.Count; i++)
            {
                Console.WriteLine($"{rowCount}: {ToDoLists[i].date} {ToDoLists[i].status} {ToDoLists[i].log}");
                rowCount++;
            }
            Console.WriteLine();
        }
        static void HelpMenu()
        {
            Console.WriteLine("list - Show activities in current file");
            Console.WriteLine("load [path] - Load file");
            Console.WriteLine("quit - End program");
        }
        static void ReadDefaultFile()
        {
            string DefaultfilePath = @"C:\Skola\Kod\c#\ToDoList\data\default.txt";
            string[] fileText = File.ReadAllLines(DefaultfilePath);
            foreach (string row in fileText)
            {
                string[] splittedRoW = row.Split('#');
                Activity N = new Activity(splittedRoW[0], splittedRoW[1], splittedRoW[2]);
                ToDoLists.Add(N);
            }
        }
    }
}