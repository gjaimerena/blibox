using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migracion
{
    class Program
    {
        static void Main(string[] args)
        {
            MigracionDB.Migracion migrar = new MigracionDB.Migracion();

           //  migrar.MigrarClientes();
          //  migrar.MigrarRubros();
            // migrar.MigrarProveedores();

          //   migrar.MigrarMarcos();
             // migrar.MigrarMateriales();
           //   migrar.MigrarVendedores();
            //migrar.MigrarMatrices();
          //   migrar.MigrarCortante();
            migrar.MigrarArticulos();

        }
    }
}
