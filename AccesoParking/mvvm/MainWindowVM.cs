using AccesoParking.servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
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

        string UrlImagen;

        public MainWindowVM()
        {
            servicioAlmacenamiento = new AzureBlobStorage();
            servicioMatricula = new ComputerVisionService();
            servicioTipoVehiculo = new CustomVisionService();
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
            UrlImagen = servicioAlmacenamiento.SubirImagen(UrlImagenInterna);

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri(UrlImagen, UriKind.Absolute);
            bi.EndInit();

            return bi;
        }

        public string CargarMatricula()
        {
            string matricula;

            matricula = servicioMatricula.GetMatricula(UrlImagen);

            return matricula;
        }

        public string CargarTipoVehiculo()
        {
            string tipoVehiculo;

            tipoVehiculo = servicioTipoVehiculo.ComprobarVehiculo(UrlImagen);

            return tipoVehiculo;
        }

    }
}
