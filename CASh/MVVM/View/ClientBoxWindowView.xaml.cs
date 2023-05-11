using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CASh.MVVM.View
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class ClientBoxWindowView : System.Windows.Window
    {
        public static ClientBoxWindowView? Instance = null;
         
        public ClientBoxWindowView()
        {
            if (Instance != null)
            {
                this.Close();
                Instance.Activate();
            }
            else
            {
                Instance = this;

                Closing += ClientBoxWindowView_Closing;

                InitializeComponent();
                TopBarGrid.AddHandler(ButtonBase.MouseLeftButtonDownEvent, new MouseButtonEventHandler(TopBar_MouseDown));

                this.Show();
            }
        }

        private void ClientBoxWindowView_Closing(object? sender, CancelEventArgs e)
        {
            Instance = null;
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MinimalizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClientsData_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
