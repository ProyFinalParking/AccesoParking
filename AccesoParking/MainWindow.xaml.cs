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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // No se hace con command, para poder resetear la imagen
            if (vm.AceptarCliente())
            {
                // Se resetea la imagen en caso de que se inicie el estacionamiento
                MatriculaImage.Source = vm.imagenPorDefecto();
            }
        }
    }
}
