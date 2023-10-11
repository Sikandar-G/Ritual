using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// Summary description for GoldDataAccess
/// </summary>
public class GoldDataAccess
{
    private readonly string _connectionString = string.Empty;
    public SqlCommand objCommand;
    public SqlConnection objSqlConnection = null;
    GoldLogging log = null;
    public GoldDataAccess()
    {
        this._connectionString = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;
    }


    /// <summary>
    /// Open_Connection method will open the current instance of the connection object if the object is in closed state  
    /// </summary>
    /// 
    public void Open_Connection()
    {
        try
        {

            if (objSqlConnection.State == ConnectionState.Closed)
            {
                objSqlConnection.Open();
            }
        }
        catch (Exception e)
        {
            log = new GoldLogging();
            log.SendErrorToText(e);
            throw new GoldException("Data Connection open Error: {0}", e.InnerException);
        }
    }
    /// <summary>
    /// Close_Connection method will close and dispose the connection object that was created earlier.
    /// </summary>
    public void Close_Connection()
    {
        //try
        //{
        //    if (objSqlConnection.State != ConnectionState.Closed)
        //    {
        //        this.objSqlConnection.Close();
        //        this.objSqlConnection.Dispose();
        //    }
        //}
        //catch (Exception e)
        //{
        //    log = new GoldLogging();
        //    log.SendErrorToText(e);
        //    throw new GoldException("Data Connection Closed Error: {0}", e.InnerException);
        //}
    }

