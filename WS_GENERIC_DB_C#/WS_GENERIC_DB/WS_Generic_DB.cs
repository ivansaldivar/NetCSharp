//////////////////////////////////////////////////////////////////////////
//                                                                      //
//    WEB SERVICE GENERIC DB V.1.5                                      //
//                                                                      //
//    AUTORES:    IVAN SALDIVAR RODRIGUEZ (R)2010-2012                  //                   
//                JOSE CORTÉS ARANEDA     (R)2010-2012                  //
//                                                                      //
//    TODOS LOS DERECHOS DE COPIA ESTÁN RESERVADOS A:                   //
//                                                                      //
//              * IVAN SALDIVAR RODRIGUEZ                               //
//              * JOSE CORTÉS ARANEDA                                   //
//                                                                      //
//    Y SU USO NO AUTORIZADO SERÁ SANCIONADO DE ACUERDO A LAS LEYES DE  //
//    DERECHO  DE AUTOR VIGENTES EN EL PAÍS DONDE SE HAYA COMETIDO LA   //
//    FALTA.                                                            //
//    SE PROHIBE LA COPIA, REPRODUCCIÓN Y DISTRIBUCIÓN CON FINES        //
//    COMERCIALES  SIN PREVIA AUTORIZACIÓN. LOS ALGORITMO QUE           //
//    COMPONEN ESTE SOFTWARE SON PROPIEDAD DEL AUTOR, POR LO TANTO,     //
//    ESTOS FORMAN PARTE INTEGRA DEL CÓDIGO EN EL CUAL SE HAN           //
//    IMPLEMENTADO.                                                     //
//                                                                      //
//    SANTIAGO - CHILE, 2012-01-01                                      //       
//                                                                      //
//////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

using Microsoft.CSharp;

namespace WS_GENERIC_DB
{
    public class WS_Generic_DB
    {
        public Byte[] CONSULTA_GENERICA_SP_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, string vConnectionString)
        {
            SqlCommand Cmd = new SqlCommand();
            System.Data.DataSet Resultado = new System.Data.DataSet();
            System.Data.DataRow dr;

            string XMLParams_str = "";
            string msgExcepcion = "";
            Byte[] objSerializado = null;

            Class_Servidor Serv = new Class_Servidor();

            //----------------------------------------------------------------
            //IVAN SALDIVAR RODRIGUEZ (20120926)
            //OBJETIVO  :   CONSULTA PERMITE EJECUTAR EN FORMA GENÉRICA 
            //              CUALQUIER SP PASANDO SU NOMBRE Y PARÁMETROS.
            //
            //SALIDA    :   RECORDSET SERIALIZADO.
            //----------------------------------------------------------------

            try
            {
                Serv.Conectar(vConnectionString);
                Cmd.CommandText = Deserialize(NOMBRESP).ToString();
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.Connection = Serv.Conec;

                XMLParams_str = Deserialize(XMLParams).ToString();

                if (!string.IsNullOrEmpty(XMLParams_str))
                {
                    CargaParametrosSP(Cmd, XMLParams_str);
                }

                SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                SqlAdapter.SelectCommand = Cmd;

                SqlAdapter.Fill(Resultado, "CONSULTA_GENERICA_SP_SERIALIZADA");

                if (Resultado.Tables[0].Rows.Count > 0)
                {
                    dr = Resultado.Tables[0].Rows[0];

                    if (dr[0].ToString() != "-1")
                    {

                        //SALIDA CON RETORNO DE DATOS
                        objSerializado = Serialize(Resultado, true);
                    }
                    else
                    {
                        msgExcepcion = "Procedimiento: " + dr["ERROR_PROCEDURE_"].ToString() +
                                       "Código error: " + dr["ERROR_NUMBER_"].ToString() +
                                       "Descripción: " + dr["ERROR_MESSAGE_"].ToString() +
                                       "Número línea: " + dr["ERROR_LINE_"].ToString();
                        //(1) SALIDA CON ERROR
                        objSerializado = Serialize(msgExcepcion, true);
                    }
                }

                Serv.Desconectar();

            }
            catch (Exception ex)
            {
                msgExcepcion = "Procedimiento: " + Cmd.CommandText.ToString() +
                               "Código error:" +
                               "Descripción:" + ex.Message +
                               "Número línea: ";

                //(2) SALIDA CON ERROR
                objSerializado = Serialize(msgExcepcion, true);
                Serv.Desconectar();
                Resultado = null;
            }

            return objSerializado;

        }

