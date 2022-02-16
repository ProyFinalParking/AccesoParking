using AccesoParking.modelo;
using AccesoParking.servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Windows.Media.Imaging;

namespace AccesoParking.mvvm
{
    class MainWindowVM : ObservableObject
    {
        //TODO Boton aceptar y agregar a DB

        // Propiedad de Configuración de aplicaciones con el numero de plazas
        static readonly int plazasCoches = Properties.Settings.Default.PlazasCoches;
        static readonly int plazasMotos = Properties.Settings.Default.PlazasMotos;

        private readonly AzureBlobStorage servicioAlmacenamiento;
        private readonly ComputerVisionService servicioMatricula;
        private readonly CustomVisionService servicioTipoVehiculo;

        private Vehiculo nuevoVehiculo;
        public Vehiculo NuevoVehiculo
        {
            get { return nuevoVehiculo; }
            set { SetProperty(ref nuevoVehiculo, value); }
        }

        private Estacionamiento nuevoEstacionamiento;
        public Estacionamiento NuevoEstacionamiento
        {
            get { return nuevoEstacionamiento; }
            set { SetProperty(ref nuevoEstacionamiento, value); }
        }

        public MainWindowVM()
        {
            servicioAlmacenamiento = new AzureBlobStorage();
            servicioMatricula = new ComputerVisionService();
            servicioTipoVehiculo = new CustomVisionService();

            NuevoVehiculo = new Vehiculo();
            NuevoEstacionamiento = new Estacionamiento();
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
            NuevoVehiculo.Foto = servicioAlmacenamiento.SubirImagen(UrlImagenInterna);
            NuevoVehiculo.Matricula = servicioMatricula.GetMatricula(nuevoVehiculo.Foto);
            NuevoVehiculo.Tipo = servicioTipoVehiculo.ComprobarVehiculo(nuevoVehiculo.Foto);

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri(UrlImagenInterna, UriKind.Absolute);
            bi.EndInit();

            return bi;
        }

        public void AceptarCliente()
        {
            if (NuevoVehiculo.Matricula != "" && NuevoVehiculo.Tipo != "")
            {
                AddEstacionamiento();
            }
            else
            {
                ServicioDialogos.ErrorMensaje("No se reconoce la imagen, la matricula o el tipo de vehiculo.");
            }
        }

        private void AddEstacionamiento()
        {
            // En caso de que el coche no este registrado, devuelve ID = 0
            NuevoEstacionamiento.IdVehiculo = ServicioDB.GetVehicleId(nuevoVehiculo.Matricula);

            NuevoEstacionamiento.Matricula = NuevoVehiculo.Matricula;
            NuevoEstacionamiento.Tipo = NuevoVehiculo.Tipo;
            NuevoEstacionamiento.Entrada = DateTime.Now.ToString();

            // Comprueba que el coche (la matricula) no tenga estacionamiento activo
            // Comprueba que hayan plazas libres para el tipo del vehiculo
            if (!ServicioDB.IsActiveParkedVehicle(nuevoVehiculo.Matricula))
            {
                if (ExistenPlazasLibres(nuevoVehiculo.Tipo))
                {
                    // Se inserta el estacionamiento en la BBDD
                    ServicioDB.InsertVehicleInParking(NuevoEstacionamiento);

                    ServicioDialogos.EstacionamientoAprobado();

                    // Se resetean las propiedades
                    NuevoVehiculo = new Vehiculo();
                    NuevoEstacionamiento = new Estacionamiento();
                }
                else
                {
                    ServicioDialogos.ErrorMensaje("El Parking esta lleno");
                }
            }
            else
            {
                ServicioDialogos.ErrorMensaje("Hay un estacionamiento activo de la matricula: " + NuevoVehiculo.Matricula);
            }
        }

        private bool ExistenPlazasLibres(string tipoVehiculo)
        {
            bool ExistenPlazasLibres = false;

            switch (tipoVehiculo)
            {
                case "Coche":
                    int numCoches = ServicioDB.GetNumberParkedCars();
                    if ((plazasCoches - numCoches) > 0)
                    {
                        ExistenPlazasLibres = true;
                    }
                    break;
                case "Moto":
                    int numMotos = ServicioDB.GetNumberParkedMotorcycles();
                    if ((plazasMotos - numMotos) > 0)
                    {
                        ExistenPlazasLibres = true;
                    }
                    break;
            }

            return ExistenPlazasLibres;
        }

    }
}
