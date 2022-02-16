using AccesoParking.mvvm;
using System.Windows;
using System.Windows.Input;

namespace AccesoParking
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowVM();
            this.DataContext = vm;

            MatriculaImage.Source = vm.imagenPorDefecto();
        }

        private void MatriculaImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MatriculaImage.Source = vm.CargarImagen();
        }
    }
}
