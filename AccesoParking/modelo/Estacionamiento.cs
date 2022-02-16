using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoParking.modelo
{
    class Estacionamiento : ObservableObject
    {
        public Estacionamiento(int idEstacionamiento, int idVehiculo, string matricula, string entrada, string salida, double importe, string tipo)
        {
            IdEstacionamiento = idEstacionamiento;
            IdVehiculo = idVehiculo;
            Matricula = matricula;
            Entrada = entrada;
            Salida = salida;
            Importe = importe;
            Tipo = tipo;
        }

        public Estacionamiento()
        {
            matricula = "";
            entrada = "";
            salida = "";
            tipo = "";
        }

        // Al insertar un Estacionamiento en la BBDD, no se tiene en cuenta la ID del objeto
        // La ID es Autoincremental
        private int idEstacionamiento;
        public int IdEstacionamiento
        {
            get { return idEstacionamiento; }
            set { SetProperty(ref idEstacionamiento, value); }
        }

        private int idVehiculo;
        public int IdVehiculo
        {
            get { return idVehiculo; }
            set { SetProperty(ref idVehiculo, value); }
        }

        private string matricula;
        public string Matricula
        {
            get { return matricula; }
            set { SetProperty(ref matricula, value); }
        }

        private string entrada;
        public string Entrada
        {
            get { return entrada; }
            set { SetProperty(ref entrada, value); }
        }

        private string salida;
        public string Salida
        {
            get { return salida; }
            set { SetProperty(ref salida, value); }
        }

        private double importe;
        public double Importe
        {
            get { return importe; }
            set { SetProperty(ref importe, value); }
        }

        private string tipo;

        public string Tipo
        {
            get { return tipo; }
            set { SetProperty(ref tipo, value); }
        }

        public override string ToString()
        {
            return "Estacionamiento{" + "idEstacionamiento=" + idEstacionamiento + ", matricula="
                + matricula + ", entrada=" + entrada + ", salida=" + salida + ", importe=" + importe + ", tipo=" + tipo + '}';
        }
    }
}
