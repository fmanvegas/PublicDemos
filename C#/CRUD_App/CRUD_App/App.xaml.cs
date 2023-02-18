using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CRUD_App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal void Application_Startup(object sender, StartupEventArgs e)
        {
            //1) The actual db
            //2) the broker service
            var dbController = new DatabaseItems.Brokers.SQLiteService();

            MainViewModel mvm = new(dbController);

            MainWindow win = new() { DataContext = mvm };

            win.Show();
        }
    }
}
