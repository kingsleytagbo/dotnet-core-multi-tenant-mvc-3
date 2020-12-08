﻿using System;
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
    }
}