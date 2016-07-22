using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Blibox.Logica.Model
{
    
    [XmlRootAttribute("soap:Envelope", Namespace = "http://www.w3.org", IsNullable = false)]
    public class Envelope
    {
               
    }

    [XmlRootAttribute("soap:Body", IsNullable = false)]
    public class Body
    {

    }

    [XmlRootAttribute("BFEAuthorize", IsNullable = false)]
    public class BFEAuthorize
    {

    }

    [XmlRootAttribute("soap:Body", IsNullable = false)]
    public class Auth
    {
        public string Token;
        public string Sign;
        public long Cuit;
        
    }

}

//xmlns.Add("one", "urn:names:specification:schema:xsd:one")
//xmlns.Add("two",  "urn:names:specification:schema:xsd:two")
//xmlns.Add("three",  "urn:names:specification:schema:xsd:three")


//<?xml version="1.0" encoding="utf-8"?> 
//<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"> 
//<soap:Body> 
//	<BFEAuthorize xmlns = "http://ar.gov.afip.dif.bfev1/" >

//    < Auth >

//        < Token > string </ Token >

//        < Sign > string </ Sign >

//        < Cuit > long </ Cuit >

//    </ Auth >

//    < Cmp >

//        < Id > long </ Id >

//        < Tipo_doc > short </ Tipo_doc >

//        < Nro_doc > long </ Nro_doc >

//        < Zona > short </ Zona >

//        < Tipo_cbte > short </ Tipo_cbte >

//        < Punto_vta > int </ Punto_vta >

//        < Cbte_nro > long </ Cbte_nro >

//        < Imp_total > double </ Imp_total >

//        < Imp_tot_conc > double </ Imp_tot_conc >

//        < Imp_neto > double </ Imp_neto >

//        < Impto_liq > double </ Impto_liq >

//        < Impto_liq_rni > double </ Impto_liq_rni >

//        < Imp_op_ex > double </ Imp_op_ex >

//        < Imp_perc > double </ Imp_perc >

//        < Imp_iibb > double </ Imp_iibb >

//        < Imp_perc_mun > double </ Imp_perc_mun >

//        < Imp_internos > double </ Imp_internos >

//        < Imp_moneda_Id > string </ Imp_moneda_Id >

//        < Imp_moneda_ctz > double </ Imp_moneda_ctz >

//        < Fecha_cbte > string </ Fecha_cbte >

//        < Opcionales >

//            < Opcional >

//            < Id > string </ Id >

//            < Valor > string </ Valor >

//            </ Opcional > < Opcional >

//            < Id > string </ Id >

//            < Valor > string </ Valor >

//            </ Opcional >

//            </ Opcionales >

//            < Items >

//                < Item >

//                    < Pro_codigo_ncm > string </ Pro_codigo_ncm >

//                    < Pro_codigo_sec > string </ Pro_codigo_sec >

//                    < Pro_ds > string </ Pro_ds >

//                    < Pro_qty > double </ Pro_qty >

//                    < Pro_umed > int </ Pro_umed >

//                    < Pro_precio_uni > double </ Pro_precio_uni >

//                    < Imp_bonif > double </ Imp_bonif >

//                    < Imp_total > double </ Imp_total >

//                    < Iva_id > short </ Iva_id >

//                </ Item >

//                < Item >

//                    < Pro_codigo_ncm > string </ Pro_codigo_ncm >

//                    < Pro_codigo_sec > string </ Pro_codigo_sec >

//                    < Pro_ds > string </ Pro_ds >

//                    < Pro_qty > double </ Pro_qty >

//                    < Pro_umed > int </ Pro_umed >

//                    < Pro_precio_uni > double </ Pro_precio_uni >

//                    < Imp_bonif > double </ Imp_bonif >

//                    < Imp_total > double </ Imp_total >

//                    < Iva_id > short </ Iva_id >

//                </ Item >

//            </ Items >

//    </ Cmp >

//    </ BFEAuthorize >
//</ soap:Body> 
//</soap:Envelope>