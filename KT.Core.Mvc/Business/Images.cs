using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Net;

using KT.Core.Mvc.Models;
using Dapper;

namespace KT.Core.Mvc.Business
{
    public class Images
    {
        public static IDbConnection GetConnection(IDbConnection connection, string connectionString)
        {
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
            }

            return connection;
        }

        public static List<wp_image> GetAll(string connectionString, int page = 1, int pageSize = 1, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            List<wp_image> result = null;

            var _connection = GetConnection(connection, connectionString);

            var sQuery = "SELECT * FROM wp_image ORDER BY ID " +
                " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            result = _connection.Query<wp_image>(sQuery, new
            {
                Offset = (page - 1) * pageSize,
                PageSize = pageSize,
            }, transaction: transaction).ToList();

            return result;
        }

        public static int GetTotal(string connectionString,IDbConnection connection = null, IDbTransaction transaction = null)
        {
            int result = 0;

            var _connection = GetConnection(connection, connectionString);

            var sQuery = "SELECT Count(*) FROM wp_image; ";

            result = _connection.Query<int>(sQuery, new
            {
            }, transaction: transaction).First();

            return result;
        }

        public static Int64 Create(wp_image image, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            var _connection = GetConnection(connection, connectionString);

            var result = _connection.Insert<wp_image>(image, transaction: transaction).Value;

            return result;
        }

        public static Int64? Update(int id, string category, string url, string name, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            Int64? result = null;

            var _connection = GetConnection(connection, connectionString);

            wp_image image = _connection.Get<wp_image>(id, transaction: transaction);
            image.name = name;
            image.category = category;
            if(url != image.url)
            {
                image.content = GetImageBytesFromUrl(url);
            }

            result = _connection.Update<wp_image>(image, transaction: transaction);

            return result;
        }

        public static int ? Delete(int id, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            int ? result= null;

            var _connection = GetConnection(connection, connectionString);

            wp_image image = _connection.Get<wp_image>(id, transaction: transaction);

            if(image != null)
            {
                result = _connection.Delete<wp_image>(id, transaction: transaction);
            }

            return result;
        }

        public static List<wp_image> Search(string name, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            List<wp_image> result = null;

            var _connection = GetConnection(connection, connectionString);

            var sQuery = "SELECT * FROM wp_image WHERE name like @name ";

            result = _connection.Query<wp_image>(sQuery, new
            {
                name = "%" + name + "%"
            }, transaction: transaction).ToList();

            return result;
        }

        public static wp_image DownloadImage(Tenant setting, Guid id, Guid entityId, string sourceUrl,
      string fileFolder = "gallery")
        {
            string publishUrl = string.Empty;
            string fileName = string.Empty;
            string filePath = string.Empty;
            wp_image image = null;

            if (!string.IsNullOrEmpty(sourceUrl))
            {
                string[] fileSizes = { "850x450", "210x210" };
                var fileExtension = string.Empty;

                fileExtension = System.IO.Path.GetExtension(sourceUrl);

                if (!string.IsNullOrEmpty(fileExtension))
                {
                    foreach (var fileSize in fileSizes)
                    {
                        fileName = GetCorrectFileName(setting, id, entityId, fileFolder, fileSize + fileExtension);
                        filePath = GetPathAndFilename(setting, id, fileFolder, fileName);
                        var source = GetImageBytesFromUrl(sourceUrl);

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        using (System.IO.FileStream output = System.IO.File.Create(filePath))
                        {
                            output.Write(source);
                        }
                    }

                    publishUrl = string.Format("http://api2.launchfeatures.com/api/images/upload1/{0}/{1}/{2}/{3}{4}", id.ToString(), fileFolder, entityId.ToString(), "850x450", fileExtension);
                    image = new wp_image();
                    image.url = sourceUrl;
                    image.url = publishUrl;
                }

            }

            return image;

        }

        public static wp_image GetImageBytes(string sourceUrl, wp_image image)
        {
            try
            {
                var source = GetImageBytesFromUrl(sourceUrl);
                image.content = source;
            }
            catch (Exception ex) { }
            return image;
        }

        public static string GetCorrectFileName(Tenant setting, Guid site, Guid id, string folder, string name)
        {

            var uploadFolder = setting.ConnectionString;

            var fileName = @"\" + folder.Trim().ToLower() + @"\" + "_" + folder.Trim().ToLower() + "_" + id.ToString() + "_image_" + name;

            fileName = @"\" + site + fileName;

            return fileName;
        }

        public static string GetPathAndFilename(Tenant setting, Guid site, string folder, string filename)
        {
            bool exists = false;

            var uploadFolder = setting.ConnectionString;

            var folderPath = string.Empty;

            try
            {

                exists = Directory.Exists(uploadFolder);
                if (!exists)
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(uploadFolder);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                    }
                }


                // Images are stored in \images\ folder
                // Files are stored in \files\ folder
                folderPath = (uploadFolder + @"\images\");
                exists = Directory.Exists(folderPath);
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                exists = Directory.Exists(folderPath + site + @"\");
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(folderPath + site + @"\");
                }

                exists = Directory.Exists(folderPath + site + @"\");
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(folderPath + site + @"\");
                }

                exists = Directory.Exists(folderPath + site + @"\" + folder + @"\");
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(folderPath + site + @"\" + folder + @"\");
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            var filePath = folderPath + filename;

            return filePath;
        }

        public static byte[] GetImageBytesFromUrl(string url)
        {
            byte[] result = null;
            Stream imageStream = null;

            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(new Uri(url));
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                imageStream = response.GetResponseStream();

                using (var memoryStream = new MemoryStream())
                {
                    imageStream.CopyTo(memoryStream);
                    result = memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return result;
        }
    }
}
