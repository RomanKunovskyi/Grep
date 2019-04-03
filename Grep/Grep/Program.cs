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

        private void RecursiveSearch(object directory)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Convert.ToString(@directory));
                foreach (var d in dir.GetDirectories())
                {

                    Thread DirThread = new Thread(RecursiveSearch) { IsBackground = true };
                    DirThread.Start(directory + "\\" + d);

                }
                foreach (var f in dir.GetFiles())
                {

                    string path = f.DirectoryName + "\\" + f;
                    string[] type = Convert.ToString(f).Split('.');
                    try
                    {
                        Thread FileThread;
                        switch (type[type.Length - 1])
                        {
                            case "doc":
                                FileThread = new Thread(SearchInFile_docx) { IsBackground = true };
                                FileThread.Start(path);
                                break;
                            case "docx":
                                FileThread = new Thread(SearchInFile_docx) { IsBackground = true };
                                FileThread.Start(path);
                                break;
                            case "txt":
                                FileThread = new Thread(SearchInFile_txt_html) { IsBackground = true };
                                FileThread.Start(path);
                                break;
                            case "html":
                                FileThread = new Thread(SearchInFile_txt_html) { IsBackground = true };
                                FileThread.Start(path);
                                break;
                            case "dat":
                                FileThread = new Thread(SearchInFile_dat) { IsBackground = true };
                                FileThread.Start(path);
                                break;
                        }

                    }
                    catch (Exception e)
                    {

                    }

                }
            }
            catch
            {

            }


        }

        private void SearchInFile_txt_html(object path)
        {
            string text = "";
            string str;
            try
            {
                using (StreamReader reader = File.OpenText(Convert.ToString(path)))
                {
                    while ((str = reader.ReadLine()) != null)
                    {
                        text += str;
                    }
                }
                SetWayOfGrapping(SearchingString, text, path);
            }
            catch
            {

            }


        }
        private void SearchInFile_docx(object path)
        {
            string text = "";
            try
            {
                Application application = new Application();
                Document document = application.Documents.Open(path);
                for (int i = 1; i <= document.Words.Count; i++)
                {
                    text += document.Words[i].Text;
                }
                application.Quit();
                SetWayOfGrapping(SearchingString, text, path);
            }
            catch { }

        }
        private void SearchInFile_dat(object path)
        {
            string text = "";
            string str;
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(Convert.ToString(path), FileMode.Open)))
                {
                    while ((str = reader.ReadString()) != "")
                    {
                        text += str;
                    }
                }
                SetWayOfGrapping(SearchingString, text, path);
            }
            catch { }
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