    /// <summary>
    /// Open_Connection method will open the current instance of the connection object if the object is in closed state  
    /// </summary>
    /// <param name="objSqlConnection"></param>
    public void Open_Connection(SqlConnection objSqlConnection)
    {
        try
        {
            if (objSqlConnection.State == ConnectionState.Closed)
                objSqlConnection.Open();
        }
        catch (Exception e)
        {
            log = new GoldLogging();
            log.SendErrorToText(e);
            throw new GoldException("Data Connection open Error: {0}", e.InnerException);
        }
    }
    #region Utilities
    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <returns></returns>
    public object ExecuteNonQuery(string procedureName)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                Open_Connection(objSqlConnection);
                ReturnValue = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Execute Non Query) Error: {0}", e.InnerException);
            }
        }
        Close_Connection();
        return ReturnValue;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection using SqlTransaction and returns the number of rows affected.
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <returns></returns>
    public object ExecuteNonQueryWithTran(string procedureName)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            Open_Connection(objSqlConnection);
            using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
                try
                {
                    command.Transaction = Transaction;
                    ReturnValue = command.ExecuteNonQuery();
                    Transaction.Commit();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(Execute Non Query With Tran) Error: {0}", e.InnerException);
                }
        }
        Close_Connection();
        return ReturnValue;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result of output parameter
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public object ExecuteNonQueryWithOutputParameters(string procedureName, SqlParameter[] parameters)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            Open_Connection(objSqlConnection);
            try
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                command.ExecuteNonQuery();
                ReturnValue = command.Parameters[parameters.Length - 1].Value;
            }
            catch (Exception e)
            {
                log = new GoldLogging();
                log.SendErrorToText(e);
                //throw new GoldException("Data Execution(Execute Non Query With Output Parameters) Error: {0}", e.InnerException);
                ReturnValue = null;
            }
        }
        Close_Connection();
        return ReturnValue;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection using SqlTransaction and returns the result of output parameter
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public object ExecuteNonQueryWithOutputParametersWithTran(string procedureName, SqlParameter[] parameters)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            Open_Connection(objSqlConnection);
            using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
                try
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    command.Transaction = Transaction;
                    command.ExecuteNonQuery();
                    ReturnValue = command.Parameters[parameters.Length - 1].Value;
                    Transaction.Commit();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(Execute Non Query With Output Parameters With Tran) Error: {0}", e.InnerException);
                }
        }
        Close_Connection();
        return ReturnValue;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public object ExecuteNonQueryWithParameters(string procedureName, SqlParameter[] parameters)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            Open_Connection(objSqlConnection);
            try
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                ReturnValue = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Execute Non Query With Parameters) Error: {0}", e.InnerException);
            }
        }
        Close_Connection();
        return ReturnValue;
    }


    public async Task<object> ExecuteNonQueryWithParametersAsync(string procedureName, SqlParameter[] parameters)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        {
            await objSqlConnection.OpenAsync();

            using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    ReturnValue = await command.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(Execute Non Query With Parameters) Error: {0}", e.InnerException);
                }
            }
        }
        // No need to close the connection manually when using 'using' statement
        return ReturnValue;
    }



    /// <summary>
    /// Executes a Transact-SQL statement against the connection using SqlTransaction and returns the number of rows affected.
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public object ExecuteNonQueryWithParametersWithTran(string procedureName, SqlParameter[] parameters)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            Open_Connection(objSqlConnection);
            using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
                try
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    command.Transaction = Transaction;
                    ReturnValue = command.ExecuteNonQuery();
                    Transaction.Commit();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(Execute Non Query With Parameters With Tran) Error: {0}", e.InnerException);
                }
        }
        Close_Connection();
        return ReturnValue;
    }

    /// <summary>
    ///Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <param name="procedureName">Store procedure Name</param>
    /// <returns></returns>
    public object ExecuteScalar(string procedureName)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            Open_Connection(objSqlConnection);
            try
            {
                ReturnValue = command.ExecuteScalar();
            }
            catch (Exception e)
            {
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Execute Scalar) Error: {0}", e.InnerException);
            }
        }
        Close_Connection();
        return ReturnValue;
    }

    /// <summary>
    ///Executes the query, and returns the first column of the first row in the result set returned by the query using SqlTransaction. Additional columns or rows are ignored.
    /// </summary>
    /// <param name="procedureName">Store procedure Name</param>
    /// <returns></returns>
    public object ExecuteScalarWithTran(string procedureName)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            Open_Connection(objSqlConnection);
            using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
                try
                {
                    command.Transaction = Transaction;
                    ReturnValue = command.ExecuteScalar();
                    Transaction.Commit();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(ExecuteScalar With Tran) Error: {0}", e.InnerException);
                }
        }
        Close_Connection();
        return ReturnValue;
    }

    /// <summary>
    /// Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters List</param>
    /// <returns></returns>
    public object ExecuteScalarWithParameters(string procedureName, SqlParameter[] parameters)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            Open_Connection(objSqlConnection);
            try
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                ReturnValue = command.ExecuteScalar();
            }
            catch (Exception e)
            {
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Execute Scalar With Parameters) Error: {0}", e.InnerException);
            }
        }
        Close_Connection();
        return ReturnValue;
    }

    /// <summary>
    /// Executes the query, and returns the first column of the first row in the result set returned by the query using SqlTransaction. Additional columns or rows are ignored.
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters List</param>
    /// <returns></returns>
    public object ExecuteScalarWithParametersWithTran(string procedureName, SqlParameter[] parameters)
    {
        object ReturnValue;
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, objSqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;
            Open_Connection(objSqlConnection);
            using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
                try
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    command.Transaction = Transaction;
                    ReturnValue = command.ExecuteScalar();
                    Transaction.Commit();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(ExecuteScalar With Parameters With Tran) Error: {0}", e.InnerException);
                }
        }
        Close_Connection();
        return ReturnValue;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format
    /// Used only when we have to return records in multiple table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <returns></returns>
    public DataSet ReturnDataset(string procedureName)
    {
        DataSet dataSet = new DataSet();
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            using (SqlDataAdapter DataAdapter = new SqlDataAdapter())
            {
                DataAdapter.SelectCommand = command;
                Open_Connection(objSqlConnection);
                try
                {
                    DataAdapter.Fill(dataSet);
                }
                catch (Exception e)
                {
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(Return Dataset) Error: {0}", e.InnerException);
                }
                finally
                {
                    command.Dispose();
                }
            }
        }
        Close_Connection();
        return dataSet;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format using SqlTransaction
    /// Used only when we have to return records in multiple table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <returns></returns>
    public DataSet ReturnDatasetWithTran(string procedureName)
    {
        DataSet dataSet = new DataSet();
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            using (SqlDataAdapter DataAdapter = new SqlDataAdapter())
            {
                DataAdapter.SelectCommand = command;
                Open_Connection(objSqlConnection);
                using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
                    try
                    {
                        DataAdapter.SelectCommand.Transaction = Transaction;
                        DataAdapter.Fill(dataSet);
                        Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Transaction.Rollback();
                        log = new GoldLogging();
                        log.SendErrorToText(e);
                        throw new GoldException("Data Execution(ReturnDataset With Tran) Error: {0}", e.InnerException);
                    }
                    finally
                    {
                        command.Dispose();
                    }
            }
        }
        Close_Connection();
        return dataSet;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format 
    /// Used only when we have to return records in multiple table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public DataSet ReturnDatasetWithParameters(string procedureName, SqlParameter[] parameters)
    {
        DataSet dataSet = new DataSet();
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using (SqlDataAdapter DataAdapter = new SqlDataAdapter())
            {
                DataAdapter.SelectCommand = command;
                Open_Connection(objSqlConnection);
                try
                {
                    DataAdapter.Fill(dataSet);
                }
                catch (Exception e)
                {
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(ReturnDataset With Parameters) Error: {0}", e.InnerException);
                }
                finally
                {
                    command.Dispose();
                }
            }
        }
        Close_Connection();
        return dataSet;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format using SqlTransaction
    /// Used only when we have to return records in multiple table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public DataSet ReturnDatasetWithParametersWithTran(string procedureName, SqlParameter[] parameters)
    {
        DataSet dataSet = new DataSet();
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using (SqlDataAdapter DataAdapter = new SqlDataAdapter())
            {
                DataAdapter.SelectCommand = command;
                Open_Connection(objSqlConnection);
                using (SqlTransaction oTransaction = objSqlConnection.BeginTransaction())
                    try
                    {
                        DataAdapter.SelectCommand.Transaction = oTransaction;
                        DataAdapter.Fill(dataSet);
                        oTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        oTransaction.Rollback();
                        log = new GoldLogging();
                        log.SendErrorToText(e);
                        throw new GoldException("Data Execution(ReturnDataset With Parameters With Tran) Error: {0}", e.InnerException);
                    }
                    finally
                    {
                        command.Dispose();
                    }
            }
        }
        Close_Connection();
        return dataSet;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format
    /// Used only when we have to return records in single table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <returns></returns>
    public DataTable ReturnDataTable(string procedureName)
    {
        DataTable dataTable = new DataTable();
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            using (SqlDataAdapter DataAdapter = new SqlDataAdapter())
            {
                DataAdapter.SelectCommand = command;
                Open_Connection(objSqlConnection);
                try
                {
                    DataAdapter.Fill(dataTable);
                }
                catch (Exception e)
                {
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(Return DataTable) Error: {0}", e.InnerException);
                }
                finally
                {
                    command.Dispose();
                }
            }
        }
        Close_Connection();
        return dataTable;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format using SqlTransaction
    /// Used only when we have to return records in single table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <returns></returns>
    public DataTable ReturnDataTableWithTran(string procedureName)
    {
        DataTable dataTable = new DataTable();
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            using (SqlDataAdapter DataAdapter = new SqlDataAdapter())
            {
                DataAdapter.SelectCommand = command;
                Open_Connection(objSqlConnection);
                using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
                    try
                    {
                        DataAdapter.SelectCommand.Transaction = Transaction;
                        DataAdapter.Fill(dataTable);
                        Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Transaction.Rollback();
                        log = new GoldLogging();
                        log.SendErrorToText(e);
                        throw new GoldException("Data Execution(Return DataTable With Tran) Error: {0}", e.InnerException);
                    }
                    finally
                    {
                        command.Dispose();
                    }
            }
        }
        Close_Connection();
        return dataTable;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format
    /// Used only when we have to return records in single table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public DataTable ReturnDataTableWithParameters(string procedureName, SqlParameter[] parameters)
    {
        DataTable dataTable = new DataTable();
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using (SqlDataAdapter DataAdapter = new SqlDataAdapter())
            {
                DataAdapter.SelectCommand = command;
                Open_Connection(objSqlConnection);
                try
                {
                    DataAdapter.Fill(dataTable);
                }
                catch (Exception e)
                {
                    log = new GoldLogging();
                    log.SendErrorToText(e);
                    throw new GoldException("Data Execution(Return DataTable With Parameters) Error: {0}", e.InnerException);
                }
                finally
                {
                    command.Dispose();
                }
            }
        }
        Close_Connection();
        return dataTable;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format using SqlTransaction
    /// Used only when we have to return records in single table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public DataTable ReturnDataTableWithParametersWithTran(string procedureName, SqlParameter[] parameters)
    {
        DataTable dataTable = new DataTable();
        using (SqlConnection objSqlConnection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using (SqlDataAdapter DataAdapter = new SqlDataAdapter())
            {
                DataAdapter.SelectCommand = command;
                Open_Connection(objSqlConnection);
                using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
                    try
                    {
                        DataAdapter.SelectCommand.Transaction = Transaction;
                        DataAdapter.Fill(dataTable);
                        Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Transaction.Rollback();
                        log = new GoldLogging();
                        log.SendErrorToText(e);
                        throw new GoldException("Data Execution(Return DataTable With Parameters With Tran) Error: {0}", e.InnerException);
                    }
                    finally
                    {
                        command.Dispose();
                    }
            }
        }
        Close_Connection();
        return dataTable;
    }
    #endregion
}