using System.Windows;
using Task6.ViewModel;

namespace Task6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AllBooksViewModel();
        }
    }
}
