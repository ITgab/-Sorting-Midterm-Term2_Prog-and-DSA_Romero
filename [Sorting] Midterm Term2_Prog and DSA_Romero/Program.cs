﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO; 
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace _Sorting__Midterm_Term2_Prog_and_DSA_Romero
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Please create your own sorting algorithms and not use any built in sort methods
            // sort in ascending order: rating / rating count / price
            // [0] name, [1] rating, [2] rating count, [3] price, [4] type, [5] category

            //read declarations
            int key = 0;
            string SetupFile = "";
            string Source = "";
            string line = "";
            string uInput = "";
            string[] temp = new string[] { }; 
            List<string> SetupDetails = new List<string>();
            List<string> SetupRequirements = new List<string> { "Source", "OutDir" };
            List<string> Categories = new List<string>();
            Dictionary<int, List<string>> items = new Dictionary<int, List<string>>();

            //program sorting declarations
            bool run = true;
            int sortOpt = 0;
            int maxNum = 0;
            int idxToRemove = 0;
            int number = 0;
            List<int> SortedIdx = new List<int>();

            int tempValue = 0;

            //OutDir declarations
            string OutDir = "";
            string OutFile = "";
            bool append = true;


            //read [Setup]
            Console.Write("Input SetUp File: ");
            uInput = Console.ReadLine();
            SetupFile = uInput;
            Console.WriteLine();

            if (System.IO.File.Exists(SetupFile))
            {
                Console.WriteLine("    Reading Setup File......");
                using (StreamReader sr = new StreamReader(SetupFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            temp = line.Split('=');
                            SetupDetails.Add(temp[1]);
                        }
                    }
                    for (int x = 0; x < SetupRequirements.Count(); x++)
                        Console.WriteLine("The {0} is " + SetupDetails[x], SetupRequirements[x]);
                    Source = SetupDetails[0];
                    OutDir = SetupDetails[1];
                }
                Console.WriteLine("    Finished Reading Setup File......");
            }

            Console.WriteLine();

            //read [Source]
            if (System.IO.File.Exists(Source))
            {
                Console.WriteLine("    Reading Source File......");
                using (StreamReader sr = new StreamReader(Source))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            temp = line.Split(',');
                            if (temp.Length == 6) 
                                items.Add(key, new List<string> { temp[0], temp[1], temp[2], temp[3], temp[4], temp[5] });
                            key++;
                        }
                    }
                }
                Console.WriteLine("There are {0} items in the Source File", items.Count);
                Console.WriteLine("    Finished Reading Source File......");
            }

            //category
            for (int x = 0; x < items.Count; x++)
            {
                Categories.Add(items[x][5]);
                if (Categories.Count > 1)
                    for (int y = 0; y < Categories.Count - 1; y++)
                        if (items[x][5] == Categories[y])
                            Categories.Remove(items[x][5]);
            }

            Console.WriteLine();
            Console.WriteLine("There are {0} categories", Categories.Count);
            Console.WriteLine("The program will segregate the files per category.");

            //array of lists
            List<int>[] ArrListCategories = new List<int>[Categories.Count]; 
            for (int x = 0; x < ArrListCategories.Length; x++)
                ArrListCategories[x] = new List<int>(); 

            //grouping based on category
            for (int x = 0; x < Categories.Count; x++) 
                for (int y = 0; y < items.Count; y++) 
                    if (items[y][5] == Categories[x])
                        ArrListCategories[x].Add(y); 

            //program
            while (run)
            {
                Console.WriteLine();
                Console.WriteLine("How do you want the program to sort them by? (always in ascending order)");
                Console.WriteLine("\t [a] rating");
                Console.WriteLine("\t [b] rating count");
                Console.WriteLine("\t [c] price");
                Console.WriteLine("\t [d] exit program");
                Console.Write("Please input your answer here:");
                uInput = Console.ReadLine().ToLower();
                Console.Clear();

                if (uInput == "a") 
                    sortOpt = 1;
                else if (uInput == "b") 
                    sortOpt = 2;
                else if (uInput == "c") 
                    sortOpt = 3;
                else if (uInput == "d") 
                {
                    run = false;
                    break;
                }                    
                else 
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                if (uInput == "a" || uInput == "b" || uInput == "c")
                {
                    for (int x = 0; x < ArrListCategories.Count(); x++)
                    {
                        SortedIdx.Clear();
                        while (ArrListCategories[x].Count > 0)
                        {
                            maxNum = 1000000;
                            idxToRemove = 0;
                            for (int y = 0; y < ArrListCategories[x].Count; y++) 
                            {
                                key = ArrListCategories[x][y]; 
                                number = int.Parse(items[key][sortOpt]); 
                                if (number <= maxNum)
                                {
                                    maxNum = number;
                                    idxToRemove = y;
                                }
                                if (maxNum == 0)
                                {
                                    break;
                                }
                            }
                            SortedIdx.Add(key);
                            ArrListCategories[x].RemoveAt(idxToRemove);
                        }
                        if (ArrListCategories[x].Count == 0)
                            Console.WriteLine(Categories[x] + " category finished sorting...!");

                        for (int i = 0; i < SortedIdx.Count; i++)
                        {
                            foreach (KeyValuePair<int, List<string>> kvp in items)
                            {
                                OutFile = OutDir + Categories[x] + ".csv";
                                using (StreamWriter sw = new StreamWriter(OutFile, append))
                                {
                                    if (SortedIdx[i] == kvp.Key)
                                    {
                                        foreach (string item in kvp.Value)
                                        {
                                            sw.Write(item + ", ");
                                        }
                                        sw.WriteLine();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
