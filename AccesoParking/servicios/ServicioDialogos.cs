using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccesoParking.servicios
{
    class ServicioDialogos
    {
        protected ServicioDialogos()
        {
        }

        public static void ErrorMensaje(string mensaje)
        {
            _ = MessageBox.Show(
                mensaje,
                "¡Error!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void EstacionamientoAprobado()
        {
            _ = MessageBox.Show(
                "¡Bienvenido! \nPuede aparcar en cualquier plaza libre.",
                "Estacionamiento iniciado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
