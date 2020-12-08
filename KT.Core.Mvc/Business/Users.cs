using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KT.Core.Mvc.Models;
using Dapper;

namespace KT.Core.Mvc.Business
{
    public class Users
    {
        public static IDbConnection GetConnection(IDbConnection connection, string connectionString)
        {
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
            }

            return connection;
        }

        public static List<wp_user> GetAllPosts(string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            List<wp_user> result = null;

            var _connection = GetConnection(connection, connectionString);

            var sQuery = "SELECT TOP 1000 * FROM wp_post WHERE (post_status = @post_status) ";

            result = _connection.Query<wp_user>(sQuery, new
            {
                post_status = "publish"
            }, transaction: transaction).ToList();

            return result;
        }


        public static wp_user GetPost(string id, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            wp_user result = null;

            var _connection = GetConnection(connection, connectionString);

            var sQuery = "SELECT TOP 10 * FROM wp_users WHERE (post_name = @post_name) ";

            result = _connection.Query<wp_user>(sQuery, new
            {
                post_name = id
            }, transaction: transaction).FirstOrDefault();

            return result;
        }

        public static wp_user Login(string id, string email, string password, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            wp_user result = null;

            var _connection = GetConnection(connection, connectionString);

            var sQuery = "SELECT * FROM wp_user WHERE ( (user_login = @email) AND (user_pass = @password) ) ";

            result = _connection.Query<wp_user>(sQuery, new
            {
                email = email,
                password = password,
            }, transaction: transaction).FirstOrDefault();

            return result;
        }

        public static int? register(string firstname, string lastname, string email, string password, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            int? result = null;

            var _connection = GetConnection(connection, connectionString);
            var username = string.Format("{0}-{1}", firstname.Trim().ToLower(), lastname.Trim().ToLower());
            var user = new wp_user()
            {
                first_name = firstname, last_name = lastname, user_email = email, user_pass = password,
                user_activation_key = Guid.NewGuid().ToString(),
                 display_name = username, user_login = email, user_nicename = username, 
                user_registered = DateTime.Now, user_status = 1, user_url = ""
            };

            result = _connection.Insert<wp_user>(user, transaction: transaction);

            return result;
        }
    }
}
