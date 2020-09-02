using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KT.Core.Mvc.Models;
using Dapper;

namespace KT.Core.Mvc.Business
{
    public class Post
    {
        public IDbConnection GetConnection(IDbConnection connection, string connectionString)
        {
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
            }

            return connection;
        }

        public List<kt_wp_post> GetAllPosts(string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            List<kt_wp_post> result = null;

            var _connection = GetConnection(connection, connectionString);

            var sQuery = "SELECT TOP 10 * FROM kt_wp_posts WHERE (post_status = @post_status) ";

            result = _connection.Query<kt_wp_post>(sQuery, new
            {
                post_status = "publish"
            }, transaction: transaction).ToList();

            return result;
        }
    }
}
