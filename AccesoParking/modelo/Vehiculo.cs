using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoParking.modelo
{
    class Vehiculo : ObservableObject
    {
        public Vehiculo()
        {
            matricula = "";
            modelo = "";
            tipo = "";
            foto = "";
        }

        public Vehiculo(int idVehiculo, int idCliente, string matricula, string foto, int idMarca, string modelo, string tipo)
        {
            IdVehiculo = idVehiculo;
            IdCliente = idCliente;
            Matricula = matricula;
            Foto = foto;
            IdMarca = idMarca;
            Modelo = modelo;
            Tipo = tipo;
        }

        // Al insertar un Vehiculo en la BBDD, no se tiene en cuenta la ID del objeto
        // La ID es Autoincremental
        private int idVehiculo;
        public int IdVehiculo
        {
            get { return idVehiculo; }
            set { SetProperty(ref idVehiculo, value); }
        }

        private int idCliente;
        public int IdCliente
        {
            get { return idCliente; }
            set { SetProperty(ref idCliente, value); }
        }

        private string matricula;
        public string Matricula
        {
            get { return matricula; }
            set { SetProperty(ref matricula, value); }
        }

        private string foto;
        public string Foto
        {
            get { return foto; }
            set { SetProperty(ref foto, value); }
        }

        private int idMarca;
        public int IdMarca
        {
            get { return idMarca; }
            set { SetProperty(ref idMarca, value); }
        }

        private string modelo;
        public string Modelo
        {
            get { return modelo; }
            set { SetProperty(ref modelo, value); }
        }

        private string tipo;

        public string Tipo
        {
            get { return tipo; }
            set { SetProperty(ref tipo, value); }
        }

        public override string ToString()
        {
            return "Vehiculo{" + "matricula=" + matricula + ", documentoCliente=" + idCliente
                + ", idMarca=" + idMarca + ", modelo=" + modelo + ", tipo=" + tipo + '}';
        }
    }
}
