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

        private void commandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
            {
                if (Shell.Execute(commandBox.Text))
                {
                    commandBox.Text = String.Empty;
                }
                    
                e.Handled = true;
            }
        }
    }
}
