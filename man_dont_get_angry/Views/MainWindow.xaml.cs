using System.Windows;

namespace man_dont_get_angry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new man_dont_get_angry.ViewModels.MainWindowViewModel();
        }
    }
}
