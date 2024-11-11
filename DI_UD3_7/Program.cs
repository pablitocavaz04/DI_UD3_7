using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_UD3_7
{
    public class ProduccionEventArgs : EventArgs
    {
        public string NombreProducto { get; set; }
        public TimeSpan TiempoProduccion { get; set; }
    }

    public class ProcesoProduccion
    {
        public event EventHandler<ProduccionEventArgs> ProduccionCompletada;

        public void IniciarProduccion(string nombreProducto, TimeSpan tiempoProduccion)
        {
            Console.WriteLine($"Iniciando producción del producto: {nombreProducto}.");

            System.Threading.Thread.Sleep(tiempoProduccion);

            Console.WriteLine($"Producción completada para el producto: {nombreProducto}.");

            OnProduccionCompletada(new ProduccionEventArgs
            {
                NombreProducto = nombreProducto,
                TiempoProduccion = tiempoProduccion
            });
        }

        protected virtual void OnProduccionCompletada(ProduccionEventArgs e)
        {
            ProduccionCompletada?.Invoke(this, e);
        }
    }

    public class ServicioNotificacion
    {
        public void EnviarNotificacion(object sender, ProduccionEventArgs e)
        {
            Console.WriteLine($"[NOTIFICACIÓN] La producción del producto '{e.NombreProducto}' ha sido completada en {e.TiempoProduccion.TotalSeconds} segundos.");
        }
    }

    public class ServicioRegistroProduccion
    {
        public void RegistrarProduccion(object sender, ProduccionEventArgs e)
        {
            Console.WriteLine($"[REGISTRO] Producto: {e.NombreProducto}, Tiempo de producción: {e.TiempoProduccion.TotalSeconds} segundos. Registro guardado.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ProcesoProduccion procesoProduccion = new ProcesoProduccion();

            ServicioNotificacion servicioNotificacion = new ServicioNotificacion();
            ServicioRegistroProduccion servicioRegistro = new ServicioRegistroProduccion();

            procesoProduccion.ProduccionCompletada += servicioNotificacion.EnviarNotificacion;
            procesoProduccion.ProduccionCompletada += servicioRegistro.RegistrarProduccion;

            procesoProduccion.IniciarProduccion("Gadget Ecológico", TimeSpan.FromSeconds(5));

            Console.ReadLine();
        }
    }
}
