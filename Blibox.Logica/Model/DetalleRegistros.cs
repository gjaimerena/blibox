using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blibox.Logica.Model
{
    public class CbtesAsoc
    {
        public int Tipo; //  Int(3) Código de tipo de comprobante. Consultar método FEParamGetTiposCbte. (S)
        public int PtoVta; // Int(4) Punto de venta (S)
        public long Nro; // Long(8) Numero de comprobante (S)
    }

    public class Tributos
    {
        public short Id; // Int(2) Código tributo según método FEParamGetTiposTributos S
        public string Desc; // String(80) Descripción del tributo. N
        public double BaseImp; // Double(13+2) Base imponible para la determinación del tributo S Especificaciones técnicas de Servicios Web –WSFEv1 Página 15 de 122
        public double Alic; // Double(3+2) Alícuota S
        public double Importe; // Double(13+2) Importe del tributo S
    }

    public class IVA
    {
        public int Id; // Int(2) Código de tipo de iva.Consultar método FEParamGetTiposIva S
        public double BaseImp; // Double(13+2) Base imponible para la determinación de la alícuota. S
        public double Importe; // Double (13+2) Importe S    }

    /**Opcionales: Campos auxiliares (array). Adicionales por R.G.
    Los datos opcionales sólo deberán ser incluidos si el emisor pertenece al conjunto de emisores habilitados a informar opcionales. En ese caso podrá incluir 
    el o los datos opcionales que correspondan, especificando el identificador de dato opcional de acuerdo a la situación del emisor. 
    El listado de tipos de datos opcionales se puede consultar con el método FEParamGetTiposOpcional. 
    */
    public class Opcionales
    {
        public string Id;  // String(4) Código de Opcional, consultar método FEParamGetTiposOpcional S
        public string Valor; // String(250) Valor    }

    public class DetalleRegistros
    {
        public int Concepto;
        public int DocTipo;
        public long DocNro;
        public long CbteDesde;
        public long CbteHasta;
        public string CbteFch;
        public double ImpTotal; //Double 13+2
        public double ImpTotConc;
        public double ImpNeto;
        public double ImpOpEx;
        public double ImpIVA;
        public double ImpTrib;        public string FchServDesde; //No obligatorio
        public string FchServHasta; //No obligatorio
        public string FchVtoPago; //No obligatorio
        public string MonId;
        public double MonCotiz;
        public CbtesAsoc[] cbtesAsoc;
        public Tributos[] tributos;
        public IVA[] Iva;
        public Opcionales[] opcionales;
    }
}
