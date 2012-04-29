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
using System.Windows.Shapes;
using System.ComponentModel;
using MyShell.Application;

namespace MyShell
{
    /// <summary>
    /// Interaction logic for ShellDataWindow.xaml
    /// </summary>
    public partial class ShellDataWindow : Window, INotifyPropertyChanged, IShellDataWindow
    {
        public string Id { get; private set; }

        public ShellDataWindow(string id)
        {
            Id = id;
            InitializeComponent();
            DataContext = this;
        }

        #region IShellDataWindow Members

        string data = String.Empty;
        public string Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                RaisePropertyChanged("Data");
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
