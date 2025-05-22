//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace TempElementsStack;

///// <summary>
///// Interaction logic for MainWindow.xaml
///// </summary>
//public partial class MainWindow : Window
//{
//    public MainWindow()
//    {
//        InitializeComponent();
//    }
//}

using System;
using System.Collections.Generic;
using System.Windows;
using TempElementsLib;

namespace WpfTextEditor
{
    public partial class MainWindow : Window
    {
        private Stack<TempTxtFile> history = new Stack<TempTxtFile>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SaveState_Click(object sender, RoutedEventArgs e)
        {
            // Zachowaj aktualny stan do pliku tymczasowego
            var tempFile = new TempTxtFile();
            tempFile.Write(TextEditor.Text);
            history.Push(tempFile);
            MessageBox.Show("Stan zapisany.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count == 0)
            {
                MessageBox.Show("Brak zapisanych stanów do cofnięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var lastState = history.Pop();
            string content = lastState.ReadAllText();
            lastState.Dispose();
            TextEditor.Text = content;
        }
    }
}