        public Byte[] CONSULTA_GENERICA_SP2_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, string vConnectionString)
        {
            SqlCommand Cmd = new SqlCommand();
            System.Data.DataSet Resultado = new System.Data.DataSet();
            System.Data.DataRow dr;

            string XMLParams_str = "";
            string msgExcepcion = "";
            string salida = "";
            Byte[] objSerializado = null;

            Class_Servidor Serv = new Class_Servidor();

            //----------------------------------------------------------------
            //IVAN SALDIVAR RODRIGUEZ (20120926)
            //OBJETIVO  :   CONSULTA PERMITE EJECUTAR EN FORMA GENÉRICA 
            //              CUALQUIER SP PASANDO SU NOMBRE Y PARÁMETROS.
            //              ESTÁ DISEÑADA ESPECIALMENTE PARA EJECUTAR 
            //              PROCEDIMIENTOS QUE REALIZAN OPERACIONES DE 
            //'             ACTUALIZACIÓN EN LA BASE DE DATOS.
            //
            //SALIDA    :   CADENA DE TEXTO SERIALIZADA.
            //----------------------------------------------------------------

            try
            {
                Serv.Conectar(vConnectionString);
                Cmd.CommandText = Deserialize(NOMBRESP).ToString();
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.Connection = Serv.Conec;

                XMLParams_str = Deserialize(XMLParams).ToString();

                if (!string.IsNullOrEmpty(XMLParams_str))
                {
                    CargaParametrosSP(Cmd, XMLParams_str);
                }

                SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                SqlAdapter.SelectCommand = Cmd;

                SqlAdapter.Fill(Resultado, "CONSULTA_GENERICA_SP2_SERIALIZADA");

                if (Resultado.Tables[0].Rows.Count > 0)
                {
                    dr = Resultado.Tables[0].Rows[0];

                    if (dr[0].ToString() != "-1")
                    {

                        //SALIDA CON RETORNO DE DATOS
                        salida = "[OK]" + " - " + dr[0].ToString();
                        objSerializado = Serialize(salida, true);
                    }
                    else
                    {
                        msgExcepcion = "Procedimiento: " + dr["ERROR_PROCEDURE_"].ToString() +
                                       "Código error: " + dr["ERROR_NUMBER_"].ToString() +
                                       "Descripción: " + dr["ERROR_MESSAGE_"].ToString() +
                                       "Número línea: " + dr["ERROR_LINE_"].ToString();
                        //(1) SALIDA CON ERROR
                        objSerializado = Serialize(msgExcepcion, true);
                    }
                }

                Serv.Desconectar();

            }
            catch (Exception ex)
            {
                msgExcepcion = "Procedimiento: " + Cmd.CommandText.ToString() +
                               "Código error:" +
                               "Descripción:" + ex.Message +
                               "Número línea: ";

                //(2) SALIDA CON ERROR
                objSerializado = Serialize(msgExcepcion, true);
                Serv.Desconectar();
                Resultado = null;
            }

            return objSerializado;

        }

        public Byte[] CONSULTA_GENERICA_SP3_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, Byte[] XMLDetalle, string vConnectionString)
        {
            SqlCommand Cmd = new SqlCommand();
            System.Data.DataSet Resultado = new System.Data.DataSet();
            System.Data.DataRow dr;
            System.Xml.XmlDocument oDOM = new System.Xml.XmlDocument();

            string XMLParams_str = "";
            string XMLDetalle_str = "";
            string msgExcepcion = "";
            string salida = "";
            Byte[] objSerializado = null;

            Class_Servidor Serv = new Class_Servidor();

            //----------------------------------------------------------------
            //IVAN SALDIVAR RODRIGUEZ (20120926)
            //OBJETIVO  :   CONSULTA PERMITE EJECUTAR EN FORMA GENÉRICA 
            //              CUALQUIER SP PASANDO SU NOMBRE Y PARÁMETROS.
            //              ESTÁ DISEÑADA ESPECIALMENTE PARA EJECUTAR PROCEDIMIENTOS 
            //              QUE REALIZAN OPERACIONES DE  ACTUALIZACIÓN EN LA BASE DE 
            //              DATOS.
            //              A DIFERENCIA DE CONSULTA_GENERICA_SP2, ESTE WEB METHOD 
            //              CONSIDERA UN NUEVO PARÁMETRO (XMLDetalles) EN EL CUAL SE  
            //              INCLUYEN LOS VALORES PARA GRABAR EN BASE DE DATOS VARIAS 
            //              INSTANCIAS REPETIDAS DE UN TIPO DE REGISTRO "DETALLE",  
            //              POR EJEMPLO: UNA RECETA Y SU DETALLE, EN ESTE CASO EL DETALLE 
            //              DE LA RECETA SE FORMATEA A UN XML QUE SERÁ PROCESADO 
            //              INTERNAMENTE EN EL PROCEDIMIENTO INVOCADO, DE ESTE MODO SE 
            //              ENVÍA DE UNA SOLA VEZ A LA CAPA DE DATOS LAS INSTANCIAS 
            //              REPETIDAS ASOCIADAS A UNA CABECERA.
            //
            //SALIDA    :   CADENA DE TEXTO SERIALIZADA.
            //----------------------------------------------------------------

            try
            {
                Serv.Conectar(vConnectionString);
                Cmd.CommandText = Deserialize(NOMBRESP).ToString();
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.Connection = Serv.Conec;

                XMLParams_str = Deserialize(XMLParams).ToString();

                if (!string.IsNullOrEmpty(XMLParams_str))
                {
                    CargaParametrosSP(Cmd, XMLParams_str);
                }

                //'AQUÍ AGREGAMOS EL PARÁMETRO ASOCIADO AL DETALLE DE LA INSTANCIA PRIMARIA
                XMLDetalle_str = Deserialize(XMLDetalle).ToString();
                if (XMLDetalle_str.ToString().Trim() != "")
                {
                    oDOM.LoadXml(XMLDetalle_str);
                    Cmd.Parameters.AddWithValue("@DETALLE_XML", oDOM.InnerXml);
                }

                SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                SqlAdapter.SelectCommand = Cmd;

                SqlAdapter.Fill(Resultado, "CONSULTA_GENERICA_SP3_SERIALIZADA");

                if (Resultado.Tables[0].Rows.Count > 0)
                {
                    dr = Resultado.Tables[0].Rows[0];

                    if (dr[0].ToString() != "-1")
                    {

                        //SALIDA CON RETORNO DE DATOS
                        salida = "[OK]" + " - " + dr[0].ToString();
                        objSerializado = Serialize(salida, true);
                    }
                    else
                    {
                        msgExcepcion = "Procedimiento: " + dr["ERROR_PROCEDURE_"].ToString() +
                                       "Código error: " + dr["ERROR_NUMBER_"].ToString() +
                                       "Descripción: " + dr["ERROR_MESSAGE_"].ToString() +
                                       "Número línea: " + dr["ERROR_LINE_"].ToString();
                        //(1) SALIDA CON ERROR
                        objSerializado = Serialize(msgExcepcion, true);
                    }
                }

                Serv.Desconectar();

            }
            catch (Exception ex)
            {
                msgExcepcion = "Procedimiento: " + Cmd.CommandText.ToString() +
                               "Código error:" +
                               "Descripción:" + ex.Message +
                               "Número línea: ";

                //(2) SALIDA CON ERROR
                objSerializado = Serialize(msgExcepcion, true);
                Serv.Desconectar();
                Resultado = null;
            }

            return objSerializado;

        }

        public Byte[] CONSULTA_GENERICA_SP4_SERIALIZADA(Byte[] NOMBRESP, Byte[] XMLParams, Byte[] XMLDetalle, Byte[] ObjetoBinario, string vConnectionString)
        {
            SqlCommand Cmd = new SqlCommand();
            System.Data.DataSet Resultado = new System.Data.DataSet();
            System.Data.DataRow dr;
            System.Xml.XmlDocument oDOM = new System.Xml.XmlDocument();

            string XMLParams_str = "";
            string XMLDetalle_str = "";
            string msgExcepcion = "";
            string salida = "";
            Byte[] objSerializado = null;

            Class_Servidor Serv = new Class_Servidor();

            //----------------------------------------------------------------
            //IVAN SALDIVAR RODRIGUEZ (20120926)
            //OBJETIVO  :   CONSULTA PERMITE EJECUTAR EN FORMA GENÉRICA 
            //              CUALQUIER SP PASANDO SU NOMBRE Y PARÁMETROS.
            //              ESTÁ DISEÑADA ESPECIALMENTE PARA EJECUTAR PROCEDIMIENTOS 
            //              QUE REALIZAN OPERACIONES DE  ACTUALIZACIÓN EN LA BASE DE 
            //              DATOS.
            //              A DIFERENCIA DE CONSULTA_GENERICA_SP2, ESTE WEB METHOD 
            //              CONSIDERA UN NUEVO PARÁMETRO (XMLDetalles) EN EL CUAL SE  
            //              INCLUYEN LOS VALORES PARA GRABAR EN BASE DE DATOS VARIAS 
            //              INSTANCIAS REPETIDAS DE UN TIPO DE REGISTRO "DETALLE",  
            //              POR EJEMPLO: UNA RECETA Y SU DETALLE, EN ESTE CASO EL DETALLE 
            //              DE LA RECETA SE FORMATEA A UN XML QUE SERÁ PROCESADO 
            //              INTERNAMENTE EN EL PROCEDIMIENTO INVOCADO, DE ESTE MODO SE 
            //              ENVÍA DE UNA SOLA VEZ A LA CAPA DE DATOS LAS INSTANCIAS 
            //              REPETIDAS ASOCIADAS A UNA CABECERA.
            //
            //SALIDA    :   CADENA DE TEXTO SERIALIZADA.
            //----------------------------------------------------------------

            try
            {
                Serv.Conectar(vConnectionString);
                Cmd.CommandText = Deserialize(NOMBRESP).ToString();
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.Connection = Serv.Conec;

                XMLParams_str = Deserialize(XMLParams).ToString();

                if (!string.IsNullOrEmpty(XMLParams_str))
                {
                    CargaParametrosSP(Cmd, XMLParams_str);
                }

                //'AQUÍ AGREGAMOS EL PARÁMETRO ASOCIADO AL DETALLE DE LA INSTANCIA PRIMARIA
                XMLDetalle_str = Deserialize(XMLDetalle).ToString();
                if (XMLDetalle_str.ToString().Trim() != "")
                {
                    oDOM.LoadXml(XMLDetalle_str);
                    Cmd.Parameters.AddWithValue("@DETALLE_XML", oDOM.InnerXml);
                }

                Cmd.Parameters.AddWithValue("@OBJETOBINARIO", ObjetoBinario);

                SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                SqlAdapter.SelectCommand = Cmd;

                SqlAdapter.Fill(Resultado, "CONSULTA_GENERICA_SP3_SERIALIZADA");

                if (Resultado.Tables[0].Rows.Count > 0)
                {
                    dr = Resultado.Tables[0].Rows[0];

                    if (dr[0].ToString() != "-1")
                    {

                        //SALIDA CON RETORNO DE DATOS
                        salida = "[OK]" + " - " + dr[0].ToString();
                        objSerializado = Serialize(salida, true);
                    }
                    else
                    {
                        msgExcepcion = "Procedimiento: " + dr["ERROR_PROCEDURE_"].ToString() +
                                       "Código error: " + dr["ERROR_NUMBER_"].ToString() +
                                       "Descripción: " + dr["ERROR_MESSAGE_"].ToString() +
                                       "Número línea: " + dr["ERROR_LINE_"].ToString();
                        //(1) SALIDA CON ERROR
                        objSerializado = Serialize(msgExcepcion, true);
                    }
                }

                Serv.Desconectar();

            }
            catch (Exception ex)
            {
                msgExcepcion = "Procedimiento: " + Cmd.CommandText.ToString() +
                               "Código error:" +
                               "Descripción:" + ex.Message +
                               "Número línea: ";

                //(2) SALIDA CON ERROR
                objSerializado = Serialize(msgExcepcion, true);
                Serv.Desconectar();
                Resultado = null;
            }

            return objSerializado;

        } 

        public Boolean CargaParametrosSP(SqlCommand cmd, String XMLParams)
        {
            //----------------------------------------------------------------
            //IVAN SALDIVAR RODRIGUEZ (20120926)
            //LOS PARÁMETROS DEL PROCEDIMIENTO ALMACENADO VIENEN EN EL FORMATO
            //XML SIGUIENTE:
            //
            //'<PARAMS><PARAM nombre="@CODIGO" valor="100-001"/></PARAMS>
            //----------------------------------------------------------------

            System.Xml.XmlDocument oDOM = new System.Xml.XmlDocument();
            System.Xml.XmlNodeList listaParametros;
            String VALOR;
            String NOMBRE;
            int i;

            oDOM.LoadXml(XMLParams);
            listaParametros = oDOM.SelectNodes(".//PARAM");

            for (i = 0; i <= listaParametros.Count - 1; i++)
            {
                VALOR = listaParametros.Item(i).Attributes.GetNamedItem("valor").Value;
                NOMBRE = listaParametros.Item(i).Attributes.GetNamedItem("nombre").Value;

                cmd.Parameters.AddWithValue(NOMBRE, VALOR);
            }
            //'----------------------------------------------------------------
            return true;
        }

        public Byte[] Serialize(object Obj, Boolean AsByte)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            if (Obj != null)
            {
                bf.Serialize(ms, Obj);
            }
            else
            {
                bf.Serialize(ms, "");
            }
            return ms.ToArray();
        }

        public object Deserialize(Byte[] Obj)
        {
            if (Obj != null)
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(Obj);

                return bf.Deserialize(ms);
            }
            else
            {
                return null;
            }
        }
    }
}
