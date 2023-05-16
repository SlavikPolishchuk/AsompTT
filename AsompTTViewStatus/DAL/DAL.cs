using AsompTTViewStatus.Models;
using Oracle.ManagedDataAccess.Client;
using System.Xml;

namespace AsompTTViewStatus.DAL
{
    public static class DAL
    {
        static string sqlText;

        //Kiev test
        //public static string ConnectionString =
        //        " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-b.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=kyasomp_dg)));User Id=API;Password=API123";


        //Lviv
        public static string ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-a.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=lvasomp_dg)));User Id=API;Password=API123";



        public static List<ExportApi> GetListAsrkFile()
        {
            List<ExportApi> _listExportApi = new List<ExportApi>();
            string str = string.Empty;


            sqlText = @"SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE  FROM  
                        (
                        SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE FROM BOU.ASRK_FILES_EXPORT_API ORDER BY DATEINS DESC  
                        )
                        WHERE  rownum <= 60";

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                oracleConnection.Open();

                OracleCommand command = new OracleCommand(sqlText, oracleConnection);
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _listExportApi.Add(new ExportApi()
                    {
                        File_Id = (decimal)reader["FILE_ID"]
                        ,Barcode = (reader["BARCODE"] is System.DBNull)?string.Empty: reader["BARCODE"].ToString()
                        ,DataStr = (reader["DATASTR"] is System.DBNull) ? string.Empty : reader["DATASTR"].ToString()
                        ,Dateins = reader["DATEINS"].ToString()
                        ,State = (Int16)reader["STATE"]
                        ,Message = (reader["MESSAGE"] is System.DBNull) ? string.Empty : reader["MESSAGE"].ToString()

                    });
                }

                oracleConnection.Close();
            }
            return _listExportApi;
        }
    }
}
