using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int wordLength;
        private bool isChecked = false;
        private int failsCount = 0;
        private DateTime inputStartTime;
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool IsChecked
        {
            get 
            { 
                return isChecked; 
            }
            set
            {
                isChecked = value;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newValue = e.NewValue;
            wordLength = Convert.ToInt32(newValue);
            DifficultyBox.Text = "Difficulty: " + wordLength;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Losung.IsReadOnly = false;
            GenerateRandomText(wordLength);
            inputStartTime = DateTime.Now;
            Losung.TextChanged += Losung_TextChanged;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            string text1 = Losung.Text;
            string text2 = MusterLosung.Text;
            failsCount =+ CountDifferences(text1, text2);
            Fails.Text = "Fails: " + failsCount;
            Losung.IsReadOnly = true;
            MusterLosung.Text = "";
            Losung.Text = "";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            string buttonName = KeyToString(e.Key);
            HighlightButtonByName(buttonName);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            string buttonName = KeyToString(e.Key);
            RestoreButtonColorByName(buttonName);
        }

        private string KeyToString(Key key)
        {
            KeyConverter keyConverter = new KeyConverter();
            string keyString = keyConverter.ConvertToString(key);
            return keyString;
        }

        private void HighlightButtonByName(string buttonName)
        {
            Button button = FindName(buttonName) as Button;
            if (button != null)
            {
                button.Tag = button.Background;

                button.Background = Brushes.LightBlue;
            }
        }

        private void RestoreButtonColorByName(string buttonName)
        {
            Button button = FindName(buttonName) as Button;
            if (button != null)
            {
                if (button.Tag is Brush originalBrush)
                {
                    button.Background = originalBrush;
                }
            }
        }

        private void GenerateRandomText(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            string randomText = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            MusterLosung.Text = randomText;
        }

        private int CountDifferences(string text1, string text2)
        {
            int differences = 0;
            for (int i = 0; i < Math.Min(text1.Length, text2.Length); i++)
            {
                if (isChecked)
                {
                    if (text1[i] != text2[i])
                    {
                        differences++;
                    }
                }
                else
                {
                    if (char.ToUpper(text1[i]) != char.ToUpper(text2[i]))
                    {
                        differences++;
                    }
                }
            }
            differences += Math.Abs(text1.Length - text2.Length);
            return differences;
        }

        private void Losung_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now - inputStartTime;
            int inputLength = Losung.Text.Length;

            if (elapsedTime.TotalSeconds > 0)
            {
                double inputSpeed = inputLength / elapsedTime.TotalSeconds;
                Speed.Text = "Speed: " + Math.Round(inputSpeed, 2) + " chars/min";
            }
        }
    }
}
