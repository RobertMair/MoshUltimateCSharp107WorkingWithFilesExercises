using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exercise_1
{
    
    // 1- Write a program that reads a text file and displays the number of words.
    // 2- Write a program that reads a text file and displays the longest word in the file.
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using (StreamWriter w = File.AppendText(filename)) { }
        }

        string filename = @"C:\Users\RobertMair\OneDrive - azienda.com.au\AAA - Coding with Mosh\The Ultimate CSharp - Part 1\07 - Working with Files\Working With Files Exercises\Exercise 1\userinputlog.txt";

        private bool fileOpen = false;

        private void Read_File(object sender, RoutedEventArgs e)
        {
            string textFileContent = ReadAFile(filename);
            UserInput.Text = textFileContent;
        }

        private async void Write_File(object sender, RoutedEventArgs e)
        {
            CloseAFile();
            UnicodeEncoding uniencoding = new UnicodeEncoding();
            byte[] result = uniencoding.GetBytes(UserInput.Text);
            using (FileStream SourceStream = File.Open(filename, FileMode.OpenOrCreate))
            {
                SourceStream.Seek(0, SeekOrigin.End);
                await SourceStream.WriteAsync(result, 0, result.Length);
            }

            if (fileOpen) OpenAFile(filename);
        }

        private void Open_File(object sender, RoutedEventArgs e)
        {
            OpenAFile(filename);
            fileOpen = true;
        }

        private void Close_File(object sender, RoutedEventArgs e)
        {
            CloseAFile();
            fileOpen = false;
        }

        private void Clear_File(object sender, RoutedEventArgs e)
        {
            File.Create(filename).Close();
            if (fileOpen) OpenAFile(filename);
        }
        public static void OpenAFile(string filename)
        {
            var notepad = Process.Start("notepad.exe", filename);
        }
        
        private void Clear_Text_Box(object sender, RoutedEventArgs e)
        {
            UserInput.Clear();
        }

        private void Count_Words_In_File(object sender, RoutedEventArgs e)
        {
            string[] words = TextFileToArray(filename);
            int count = words.Count();
            OutputText.Text = "Word count: " + count;
        }
        
        private void Display_Longest_Word(object sender, RoutedEventArgs e)
        {
            string[] words = TextFileToArray(filename);
            int longestLength = words.Max(w => w.Length);
            OutputText.Text = "Longest word length: " + longestLength;
        }

        public static string ReadAFile(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string readText = reader.ReadToEnd();
                return System.Text.RegularExpressions.Regex.Replace(readText, @"[^a-zA-Z0-9\s]", string.Empty);
            }
        }

        public static void CloseAFile()
        {
            Process[] localByName = Process.GetProcessesByName("notepad");
            foreach (var process in localByName)
                process.Kill();
        }

        public static string[] TextFileToArray(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string readText = reader.ReadToEnd();
                string cleanText = Regex.Replace(readText, @"[^a-zA-Z0-9\s]", string.Empty);
                cleanText = Regex.Replace(cleanText, @"\t|\n|\r", " ");
                string[] words = cleanText.Split(new char[] {'.', '?', '!', ' ', ';', ':', ','},
                    StringSplitOptions.RemoveEmptyEntries);
                return words;
            }
        }

    }
}
