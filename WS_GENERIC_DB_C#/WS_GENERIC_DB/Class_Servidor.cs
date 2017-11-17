using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace WS_GENERIC_DB
{
    class Class_Servidor
    {
        public SqlConnection Conec = new SqlConnection();

        public string Conectar(string vConnectionString)
        {
            string outmessage = "";

            try
            {
                string DBString = vConnectionString;

                Conec.ConnectionString = DBString;
                Conec.Open();

                outmessage = "[OK_OPEN_CONNECTION]";
            }
            catch (Exception ex)
            {
                outmessage = ex.Message;
            }
            return outmessage;
        }

        public string Desconectar()
        {
            string outmessage = "";

            try
            {
                Conec.Close();
                Conec = null;

                outmessage = "[OK_CLOSE_CONNECTION]";
            }
            catch (Exception ex)
            {
                outmessage = ex.Message;
                Conec = null;
            }
            return outmessage;
        }
    }
}
