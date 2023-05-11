using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CASh.MVVM.View
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class MessageBoxWindowView : System.Windows.Window
    {
        public static MessageBoxWindowView? Instance = null;
         
        public MessageBoxWindowView()
        {
            if (Instance != null)
            {
                this.Close();
                Instance.Activate();
            }
            else
            {
                Instance = this;

                Closing += MessageBoxWindowView_Closing;
                Deactivated += MessageBoxWindowView_Deactivated;

                this.Topmost = true;

                InitializeComponent();
                TopBarGrid.AddHandler(ButtonBase.MouseLeftButtonDownEvent, new MouseButtonEventHandler(TopBar_MouseDown));

                this.Show();
            }
        }

        private void MessageBoxWindowView_Closing(object? sender, CancelEventArgs e)
        {
            Instance = null;
        }

        private void MessageBoxWindowView_Deactivated(object? sender, EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
