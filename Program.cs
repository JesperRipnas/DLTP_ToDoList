﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace ToDoList
{
    /* CLASS: Activity
     * PURPOSE: Blueprint for all objects created to be stored inside list ToDoLists
     */
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
        // Keeps list outside Main so all methods can call for it without routing the list itself into them.
        private static List<Activity> ToDoLists = new List<Activity>();
        static void Main(string[] args)
        {
            bool quit = false;
            string filePath = "";
            Console.WriteLine("ToDoList 2020");
            do
            {
                Console.WriteLine("Type 'help' to display commands");
                Console.Write("> ");
                string userInput = Console.ReadLine().ToLower();
                string[] words = userInput.Split(' ');
                // Takes input from user and calls for the correct method based on the first word
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
                        // Read each line of the file one by one and split each row into words based on the symbol #
                        // Creates a new object for each row and stores it into ToDoLists
                        string[] fileText = File.ReadAllLines(filePath);
                        foreach (string row in fileText)
                        {
                            string[] splittedRoW = row.Split('#');
                            Activity N = new Activity(splittedRoW[0], splittedRoW[1], splittedRoW[2]);
                            ToDoLists.Add(N);
                        }
                        PrintCurrentFile();
                        break;
                    case "show":
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
                    case "set":
                        SetActivityStatus(words);
                        break;
                    case "save":
                        if (words.Length == 1)
                        {
                            SaveToFile(filePath);
                        }
                        else
                        {
                            SaveNewFile(words);
                        }
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
        /* METHOD: SetActivityStatus
        * PURPOSE: Change the current status of an activity to done, on hold or work in progress. */
        static void SetActivityStatus(string[] words)
        {
            int index = Convert.ToInt32(words[1]) - 1;
            string newStatus = words[2];
            switch (newStatus)
            {
                case "done":
                    ToDoLists[index].status = "*";
                    break;
                case "wip": // Work in progress
                    ToDoLists[index].status = "W";
                    break;
                case "hold":
                    ToDoLists[index].status = "H";
                    break;
                default:
                    Console.WriteLine("Incorrect input");
                    break;
            }
            PrintCurrentFile();
        }
        /* METHOD: MoveActivity
        * PURPOSE: Change the index for a specific activity up or down (-/+) to move its position on the list */
        static void MoveActivity(string[] words)
        {
            int inputActivity = Convert.ToInt32(words[1]);
            int index = inputActivity - 1;
            var item = ToDoLists[index];

            if (words[2] == "up")
            {
                ToDoLists.RemoveAt(index);
                ToDoLists.Insert(index - 1, item);
            }
            else if (words[2] == "down")
            {
                ToDoLists.RemoveAt(index);
                ToDoLists.Insert(index + 1, item);
            }
            Console.WriteLine("Activity moved!, remember to save before ending program");
            PrintCurrentFile();
        }
        /* METHOD: RemoveFromList
        * PURPOSE: Remove acitivity from current loaded list */
        static void RemoveFromList(string[] words, string filePath)
        {
            int inputActivity = Convert.ToInt32(words[1]);
            int index = inputActivity - 1;
            ToDoLists.RemoveAt(index);
            PrintCurrentFile();
            Console.WriteLine("If you want to save file, type 'save'");
        }
        /* METHOD: SaveNewFile
        * PURPOSE: Method can create new file and output information stored in list ToDoLists
        * [WORK IN PROGRESS] Will ATM not load the new file as the current file [WORK IN PROGRESS] */
        static string SaveNewFile(string[] words)
        {
            string folderRoot = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\data\");
            string newFileName = words[1];
            string fileType = ".txt";
            string pathString = Path.Combine(folderRoot, newFileName) + fileType;
            var newFile = File.Create(pathString);
            newFile.Close();
            Console.WriteLine($"{pathString} has been created!");
            foreach (Activity i in ToDoLists)
            {
                File.AppendAllText(pathString, $"{i.date}#{i.status}#{i.log}\n");
            }
            return pathString;
        }
        /* METHOD: SaveToFile
        *  PURPOSE: Save all current information stored in ToDoLists to file */
        static void SaveToFile(string filePath)
        {
            Console.WriteLine("Are you sure you want to save (y/n)");
            string input = Console.ReadLine().ToLower();
            if (input == "y")
            {
                File.WriteAllText(filePath, string.Empty);
                foreach (Activity i in ToDoLists)
                {
                    File.AppendAllText(filePath, $"{i.date}#{i.status}#{i.log}\n");
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("File saved!\n");
                System.Threading.Thread.Sleep(1000);
                Console.ResetColor();
                Console.Clear();
                PrintCurrentFile();
            }
            else
            {
                Console.WriteLine("No changes were made!");
            }
        }
        /* METHOD: AddToList
        *  PURPOSE: Creates a new object to be displayed in the current list */
        static void AddToList(string[] words, string userInput)
        {
            int startIndex = 0;
            int endIndex = 0;
            if (userInput.Contains("--"))
            {
                startIndex = 7;
                endIndex = userInput.Length - 7;
            }
            else
            {
                startIndex = 11;
                endIndex = userInput.Length - 11;
            }
            string defaultStatus = "H";
            string message = userInput.Substring(startIndex, endIndex);
            Activity N = new Activity(words[1], defaultStatus, message);
            ToDoLists.Add(N);
            Console.WriteLine("Activity added!, remember to save before ending program");
            PrintCurrentFile();
            Console.WriteLine("If you want to save file, type 'save'");
        }
        /* METHOD: PrintCurrentFile
        *  PURPOSE: Prints out all stored information in ToDoLists */
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
            Console.WriteLine("\nH - On hold");
            Console.WriteLine("W - Work in progress");
            Console.WriteLine("* - Done\n");
        }
        /* METHOD: HelpMenu
        *  PURPOSE: Prints out a menu of working command for user */
        static void HelpMenu()
        {
            Console.WriteLine("\nshow - Show activities in current file");
            Console.WriteLine("add [date] [headline] - Add new activity to current list");
            Console.WriteLine("delete [number] - Delete activity from current list");
            Console.WriteLine("move [number] up/down - Move activity up or down in the list");
            Console.WriteLine("load [path] - Load file from specific path");
            Console.WriteLine("save - Save current list to file");
            Console.WriteLine("quit - End program\n");
        }
    }
}