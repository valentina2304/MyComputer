﻿using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Tema1Calculator;

namespace Tema1Calculator
{
    public partial class MainWindow : Window
    {
        private readonly CalculatorViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new CalculatorViewModel();
            DataContext = _viewModel;
            AttachButtonHandlers();
            AttachProgrammerButtonHandlers();

        }

        private void AttachButtonHandlers()
        {
            foreach (UIElement element in (FindName("Grid") as Grid).Children)
            {
                if (element is Button button)
                {
                    string content = button.Content.ToString();
                    button.Click += (s, e) => HandleButtonClick(content);
                }
            }

        }


        private void AttachProgrammerButtonHandlers()
        {
            foreach (UIElement element in (FindName("ProgrammerGrid") as Grid).Children)
            {
                if (element is Grid grid)
                {
                    foreach (UIElement gridElement in grid.Children)
                    {
                        if (gridElement is Button button)
                        {
                            string content = button.Content.ToString();
                            button.Click += (s, e) => HandleProgrammerButtonClick(content);
                        }
                    }
                }
            }
        }

        private void HandleButtonClick(string content)
        {
            switch (content)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case ".":
                    _viewModel.EnterDigit(content);
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                    _viewModel.SetOperation(content);
                    break;
                case "=":
                    _viewModel.Calculate();
                    break;
                case "C":
                    _viewModel.Clear();
                    break;
                case "⌫":
                    _viewModel.Backspace();
                    break;
                case "CE":
                    _viewModel.ClearEntry();
                    break;
                case "±":
                    _viewModel.ChangeSign();
                    break;
                case "⅟x":
                    _viewModel.Reciprocal();
                    break;
                case "x²":
                    _viewModel.Square();
                    break;
                case "√x":
                    _viewModel.SquareRoot();
                    break;
                case "%":
                    _viewModel.Percentage();
                    break;

            }
        }

        private void HandleProgrammerButtonClick(string content)
        {
            if ((content.Length == 1 && "0123456789ABCDEF".Contains(content)))
            {
                _viewModel.EnterDigitProgrammer(content);
            }
            else if (content == "+" || content == "-" || content == "*" || content == "/" ||
                    content == "=" || content == "C" || content == "CE" || content == "⌫" || content == "%")
            {
                switch (content)
                {
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        _viewModel.SetOperation(content);
                        break;
                    case "=":
                        _viewModel.Calculate();
                        break;
                    case "C ":
                        _viewModel.Clear();
                        break;
                    case "CE":
                        _viewModel.ClearEntry();
                        break;
                    case "⌫":
                        _viewModel.Backspace();
                        break;
                    case "%":
                        _viewModel.Percentage();
                        break;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string digit = (e.Key - Key.D0).ToString();
                _viewModel.EnterDigit(digit);
                e.Handled = true;
                return;
            }

            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                string digit = (e.Key - Key.NumPad0).ToString();
                _viewModel.EnterDigit(digit);
                e.Handled = true;
                return;
            }

           if(e.Key== Key.A)
            {
                _viewModel.EnterDigit("A");
                e.Handled = true;
                return;
            }

            if (e.Key == Key.B)
            {
                _viewModel.EnterDigit("B");
                e.Handled = true;
                return;
            }

            if (e.Key == Key.C)
            {
                _viewModel.EnterDigit("C");
                e.Handled = true;
                return;
            }

            if (e.Key == Key.D)
            {
                _viewModel.EnterDigit("D");
                e.Handled = true;
                return;
            }

            if (e.Key == Key.E)
            {
                _viewModel.EnterDigit("E");
                e.Handled = true;
                return;
            }

            if (e.Key == Key.F)
            {
                _viewModel.EnterDigit("F");
                e.Handled = true;
                return;
            }

                switch (e.Key)
            {
                case Key.Decimal:
                case Key.OemPeriod:
                case Key.OemComma:
                    _viewModel.EnterDigit(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    break;
                case Key.Add:
                    _viewModel.SetOperation("+");
                    break;
                case Key.Subtract:
                    _viewModel.SetOperation("-");
                    break;
                case Key.Multiply:
                    _viewModel.SetOperation("*");
                    break;
                case Key.Divide:
                    _viewModel.SetOperation("/");
                    break;
                case Key.Back:
                    _viewModel.Backspace();
                    break;
                case Key.Enter:
                    _viewModel.Calculate();
                    break;
                case Key.Escape:
                    _viewModel.Clear();
                    break;
            }

            e.Handled = true;
        }


        // CLIPBOARD

        private void CutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string text = _viewModel.DisplayText.ToString();
            Clipboard.SetText(text);
            _viewModel.ClearEntry();
        }

        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string text = _viewModel.DisplayText.ToString();
            Clipboard.SetText(text);
        }

        private void PasteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string text = Clipboard.GetText();

                _viewModel.ClearEntry();

                bool hasDecimal = false;
                foreach (char c in text)
                {
                    if (char.IsDigit(c))
                    {
                        _viewModel.EnterDigit(c.ToString());
                    }
                    else if (c == '.' && !hasDecimal)
                    {
                        _viewModel.EnterDigit(".");
                        hasDecimal = true;
                    }
                }
            }
        }


        // MEMORY
        private void MemoryList_Click(object sender, RoutedEventArgs e)
        {
            var memoryValues = _viewModel.GetMemoryList();
            if (memoryValues.Count == 0)
            {
                MessageBox.Show("No values stored in memory.", "Memory", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ContextMenu contextMenu = new ContextMenu();
            foreach (var value in memoryValues)
            {
                MenuItem item = new MenuItem();
                item.Header = value.ToString();
                item.Tag = value;
                item.Click += MemoryItem_Click;
                contextMenu.Items.Add(item);
            }

            Button button = sender as Button;
            contextMenu.PlacementTarget = button;
            contextMenu.IsOpen = true;
        }

        private void MemoryItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            double value = (double)item.Tag;
            _viewModel.UseValueFromMemory(value);
        }

        private void MemoryClear_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.MemoryClear();
        }

        private void MemoryStore_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.MemoryStore();
        }

        private void MemoryRecall_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.MemoryRecall();
        }

        private void MemoryAdd_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.MemoryAdd();
        }

        private void MemorySub_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.MemorySubtract();
        }


        // MODE SWITCHING

        private void ModeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                _viewModel.CalculatorMode = menuItem.Header.ToString();
            }

            if (sender == StandardMenuItem)
            {
                this.Title = "Calculator - Standard";
            }
            else if (sender == ProgrammerMenuItem)
            {
                this.Title = "Calculator - Programmer";
            }
        }

        // BASE SWITCHING
        private void BaseRadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Tag != null)
            {
                int selectedBase = int.Parse(radioButton.Tag.ToString());
                _viewModel.ProgrammerBase = selectedBase;
            }
        }


        // OPTIONS
        private void DigitGrouping_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                _viewModel.DigitGroupingEnabled = menuItem.IsChecked;
            }
        }
        private void ExpressionEvaluator_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                _viewModel.UsePrecedenceEnabled = menuItem.IsChecked;
            }
        }

        // ABOUT
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Created by Lixandru Valentina-Mariana 10LF332", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
    