using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for AccessDB
/// </summary>
public class CRMAccessDB
{
    public CRMAccessDB()
    {

    }

    public static string connectionString
    {
        get
        {
            return ConfigurationManager.ConnectionStrings["CRMConnectionString"].ConnectionString;
        }

    }
    public static string connectionStringforlaboursdb
    {
        get
        {
            return ConfigurationManager.ConnectionStrings["LaborServicesConnectionString"].ConnectionString;
        }

    }

    public static System.Data.DataSet SelectQ(string queryString)
    {
        //Response.Write(queryString);
        //string connectionString = "server=\'(local)\'; trusted_connection=true; database=\'warehouse\'";
        //
        System.Data.IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(connectionString);

        System.Data.IDbCommand dbCommand = new System.Data.SqlClient.SqlCommand();
        dbCommand.CommandText = queryString;
        dbCommand.Connection = dbConnection;

        System.Data.IDbDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter();
        dataAdapter.SelectCommand = dbCommand;
        System.Data.DataSet dataSet = new System.Data.DataSet();
       dataAdapter.Fill(dataSet);

        return dataSet;
    }


    public static System.Data.DataSet SelectQlabourdb(string queryString)
    {
        //Response.Write(queryString);
        //string connectionString = "server=\'(local)\'; trusted_connection=true; database=\'warehouse\'";
        //
        System.Data.IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(connectionStringforlaboursdb);

        System.Data.IDbCommand dbCommand = new System.Data.SqlClient.SqlCommand();
        dbCommand.CommandText = queryString;
        dbCommand.Connection = dbConnection;

        System.Data.IDbDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter();
        dataAdapter.SelectCommand = dbCommand;
        System.Data.DataSet dataSet = new System.Data.DataSet();
        dataAdapter.Fill(dataSet);

        return dataSet;
    }




    public static System.Data.DataSet SelectQ(System.Data.IDbCommand cmd)
    {
        //Response.Write(queryString);
        //string connectionString = "server=\'(local)\'; trusted_connection=true; database=\'warehouse\'";

        System.Data.IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(connectionString);

        System.Data.IDbCommand dbCommand = cmd;
        // dbCommand.CommandText = queryString;
        dbCommand.Connection = dbConnection;

        System.Data.IDbDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter();
        dataAdapter.SelectCommand = dbCommand;
        System.Data.DataSet dataSet = new System.Data.DataSet();
        dataAdapter.Fill(dataSet);

        return dataSet;
    }
    public static int Update(string queryString)
    {
        //string connectionString = "server=\'(local)\'; user id=\'berbawy\'; password=\'berbawy\'; database=\'PMO\'";

        System.Data.IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(connectionString);


        //string queryString = "UPDATE [Customer] SET NAME=@Name,ProjName=@ProjName WHERE ID=@ID";
        System.Data.IDbCommand dbCommand = new System.Data.SqlClient.SqlCommand();
        dbCommand.CommandText = queryString;
        dbCommand.Connection = dbConnection;



        int rowsAffected = 0;
        dbConnection.Open();
        try
        {

            rowsAffected = dbCommand.ExecuteNonQuery();


        }
        catch (Exception exc)
        {
            throw new ArgumentException(queryString+" "+ exc.ToString());
        }

        finally
        {
            dbConnection.Close();
        }

        return rowsAffected;
    }



    public static object ExecuteScalar(string sql)
    {
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Connection = new SqlConnection(connectionString);
        try
        {
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return obj;
        }
        catch
        {
            return null;
        }
        finally
        {
            cmd.Connection.Close();
        }

    }
    public static object ExecuteScalar(SqlCommand cmd)
    {
        // SqlCommand cmd = new SqlCommand(sql);
        cmd.Connection = new SqlConnection(connectionString);
        try
        {
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return obj;
        }
        catch
        {
            return null;
        }
        finally
        {
            cmd.Connection.Close();
        }

    }

    public static int ExecuteNonQuery(string queryString)
    {

        System.Data.IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(connectionString);


        //string queryString = "UPDATE [Customer] SET NAME=@Name,ProjName=@ProjName WHERE ID=@ID";
        System.Data.IDbCommand dbCommand = new System.Data.SqlClient.SqlCommand();
        dbCommand.CommandText = queryString;
        dbCommand.Connection = dbConnection;



        int rowsAffected = 0;
        dbConnection.Open();
        try
        {
            rowsAffected = dbCommand.ExecuteNonQuery();

        }
        catch (Exception e)
        {
            throw new ArgumentException(queryString+ e.ToString());
        }
        finally
        {
            dbConnection.Close();
        }

        return rowsAffected;
    }



    public static int ExecuteNonQueryLaboursdb(string queryString)
    {

        System.Data.IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(connectionStringforlaboursdb);


        //string queryString = "UPDATE [Customer] SET NAME=@Name,ProjName=@ProjName WHERE ID=@ID";
        System.Data.IDbCommand dbCommand = new System.Data.SqlClient.SqlCommand();
        dbCommand.CommandText = queryString;
        dbCommand.Connection = dbConnection;



        int rowsAffected = 0;
        dbConnection.Open();
        try
        {
            rowsAffected = dbCommand.ExecuteNonQuery();

        }
        catch (Exception e)
        {
            throw new ArgumentException(queryString + e.ToString());
        }
        finally
        {
            dbConnection.Close();
        }

        return rowsAffected;
    }


    public static int ExecuteNonQuery(SqlCommand dbCommand)
    {
        //

        SqlConnection dbConnection = new System.Data.SqlClient.SqlConnection(connectionString);


        //string queryString = "UPDATE [Customer] SET NAME=@Name,ProjName=@ProjName WHERE ID=@ID";
        // System.Data.IDbCommand dbCommand = new System.Data.SqlClient.SqlCommand();
        // dbCommand.CommandText = queryString;
        dbCommand.Connection = dbConnection;



        int rowsAffected = 0;
        dbConnection.Open();
        try
        {
            rowsAffected = dbCommand.ExecuteNonQuery();

        }
        finally
        {
            dbConnection.Close();
        }

        return rowsAffected;
    }
    public static System.Data.SqlClient.SqlDataReader ExecuteReader(string sql)
    {
        //Response.Write(queryString);
        //string connectionString = "server=\'(local)\'; trusted_connection=true; database=\'warehouse\'";
        // string connectionString = Login.connectionString;
        System.Data.SqlClient.SqlConnection dbConnection = new System.Data.SqlClient.SqlConnection(connectionString);

        SqlCommand dbCommand = new System.Data.SqlClient.SqlCommand();
        dbCommand.CommandText = sql;
        dbCommand.Connection = dbConnection;
        dbConnection.Open();
        return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

    }




}