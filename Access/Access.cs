using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace PensionConsultants.Data.Access
{
    public class DataAccessComponent
    {
        public string ConnectionString { get; private set; }

        public enum Connections
        {
            PCIISP_PCI_ISP_DB_Ver2,
            PCIISP_InvestmentDatabase_Test,
            PCIISP_PCI_ISP_ImportStage,
            PCIISP_TicketTracker,
            PCIDB_DataIntegrationHub,
            PCIDB_PlanPerformance,
            PCIDB_RfpTool,
            PCIDB_AskAway,
            PCIDB_VendorServicesProgram,
            PCIDB_Pension_Consultants_MSCRM
        }

        public enum SecurityTypes
        {
            Integrated,
            Impersonate
        }

        public DataAccessComponent(Connections _connection)
        {
            SetDatabaseConnectionString(_connection);
        }

        public DataAccessComponent(Connections _connection, SecurityTypes _securityType)
        {
            SetDatabaseConnectionString(_connection, _securityType);
        }

        private void SetDatabaseConnectionString(Connections _connection)
        {
            switch (_connection)
            {
                case Connections.PCIISP_PCI_ISP_DB_Ver2:
                    ConnectionString = "Data Source=PCIISP;Initial Catalog=PCI_ISP_DB_Ver2;Integrated Security=True";
                    break;
                case Connections.PCIISP_InvestmentDatabase_Test:
                    ConnectionString = "Data Source=PCIISP;Initial Catalog=InvestmentDatabase_Test;Integrated Security=True";
                    break;
                case Connections.PCIISP_PCI_ISP_ImportStage:
                    ConnectionString = "Data Source=PCIISP;Initial Catalog=PCI_ISP_ImportStage;Integrated Security=True";
                    break;
                case Connections.PCIISP_TicketTracker:
                    ConnectionString = "Data Source=PCIISP;Initial Catalog=TicketTracker;Integrated Security=True";
                    break;
                case Connections.PCIDB_DataIntegrationHub:
                    ConnectionString = "Data Source=PCI-DB;Initial Catalog=DataIntegrationHub;Integrated Security=True";
                    break;
                case Connections.PCIDB_PlanPerformance:
                    ConnectionString = "Data Source=PCI-DB;Initial Catalog=PlanPerformance;Integrated Security=True";
                    break;
                case Connections.PCIDB_RfpTool:
                    ConnectionString = "Data Source=PCI-DB;Initial Catalog=RfpTool;Integrated Security=True";
                    break;
                case Connections.PCIDB_AskAway:
                    ConnectionString = "Data Source=PCI-DB;Initial Catalog=AskAway;Integrated Security=True";
                    break;
                case Connections.PCIDB_VendorServicesProgram:
                    ConnectionString = "Data Source=PCI-DB;Initial Catalog=VendorServicesProgram;Integrated Security=True";
                    break;
                case Connections.PCIDB_Pension_Consultants_MSCRM:
                    ConnectionString = "Data Source=PCI-DB;Initial Catalog=Pension_Consultants_MSCRM;Integrated Security=True";
                    break;
            }
        }

        private void SetDatabaseConnectionString(Connections _connection, SecurityTypes _securityType)
        {
            switch (_securityType)
            {
                case SecurityTypes.Integrated :
                    SetDatabaseConnectionString(_connection);
                    break;
                case SecurityTypes.Impersonate:
                    switch (_connection)
                    {
                        case Connections.PCIISP_PCI_ISP_DB_Ver2:
                            ConnectionString = GetConnectionString("PCIISP_PCI_ISP_DB_Ver2");
                            break;
                        case Connections.PCIISP_InvestmentDatabase_Test:
                            throw new NotImplementedException();
                        case Connections.PCIISP_PCI_ISP_ImportStage:
                            throw new NotImplementedException();
                        case Connections.PCIISP_TicketTracker:
                            throw new NotImplementedException();
                        case Connections.PCIDB_DataIntegrationHub:
                            ConnectionString = GetConnectionString("PCIDB_DataIntegrationHub");
                            break;
                        case Connections.PCIDB_PlanPerformance:
                            ConnectionString = GetConnectionString("PCIDB_PlanPerformance");
                            break;
                        case Connections.PCIDB_RfpTool:
                            ConnectionString = GetConnectionString("PCIDB_RfpTool");
                            break;
                        case Connections.PCIDB_AskAway:
                            ConnectionString = GetConnectionString("PCIDB_AskAway");
                            break;
                        case Connections.PCIDB_VendorServicesProgram:
                            ConnectionString = GetConnectionString("PCIDB_VendorServicesProgram");
                            break;
                        case Connections.PCIDB_Pension_Consultants_MSCRM:
                            throw new NotImplementedException();
                    }
                    break;
            }
        }

        private string GetConnectionString(string name)
        {
            var settings = PensionConsultants.Data.Properties.Settings.Default;

            if (settings[name] != null)
            {
                var setting = settings[name];
                return settings[name].ToString();
            }

            return "";
        }

        public bool ConnectionSucceeded()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public DataTable ExecuteStoredProcedureQuery(string storedProcedureName, Hashtable parameterList = null)
        {
            DataTable dt = null;
            int counter = 0;

            SqlConnection objSqlConnection = new SqlConnection(ConnectionString);

            SqlCommand objSqlCommand = new SqlCommand(storedProcedureName, objSqlConnection);
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            using (objSqlConnection)
            {
                using (objSqlCommand)
                {
                    if (parameterList != null)
                    {
                        foreach (string parametername in parameterList.Keys)
                        {
                            if (parameterList[parametername] == null || String.IsNullOrWhiteSpace(parameterList[parametername].ToString()))
                            {
                                objSqlCommand.Parameters.AddWithValue(parametername, DBNull.Value);
                            }
                            else
                            {
                                objSqlCommand.Parameters.AddWithValue(parametername, parameterList[parametername]);
                            }

                            if (parametername.StartsWith("@OUT_"))
                            {
                                objSqlCommand.Parameters[counter].Direction = ParameterDirection.Output;
                            }
                            else
                            {
                                objSqlCommand.Parameters[counter].Direction = ParameterDirection.Input;
                            }

                            counter++;
                        }
                    }

                    try
                    {
                        objSqlConnection.Open();
                        dt = new DataTable();
                        dt.Load(objSqlCommand.ExecuteReader());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return dt;
        }

        public DataTable ExecuteSqlQuery(string sqlCommand, Hashtable parameterList = null)
        {
            DataTable dt = null;
            int counter = 0;

            SqlConnection objSqlConnection = new SqlConnection(ConnectionString);

            SqlCommand objSqlCommand = new SqlCommand(sqlCommand, objSqlConnection);
            objSqlCommand.CommandType = CommandType.Text;

            using (objSqlConnection)
            {
                using (objSqlCommand)
                {
                    if (parameterList != null)
                    {
                        foreach (string parametername in parameterList.Keys)
                        {
                            if (parameterList[parametername] == null || String.IsNullOrWhiteSpace(parameterList[parametername].ToString()))
                            {
                                objSqlCommand.Parameters.AddWithValue(parametername, DBNull.Value);
                            }
                            else
                            {
                                objSqlCommand.Parameters.AddWithValue(parametername, parameterList[parametername]);
                            }

                            if (parametername.StartsWith("@OUT_"))
                            {
                                objSqlCommand.Parameters[counter].Direction = ParameterDirection.Output;
                            }
                            else
                            {
                                objSqlCommand.Parameters[counter].Direction = ParameterDirection.Input;
                            }

                            counter++;
                        }
                    }

                    try
                    {
                        objSqlConnection.Open();
                        dt = new DataTable();
                        dt.Load(objSqlCommand.ExecuteReader());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return dt;
        }

        public Int32 ExecuteStoredProcedureNonQuery(string storedProcedureName, Hashtable parameterList = null)
        {
            Int32 rowsAffected = 0;
            int counter = 0;

            SqlConnection objSqlConnection = new SqlConnection(ConnectionString);

            SqlCommand objSqlCommand = new SqlCommand(storedProcedureName, objSqlConnection);
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            using (objSqlConnection)
            {
                using (objSqlCommand)
                {
                    if (parameterList != null)
                    {
                        foreach (string parametername in parameterList.Keys)
                        {
                            if (parameterList[parametername] == null || String.IsNullOrWhiteSpace(parameterList[parametername].ToString()))
                            {
                                objSqlCommand.Parameters.AddWithValue(parametername, DBNull.Value);
                            }
                            else
                            {
                                objSqlCommand.Parameters.AddWithValue(parametername, parameterList[parametername]);
                            }

                            if (parametername.StartsWith("@OUT_"))
                            {
                                objSqlCommand.Parameters[counter].Direction = ParameterDirection.Output;
                            }
                            else
                            {
                                objSqlCommand.Parameters[counter].Direction = ParameterDirection.Input;
                            }

                            counter++;
                        }
                    }

                    try
                    {
                        objSqlConnection.Open();
                        rowsAffected = objSqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return rowsAffected;
        }
    }
}
