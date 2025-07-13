using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace PasswordGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PasswordGen pwGen;
        public MainWindow()
        {
            pwGen = new PasswordGen();
            InitializeComponent();
            numberOfWordsStackPanel.IsEnabled = false;
            lengthOfWordsStackPanel.IsEnabled = false;
            capitalizeFirstLetterCheckBox.IsEnabled = false;
            numberOfWordsStackPanel.IsEnabled = false;
            lengthOfWordsStackPanel.IsEnabled = false;
            manageLengthOfWordsCheckBox.IsEnabled = false;
            pwGen.minLengthOfWord = (int)lengthOfWordsIntUpDown.Minimum;
            pwGen.maxLengthOfWord = (int)lengthOfWordsIntUpDown.Maximum;
            pwGen.maxPwLength = (int)lengthOfPwIntUpDown.Maximum;
        }
        private void passwordLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is int pwLength)
                pwGen.pwLength = pwLength;
        }
        private void numberOfWords_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is int numWords)
                pwGen.numWords = numWords;
        }

        private void lengthOfWords_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is int lengthOfWords)
                pwGen.lengthOfWords = lengthOfWords;
        }

        private void lowercaseCharCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pwGen.lowercaseChars = true;
        }

        private void uppercaseCharCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pwGen.uppercaseChars = true;
        }

        private void numbersCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pwGen.numbersIncluded = true;
        }

        private void symbolsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pwGen.symbolsIncluded = true;
        }

        private void excludeSimilarCharsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pwGen.excludeSimilarChars = true;
        }

        private void excludeAmbiguousCharsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pwGen.excludeAmbigChars = true;
        }

        private void useDictionaryWordsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pwGen.useDictionaryWords = true;

            capitalizeFirstLetterCheckBox.IsEnabled = true;
            numberOfWordsStackPanel.IsEnabled = true;
            lengthOfWordsStackPanel.IsEnabled = true;
            manageLengthOfWordsCheckBox.IsEnabled = true;
            if ((bool)manageLengthOfWordsCheckBox.IsChecked)
                lengthOfWordsStackPanel.IsEnabled = true;
            else
                lengthOfWordsStackPanel.IsEnabled = false;
        }
        private void capitalizeFirstLetterCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pwGen.capitalizeFirstLetter = true;

            useDictionaryWordsCheckBox.IsEnabled = true;
        }

        private void manageLengthOfWordsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pwGen.manageLengthOfWords = true;

            lengthOfWordsStackPanel.IsEnabled = true;
        }

        private void lowercaseCharCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pwGen.lowercaseChars = false;
        }

        private void uppercaseCharCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pwGen.uppercaseChars = false;
        }

        private void numbersCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pwGen.numbersIncluded = false;
        }

        private void symbolsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pwGen.symbolsIncluded = false;
        }

        private void excludeSimilarCharsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pwGen.excludeSimilarChars = false;
        }

        private void excludeAmbiguousChars_Unchecked(object sender, RoutedEventArgs e)
        {
            pwGen.excludeAmbigChars = false;
        }

        private void useDictionaryWordsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pwGen.useDictionaryWords = false;

            capitalizeFirstLetterCheckBox.IsEnabled = false;
            numberOfWordsStackPanel.IsEnabled = false;
            lengthOfWordsStackPanel.IsEnabled = false;
            manageLengthOfWordsCheckBox.IsEnabled = false;
        }
        private void capitalizeFirstLetterCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pwGen.capitalizeFirstLetter = false;

            useDictionaryWordsCheckBox.IsEnabled = true;
        }

        private void manageLengthOfWordsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pwGen.manageLengthOfWords = false;
            lengthOfWordsStackPanel.IsEnabled = false;
        }

        private void genPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (pwGen.useDictionaryWords)
            {
                pwGen.GeneratePasswordWithDictionary();
                passwordTextBox.Text = pwGen.passwords.Last();
                return;
            }

            pwGen.GeneratePassword();

            // display latest pw in list if not null
            if (!pwGen.passwords.Any())
            {
                passwordTextBox.Text = "";
                MessageBox.Show("Please select options.");
                return;
            }

            else
                passwordTextBox.Text = pwGen.passwords.Last();
        }

        private void exportPasswords_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "";

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.CreatePrompt = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            //saveFileDialog.FilterIndex = 1; // selects default filter
            saveFileDialog.RestoreDirectory = true; // restore current directory instead of what user last browsed to

            // user selects "OK"
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;

                File.WriteAllLines(filePath, pwGen.passwords);
                MessageBox.Show("Files exported to: " + filePath);
            }
        }
    }
}
