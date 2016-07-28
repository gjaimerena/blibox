using System.Collections.Generic;

namespace Blibox.Logica.Model
{
    public class FECAERespuestaDetalle
    {

        public int Concepto { get; set; }
        public int DocTipo { get; set; }
        public long DocNro { get; set; }
        public long CbteDesde { get; set; }
        public long CbteHasta { get; set; }
        public string Resultado { get; set; }
        public string CAE { get; set; }
        public string CbteFch { get; set; }
        public string CAEFchVto { get; set; }
        public List<Observacion> Observaciones { get; set; }
    }
}