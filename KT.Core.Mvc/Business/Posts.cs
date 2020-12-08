﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KT.Core.Mvc.Models;
using Dapper;

namespace KT.Core.Mvc.Business
{
    public class Posts
    {
        public IDbConnection GetConnection(IDbConnection connection, string connectionString)
        {
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
            }

            return connection;
        }

        public List<wp_post> GetAllPosts(string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            List<wp_post> result = null;

            var _connection = GetConnection(connection, connectionString);

            var sQuery = "SELECT TOP 1000 * FROM wp_post WHERE (post_status = @post_status) ";

            result = _connection.Query<wp_post>(sQuery, new
            {
                post_status = "publish"
            }, transaction: transaction).ToList();

            return result;
        }


        public wp_post GetPost(string id, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            wp_post result = null;

            var _connection = GetConnection(connection, connectionString);

            var sQuery = "SELECT TOP 10 * FROM wp_posts WHERE (post_name = @post_name) ";

            result = _connection.Query<wp_post>(sQuery, new
            {
                post_name = id
            }, transaction: transaction).FirstOrDefault();

            return result;
        }
    }
}