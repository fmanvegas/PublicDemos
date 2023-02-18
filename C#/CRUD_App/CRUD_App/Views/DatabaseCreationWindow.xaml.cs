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
using System.Windows.Shapes;

namespace CRUD_App.Views
{
    /// <summary>
    /// Interaction logic for DatabaseCreationWindow.xaml
    /// </summary>
    public partial class DatabaseCreationWindow : Window
    {
        private readonly MainViewModel MVM;
        private bool mouseIsDown = false;
        private Point mouseOffset;

        public DatabaseCreationWindow(MainViewModel mvm)
        {
            InitializeComponent();
            DataContext = MVM = mvm;

            MouseMove += new MouseEventHandler(MainWindow_MouseMove);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();            
        }
    }
}
