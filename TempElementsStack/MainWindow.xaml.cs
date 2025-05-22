using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TempElementsLib;

namespace WpfHistoryDemo
{
    public partial class MainWindow : Window
    {
        private Stack<TempTxtFile> history = new Stack<TempTxtFile>();
        private List<(TempTxtFile File, string Description)> historyListItems = new List<(TempTxtFile, string)>();

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                TextEditor.Text = "Initial text";
                var initialFile = new TempTxtFile();
                initialFile.Write(TextEditor.Text);
                history.Push(initialFile);
                AddHistoryEntry("Initial state");
                Console.WriteLine("Initialization: Initial state added, stack contains " + history.Count + " items.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialization error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveState_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextEditor.Text)) return;

            try
            {
                var tempFile = new TempTxtFile();
                tempFile.Write(TextEditor.Text);
                history.Push(tempFile);
                AddHistoryEntry($"Saved at {DateTime.Now.ToString("HH:mm:ss")}");
                MessageBox.Show("State saved.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Console.WriteLine("Saved: Stack contains " + history.Count + " items.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count == 0)
            {
                MessageBox.Show("No saved states to undo.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var lastState = history.Pop();
                TextEditor.Text = lastState.ReadAllText();
                lastState.Dispose();

                if (historyListItems.Count > 0)
                {
                    historyListItems.RemoveAt(historyListItems.Count - 1);
                    HistoryList.ItemsSource = null;
                    HistoryList.ItemsSource = historyListItems;
                }
                Console.WriteLine("Undo: Stack contains " + history.Count + " items.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Undo error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddHistoryEntry(string description)
        {
            try
            {
                if (history.Count > 0)
                {
                    historyListItems.Add((history.Peek(), description));
                }
                else
                {
                    historyListItems.Add((null, description));
                }
                HistoryList.ItemsSource = null;
                HistoryList.ItemsSource = historyListItems;
                Console.WriteLine("History entry added: " + description + ", total entries: " + historyListItems.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show("History addition error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HistoryList.SelectedItem == null) return;

            try
            {
                var selectedItem = (ValueTuple<TempTxtFile, string>)HistoryList.SelectedItem;
                if (selectedItem.Item1 != null)
                {
                    TextEditor.Text = selectedItem.Item1.ReadAllText();
                }
                else
                {
                    TextEditor.Text = "Initial text";
                }

                while (history.Count > 0 && history.Peek() != selectedItem.Item1)
                {
                    var state = history.Pop();
                    state.Dispose();
                    if (historyListItems.Count > 0)
                    {
                        historyListItems.RemoveAt(historyListItems.Count - 1);
                    }
                }

                HistoryList.ItemsSource = null;
                HistoryList.ItemsSource = historyListItems;
                Console.WriteLine("State selected: " + selectedItem.Item2 + ", stack contains " + history.Count + " items.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Selection error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
