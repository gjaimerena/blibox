using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blibox.Logica.Model
{
    
    public class FECAERespuesta
    {
        public FECAERespuestaCabecera Cabecera { get; set; }
        public List<FECAERespuestaDetalle> Detalles { get; set; }
        public List<Evento> Eventos { get; set; }
        public List<Error> Errores { get; set; }
    }
}
