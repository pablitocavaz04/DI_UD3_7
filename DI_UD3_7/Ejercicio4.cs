using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DI_UD3_7
{
    public class ReservaEventArgs : EventArgs
    {
        public string NombreCliente { get; }
        public string TipoHabitacion { get; }
        public DateTime FechaEntrada { get; }
        public DateTime FechaSalida { get; }

        public ReservaEventArgs(string nombreCliente, string tipoHabitacion, DateTime fechaEntrada, DateTime fechaSalida)
        {
            NombreCliente = nombreCliente;
            TipoHabitacion = tipoHabitacion;
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
        }
    }

    public class GestorReservas
    {
        public event EventHandler<ReservaEventArgs> ReservaConfirmada;

        public void RealizarReserva(string nombreCliente, string tipoHabitacion, DateTime fechaEntrada, DateTime fechaSalida)
        {
            Console.WriteLine($"Reserva realizada: {nombreCliente} para una habitación de tipo {tipoHabitacion} desde {fechaEntrada.ToShortDateString()} hasta {fechaSalida.ToShortDateString()}");

            OnReservaConfirmada(nombreCliente, tipoHabitacion, fechaEntrada, fechaSalida);
        }

        protected virtual void OnReservaConfirmada(string nombreCliente, string tipoHabitacion, DateTime fechaEntrada, DateTime fechaSalida)
        {
            ReservaConfirmada?.Invoke(this, new ReservaEventArgs(nombreCliente, tipoHabitacion, fechaEntrada, fechaSalida));
        }
    }

    public class ServicioLimpieza
    {
        public void OnReservaConfirmada(object sender, ReservaEventArgs e)
        {
            Console.WriteLine($"Servicio de limpieza programado para la habitación de {e.NombreCliente} de tipo {e.TipoHabitacion}, a limpiar antes del {e.FechaEntrada.ToShortDateString()}");
        }
    }

    public class ServicioNotificacionCliente
    {
        public void OnReservaConfirmada(object sender, ReservaEventArgs e)
        {
            Console.WriteLine($"Notificación enviada a {e.NombreCliente}: Su reserva de habitación {e.TipoHabitacion} ha sido confirmada. Estancia desde {e.FechaEntrada.ToShortDateString()} hasta {e.FechaSalida.ToShortDateString()}");
        }
    }

    public class Program
    {
        public static void Main()
        {
            GestorReservas gestorReservas = new GestorReservas();

            ServicioLimpieza servicioLimpieza = new ServicioLimpieza();
            ServicioNotificacionCliente servicioNotificacion = new ServicioNotificacionCliente();

            gestorReservas.ReservaConfirmada += servicioLimpieza.OnReservaConfirmada;
            gestorReservas.ReservaConfirmada += servicioNotificacion.OnReservaConfirmada;

            gestorReservas.RealizarReserva("Juan Pérez", "Individual", new DateTime(2024, 12, 1), new DateTime(2024, 12, 7));
        }
    }
}
