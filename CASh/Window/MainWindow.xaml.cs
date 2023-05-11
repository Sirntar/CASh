using CASh.MVVM.View;
using CASh.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CASh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TopBarGrid.AddHandler(ButtonBase.MouseLeftButtonDownEvent, new MouseButtonEventHandler(TopBar_MouseDown));
            ShortVersionSign.Text = CASh.Version.__Ver;
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxWindowView.Instance == null)
            {
                MessageBoxWindowView messageBox = new MessageBoxWindowView();
                MessageBoxWindowViewModel messageBoxVM = new("Exit message", "Are You sure, that You want to exit?");

                messageBox.DataContext = messageBoxVM;

                messageBoxVM.PropertyChanged += (object? sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                {
                    if (e.PropertyName == "Action")
                    {
                        ActionStatus? status = (sender as MessageBoxWindowViewModel)?.Action;

                        if (status != null)
                            switch (status)
                            {
                                case ActionStatus.OK:
                                    this.Close();
                                    /**
                                     * Shutdown is implicitly called by Windows Presentation Foundation (WPF) in the following situations:
                                     * - When ShutdownMode is set to OnLastWindowClose.
                                     * - When the ShutdownMode is set to OnMainWindowClose.
                                     * - When a user ends a session and the SessionEnding event is either unhandled, or handled without cancellation.
                                    **/
                                    Application.Current.Shutdown();
                                    break;
                                default:
                                    break;
                            }
                    }
                };
            }
        }

        private void MinimalizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Window.About about = new Window.About();
        }
    }
}
