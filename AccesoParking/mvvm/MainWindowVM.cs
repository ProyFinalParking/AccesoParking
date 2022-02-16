using AccesoParking.modelo;
using AccesoParking.servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AccesoParking.mvvm
{
    class MainWindowVM : ObservableObject
    {
        //TODO Boton aceptar y agregar a DB

        private readonly AzureBlobStorage servicioAlmacenamiento;
        private readonly ComputerVisionService servicioMatricula;
        private readonly CustomVisionService servicioTipoVehiculo;

        private Vehiculo nuevoVehiculo;

        public Vehiculo NuevoVehiculo
        {
            get { return nuevoVehiculo; }
            set { SetProperty(ref nuevoVehiculo, value); }
        }

        public RelayCommand AceptarEstacionamientoCommand { get; }

        public MainWindowVM()
        {
            servicioAlmacenamiento = new AzureBlobStorage();
            servicioMatricula = new ComputerVisionService();
            servicioTipoVehiculo = new CustomVisionService();

            nuevoVehiculo = new Vehiculo();

            AceptarEstacionamientoCommand = new RelayCommand(AceptarCliente);
        }

        public string AbrirDialogo()
        {
            string nombreArchivo = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                nombreArchivo = openFileDialog.FileName;
            }

            return nombreArchivo;
        }

        public BitmapImage imagenPorDefecto()
        {
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri("assets/IconoEmpresa.jpg", UriKind.Relative);
            bi.EndInit();

            return bi;
        }

        public BitmapImage CargarImagen()
        {
            string UrlImagenInterna;

            UrlImagenInterna = AbrirDialogo();
            nuevoVehiculo.Foto = servicioAlmacenamiento.SubirImagen(UrlImagenInterna);
            nuevoVehiculo.Matricula = servicioMatricula.GetMatricula(nuevoVehiculo.Foto);
            nuevoVehiculo.Tipo = servicioTipoVehiculo.ComprobarVehiculo(nuevoVehiculo.Foto);

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri(UrlImagenInterna, UriKind.Absolute);
            bi.EndInit();

            return bi;
        }

        public void AceptarCliente()
        {
            int idVehiculo;

            idVehiculo = ServicioDB.GetVehicleId(nuevoVehiculo.Matricula);

            if()
        }

    }
}
