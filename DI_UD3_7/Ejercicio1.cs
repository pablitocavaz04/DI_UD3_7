using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_UD3_7
{
    public class TransmisionEventArgs : EventArgs
    {
        public string TituloEvento { get; set; }
        public TimeSpan Duracion { get; set; }
    }

    public class ControlTransmision
    {
        public event EventHandler<TransmisionEventArgs> TransmisionFinalizada;

        public void IniciarTransmision(string tituloEvento, TimeSpan duracion)
        {
            Console.WriteLine($"Iniciando transmisión en vivo: {tituloEvento}.");

            System.Threading.Thread.Sleep(duracion);

            Console.WriteLine($"Transmisión '{tituloEvento}' completada.");

            OnTransmisionFinalizada(new TransmisionEventArgs
            {
                TituloEvento = tituloEvento,
                Duracion = duracion
            });
        }

        protected virtual void OnTransmisionFinalizada(TransmisionEventArgs e)
        {
            TransmisionFinalizada?.Invoke(this, e);
        }
    }

    public class ServicioNotificacionUsuario
    {
        public void EnviarNotificacion(object sender, TransmisionEventArgs e)
        {
            Console.WriteLine($"[NOTIFICACIÓN] La transmisión '{e.TituloEvento}' ha finalizado después de {e.Duracion.TotalMinutes} minutos.");
        }
    }

    public class ServicioRegistroEventos
    {
        public void RegistrarEvento(object sender, TransmisionEventArgs e)
        {
            Console.WriteLine($"[REGISTRO] Evento: {e.TituloEvento}, Duración: {e.Duracion.TotalMinutes} minutos. Registro guardado en el sistema.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ControlTransmision controlTransmision = new ControlTransmision();

            ServicioNotificacionUsuario servicioNotificacion = new ServicioNotificacionUsuario();
            ServicioRegistroEventos servicioRegistro = new ServicioRegistroEventos();

            controlTransmision.TransmisionFinalizada += servicioNotificacion.EnviarNotificacion;
            controlTransmision.TransmisionFinalizada += servicioRegistro.RegistrarEvento;

            controlTransmision.IniciarTransmision("Concierto de Música", TimeSpan.FromSeconds(3));

            Console.ReadLine();
        }
    }
}
