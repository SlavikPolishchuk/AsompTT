using AsompTTViewStatus.Models;
using Oracle.ManagedDataAccess.Client;
using System.Xml;

namespace AsompTTViewStatus.DAL
{
    public static class DAL
    {
        static string sqlText;
        public static string ConnectionString = string.Empty;


        public static List<ExportApi> GetListAsrkFile(string city)
        {
            switch (city)
            {
                case "TT":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-b.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=kyasomp_dg)));User Id=API;Password=API123";
                    break;
                case "Lviv":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-a.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=lvasomp_dg)));User Id=API;Password=API123";
                    break;
                default:
                    break;
            }

            List<ExportApi> _listExportApi = new List<ExportApi>();
            string str = string.Empty;


            sqlText = @"SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE  FROM  
                        (
                        SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE FROM BOU.ASRK_FILES_EXPORT_API ORDER BY DATEINS DESC  
                        )
                        WHERE  rownum <= 60 ";

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
                        ,
                        Barcode = (reader["BARCODE"] is System.DBNull) ? string.Empty : reader["BARCODE"].ToString()
                        ,
                        DataStr = (reader["DATASTR"] is System.DBNull) ? string.Empty : reader["DATASTR"].ToString()
                        ,
                        Dateins = reader["DATEINS"].ToString()
                        ,
                        State = (Int16)reader["STATE"]
                        ,
                        Message = (reader["MESSAGE"] is System.DBNull) ? string.Empty : reader["MESSAGE"].ToString()

                    });
                }

                oracleConnection.Close();
            }
            return _listExportApi;
        }

        public static List<ExportApi> GetListAsrkFile(string city, DateTime dateF, DateTime dateT)
        {
            switch (city)
            {
                case "TT":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-b.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=kyasomp_dg)));User Id=API;Password=API123";
                    break;
                case "Lviv":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-a.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=lvasomp_dg)));User Id=API;Password=API123";
                    break;
                default:
                    break;
            }

            List<ExportApi> _listExportApi = new List<ExportApi>();
            string str = string.Empty;


            sqlText = @"SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE  FROM  BOU.ASRK_FILES_EXPORT_API
                        WHERE  TO_DATE(DATEINS) BETWEEN TO_DATE(:dateF) and TO_DATE(:dateT)  and rownum <= 60";

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                oracleConnection.Open();

                OracleCommand command = new OracleCommand(sqlText, oracleConnection);
                command.Parameters.Add("dateF",OracleDbType.Date,dateF,System.Data.ParameterDirection.Input);
                command.Parameters.Add("dateT", OracleDbType.Date, dateT, System.Data.ParameterDirection.Input);
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

        public static List<ExportApi> GetListAsrkFile(string city, string searchText)
        {
            switch (city)
            {
                case "TT":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-b.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=kyasomp_dg)));User Id=API;Password=API123";
                    break;
                case "Lviv":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-a.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=lvasomp_dg)));User Id=API;Password=API123";
                    break;
                default:
                    break;
            }

            List<ExportApi> _listExportApi = new List<ExportApi>();
            string str = string.Empty;


            sqlText = @"SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE  FROM  
                        (
                        SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE FROM BOU.ASRK_FILES_EXPORT_API ORDER BY DATEINS DESC  
                        )
                        WHERE  BARCODE = :barcod";

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                oracleConnection.Open();

                OracleCommand command = new OracleCommand(sqlText, oracleConnection);
                command.Parameters.Add("barcod", searchText);
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _listExportApi.Add(new ExportApi()
                    {
                        File_Id = (decimal)reader["FILE_ID"]
                        ,
                        Barcode = (reader["BARCODE"] is System.DBNull) ? string.Empty : reader["BARCODE"].ToString()
                        ,
                        DataStr = (reader["DATASTR"] is System.DBNull) ? string.Empty : reader["DATASTR"].ToString()
                        ,
                        Dateins = reader["DATEINS"].ToString()
                        ,
                        State = (Int16)reader["STATE"]
                        ,
                        Message = (reader["MESSAGE"] is System.DBNull) ? string.Empty : reader["MESSAGE"].ToString()

                    });
                }

                oracleConnection.Close();
            }
            return _listExportApi;
        }

        public static List<ExportApi> GetListAsrkFile(string city,  int? fileId)
        {
            switch (city)
            {
                case "TT":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-b.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=kyasomp_dg)));User Id=API;Password=API123";
                    break;
                case "Lviv":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-a.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=lvasomp_dg)));User Id=API;Password=API123";
                    break;
                default:
                    break;
            }

            List<ExportApi> _listExportApi = new List<ExportApi>();
            string str = string.Empty;


            sqlText = @"SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE  FROM  BOU.ASRK_FILES_EXPORT_API
                        WHERE  FILE_ID = :fileId ORDER BY DATEINS DESC ";

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                oracleConnection.Open();

                OracleCommand command = new OracleCommand(sqlText, oracleConnection);
                command.Parameters.Add("fileId",fileId);
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _listExportApi.Add(new ExportApi()
                    {
                        File_Id = (decimal)reader["FILE_ID"]
                        ,
                        Barcode = (reader["BARCODE"] is System.DBNull) ? string.Empty : reader["BARCODE"].ToString()
                        ,
                        DataStr = (reader["DATASTR"] is System.DBNull) ? string.Empty : reader["DATASTR"].ToString()
                        ,
                        Dateins = reader["DATEINS"].ToString()
                        ,
                        State = (Int16)reader["STATE"]
                        ,
                        Message = (reader["MESSAGE"] is System.DBNull) ? string.Empty : reader["MESSAGE"].ToString()

                    });
                }

                oracleConnection.Close();
            }
            return _listExportApi;
        }


        public static List<ExportApi> GetListAsrkFile(string city, string searchText, int? fileId)
        {
            switch (city)
            {
                case "TT":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-b.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=kyasomp_dg)));User Id=API;Password=API123";
                    break;
                case "Lviv":
                    ConnectionString =
                " DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=50-oracle-a.ukrposhta.loc)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=lvasomp_dg)));User Id=API;Password=API123";
                    break;
                default:
                    break;
            }

            List<ExportApi> _listExportApi = new List<ExportApi>();
            string str = string.Empty;


            sqlText = @"SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE  FROM  
                        (
                        SELECT FILE_ID, BARCODE, DATASTR, DATEINS, STATE, MESSAGE FROM BOU.ASRK_FILES_EXPORT_API ORDER BY DATEINS DESC  
                        )
                        WHERE  FILE_ID = :fileId and BARCODE=:barcode";

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                oracleConnection.Open();

                OracleCommand command = new OracleCommand(sqlText, oracleConnection);
                command.Parameters.Add("fileId",fileId);
                command.Parameters.Add("barcode",searchText);
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    _listExportApi.Add(new ExportApi()
                    {
                        File_Id = (decimal)reader["FILE_ID"]
                        ,
                        Barcode = (reader["BARCODE"] is System.DBNull) ? string.Empty : reader["BARCODE"].ToString()
                        ,
                        DataStr = (reader["DATASTR"] is System.DBNull) ? string.Empty : reader["DATASTR"].ToString()
                        ,
                        Dateins = reader["DATEINS"].ToString()
                        ,
                        State = (Int16)reader["STATE"]
                        ,
                        Message = (reader["MESSAGE"] is System.DBNull) ? string.Empty : reader["MESSAGE"].ToString()

                    });
                }

                oracleConnection.Close();
            }
            return _listExportApi;
        }


    }
}
