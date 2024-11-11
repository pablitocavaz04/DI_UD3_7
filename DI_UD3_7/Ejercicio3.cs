using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_UD3_7
{
    public class StockEventArgs : EventArgs
    {
        public string NombreProducto { get; }
        public int NivelStockActual { get; }

        public StockEventArgs(string nombreProducto, int nivelStockActual)
        {
            NombreProducto = nombreProducto;
            NivelStockActual = nivelStockActual;
        }
    }

    public class ControlStock
    {
        public event EventHandler<StockEventArgs> StockBajo;

        private int _nivelMinimo;

        public ControlStock(int nivelMinimo)
        {
            _nivelMinimo = nivelMinimo;
        }

        public void VerificarStock(string nombreProducto, int nivelStockActual)
        {
            if (nivelStockActual < _nivelMinimo)
            {
                OnStockBajo(nombreProducto, nivelStockActual);
            }
        }

        protected virtual void OnStockBajo(string nombreProducto, int nivelStockActual)
        {
            StockBajo?.Invoke(this, new StockEventArgs(nombreProducto, nivelStockActual));
        }
    }

    public class ServicioPedidoReposicion
    {
        public void OnStockBajo(object sender, StockEventArgs e)
        {
            Console.WriteLine($"Generando pedido de reposición para el producto: {e.NombreProducto} con nivel de stock actual: {e.NivelStockActual}");
        }
    }

    public class ServicioAlertaStock
    {
        public void OnStockBajo(object sender, StockEventArgs e)
        {
            Console.WriteLine($"¡ALERTA! El stock del producto {e.NombreProducto} está bajo. Nivel actual: {e.NivelStockActual}");
        }
    }

    public class Program
    {
        public static void Main()
        {
            ControlStock controlStock = new ControlStock(10);

            ServicioPedidoReposicion servicioPedido = new ServicioPedidoReposicion();
            ServicioAlertaStock servicioAlerta = new ServicioAlertaStock();

            controlStock.StockBajo += servicioPedido.OnStockBajo;
            controlStock.StockBajo += servicioAlerta.OnStockBajo;

            controlStock.VerificarStock("Producto A", 5);  
            controlStock.VerificarStock("Producto B", 15); 
        }
    }
}
