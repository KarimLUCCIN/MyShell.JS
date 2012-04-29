using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyShellImplementationModel shell;

        public MyShellImplementationModel Shell
        {
            get { return shell; }
            set { shell = value; }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = shell = new MyShellImplementationModel();

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            shell.RequestScrollToLastResultEventHandler += delegate
            {
                Dispatcher.Invoke((Action)delegate
                {
                    if (resultsBox.Items != null)
                    {
                        var lastIndex = resultsBox.Items.Count;
                     
                        if (lastIndex > 0)
                            resultsBox.ScrollIntoView(resultsBox.Items[lastIndex - 1]);
                    }
                });
            };

            shell.RequestCloseEventHandler += delegate
            {
                Dispatcher.Invoke((Action)delegate
                {
                    Close();
                });
            };
        }

        bool isExploringOperations = false;
        int maxOperationsHistoryCount = 50;
        List<string> previousOperations = new List<string>();
        int operationsHistoryExploreIndex = 0;

        private void commandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
            {
                if (Shell.Execute(commandBox.Text))
                {
                    previousOperations.Add(commandBox.Text);
                    while (previousOperations.Count > maxOperationsHistoryCount)
                        previousOperations.RemoveAt(0);

                    commandBox.Text = String.Empty;
                }

                e.Handled = true;
                isExploringOperations = false;

            }
            else
            {
                if (previousOperations.Count > 0)
                {
                    if (e.Key == Key.Up)
                    {
                        if (!isExploringOperations && String.IsNullOrEmpty(commandBox.Text))
                        {
                            isExploringOperations = true;
                            operationsHistoryExploreIndex = previousOperations.Count - 1;

                            commandBox.Text = previousOperations[operationsHistoryExploreIndex];
                        }
                        else if (isExploringOperations)
                        {
                            commandBox.Text = previousOperations[operationsHistoryExploreIndex = Math.Max(0, operationsHistoryExploreIndex - 1)];
                        }
                    }
                    else if (e.Key == Key.Down && isExploringOperations)
                    {
                        commandBox.Text = previousOperations[operationsHistoryExploreIndex = Math.Min(previousOperations.Count - 1, operationsHistoryExploreIndex + 1)];
                    }

                    if (e.Key != Key.Up && e.Key != Key.Down)
                        isExploringOperations = false;
                }
            }
        }
    }
}
