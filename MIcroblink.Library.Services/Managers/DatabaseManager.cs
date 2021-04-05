using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Microblink.Library.Services.Managers
{
    public interface IDatabaseManager { }


    [Serializable]
	public class DatabaseManager : IDatabaseManager
	{
        private string ConnectionString;
        
        private SqlConnection Connection
		{
			get
			{
				var connection = new SqlConnection(ConnectionString);				
                return connection;
			}
		}

        public DatabaseManager(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public static DatabaseManager Create(string connectionString)
		{
			return new DatabaseManager(connectionString);
		}


		public async Task<DataTable> FillTable(string queryString)
		{
			var sqlCommand = new SqlCommand(queryString);
			
            return await FillTable(sqlCommand);
		}

		public async Task<DataTable> FillTable(SqlCommand sqlCommand)
		{
            try
            {   
                sqlCommand.Connection = Connection;
                await sqlCommand.Connection.OpenAsync();

                using (var dataReader = await sqlCommand.ExecuteReaderAsync())
                {
                    var resultTable = new DataTable();
                    resultTable.Load(dataReader);
                    return resultTable;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
			finally
            {
                await sqlCommand.Connection.CloseAsync();
                await sqlCommand.Connection.DisposeAsync();
            }
		}

        public async Task<DataTable> ExecuteProcedure(String procedureName)
        {
            return await ExecuteProcedure(procedureName, new Hashtable());
        }

        public async Task<DataTable> ExecuteProcedure(String procedureName, Hashtable parameteres)
        {
            if(parameteres == null)
                parameteres = new Hashtable();

            var sqlCommand = new SqlCommand(procedureName);
            sqlCommand.Connection = Connection;
            sqlCommand.CommandType = CommandType.StoredProcedure;

            foreach (object key in parameteres.Keys)
            {
                string parameterName = (key.ToString().StartsWith("@")) ? String.Empty : "@";
                parameterName += key.ToString();
                sqlCommand.Parameters.AddWithValue(parameterName, parameteres[key]);
            }

            return await FillTable(sqlCommand);
        }
	}
}
