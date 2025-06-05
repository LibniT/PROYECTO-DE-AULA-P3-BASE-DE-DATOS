using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Mano.DAl
{
    public class ConexionOracle
    {
        private string cadenaConexion = "User Id=DataPiece;Password=zoroDagoat;Data Source=localhost:1521/xepdb1;";

        protected OracleConnection conexion;

        public ConexionOracle()
        {
            conexion = new OracleConnection(cadenaConexion);
        }

        public bool AbrirConexion()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
                return true;
            }
            return false;
        }

        public void CerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }

        public OracleConnection ObtenerConexion()
        {
            return conexion;
        }

        // Método auxiliar para convertir decimales de Oracle de forma segura
        protected decimal ConvertirDecimalOracle(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return 0;

            if (valor is Oracle.ManagedDataAccess.Types.OracleDecimal oracleDecimal)
                return oracleDecimal.IsNull ? 0 : (decimal)oracleDecimal.Value;

            return Convert.ToDecimal(valor);
        }
    }
}
