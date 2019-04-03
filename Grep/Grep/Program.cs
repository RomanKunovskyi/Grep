﻿using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Threading;

namespace Grep
{
    class Grep
    {
        public string Directory = "1";
        private string GrepType = "";
        private readonly string SearchingString;
        private int amount = 0;

   

        private bool Verification(string[] command_arr)
        {
            foreach (string el in command_arr)
            {
                if (el == "")
                {
                    return false;
                }
            }
            return true;
        }
        private void SetGrepType(string type)
        {
            switch (type)
            {
                case "-i":
                    GrepType = "-i";
                    break;
                case "-c":
                    GrepType = "-c";
                    break;
                case "-v":
                    GrepType = "-v";
                    break;
                case "-l":
                    GrepType = "-l";
                    break;
                case "-L":
                    GrepType = "-L";
                    break;
            }
        }

        public Grep(string command)
        {


            string[] command_arr = Convert.ToString(command).Split(' ');
            string[] searching_str = Convert.ToString(command).Split('"');
            SearchingString = searching_str[1];
            int amount_of_spaces = SearchingString.Split(' ').Length;
            if (command_arr[0] == "grep")
            {

                switch (command_arr.Length - amount_of_spaces)
                {
                    case 1:

                        RecursiveSearch(Directory);
                        break;
                    case 2:

                        SetGrepType(command_arr[1]);

                        if (GrepType == "")
                        {

                            Directory = command_arr[1 + amount_of_spaces];
                            RecursiveSearch(Directory);
                        }
                        else
                        {

                            RecursiveSearch(Directory);
                        }
                        break;
                    case 3:

                        SetGrepType(command_arr[1]);

                        Directory = searching_str[2];
                        RecursiveSearch(Directory);
                        break;
                }

            }
            else
            {
                Console.WriteLine("Input error <<grep [parametr] [Searching string] [Directory]>>");
            }


        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            string str = "";
            str = Console.ReadLine();

            Grep G = new Grep(str);

            Console.ReadKey();
        }
    }
}