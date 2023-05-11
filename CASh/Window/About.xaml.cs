using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CASh.Window
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : System.Windows.Window
    {
        private static About? Instance = null;
        public About()
        {
            if (Instance != null)
            {
                this.Close();
                Instance.Activate();
            }
            else
            {
                Instance = this;

                Closing += About_Closing;
                Deactivated += About_Deactivated;

                this.Topmost = true;

                InitializeComponent();
                TopBarGrid.AddHandler(ButtonBase.MouseLeftButtonDownEvent, new MouseButtonEventHandler(TopBar_MouseDown));

                Version.Text = CASh.Version.__Version;

                this.Show();
            }
        }

        private void About_Closing(object? sender, CancelEventArgs e)
        {
            Instance = null;
        }

        private void About_Deactivated(object? sender, EventArgs e)
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
