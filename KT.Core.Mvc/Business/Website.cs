using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KT.Core.Mvc.Models;
using Dapper;

namespace KT.Core.Mvc.Business
{
    public class Website
    {
        public static IDbConnection GetConnection(IDbConnection connection, string connectionString)
        {
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
            }

            return connection;
        }

        public static Boolean IsDatabaseConfigured(string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            var success = false;
            IDbConnection _connection = null;

            //Check SQL Connection
            try
            {
                var connect = GetConnection(connection, connectionString);
                var query = "SELECT Users = (SELECT Count(*) FROM WP_USER), Posts = (SELECT Count(*) FROM WP_POST), Images = (SELECT Count(*) FROM WP_IMAGE)";
                var result = connect.Query<int>(query, new
                {
                }, transaction: transaction).First();
                success = true;
            }
            catch(Exception ex)
            {
                success = false;
                return success;
            }

            /*
            //Check wp_user table
            try
            {
                var connect = GetConnection(connection, connectionString);
                var result = connect.Query<int>("SELECT Count(*) FROM wp_user; ", new
                {
                }, transaction: transaction).First();
            }
            catch(Exception ex)
            {
                return success;
            }

            //Check wp_post table
            try
            
            {
                var connect = GetConnection(connection, connectionString);
                var result = connect.Query<int>("SELECT Count(*) FROM wp_post; ", new
                {
                }, transaction: transaction).First();
            }
            catch
            {
                return success;
            }

            //Check wp_image table
            try
            {
                var connect = GetConnection(connection, connectionString);
                var result = connect.Query<int>("SELECT Count(*) FROM wp_image; ", new
                {
                }, transaction: transaction).First();
            }
            catch
            {
                return success;
            }
            */

            return success;
        }
    }
}
