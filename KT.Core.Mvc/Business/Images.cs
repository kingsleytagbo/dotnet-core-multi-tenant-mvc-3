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

            var sQuery = "SELECT * FROM wp_image ORDER BY ID DESC " +
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

        public static wp_image Get(Int64 id, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            var _connection = GetConnection(connection, connectionString);

            var result = _connection.Get<wp_image>(id, transaction: transaction);

            return result;
        }

        public static Int64 Create(wp_image image, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            var _connection = GetConnection(connection, connectionString);

            var result = _connection.Insert<wp_image>(image, transaction: transaction).Value;

            return result;
        }


        public static Int64? Update(Int64 id, wp_image image, string connectionString, IDbConnection connection, IDbTransaction transaction)
        {
            Int64? result = null;

            var _connection = GetConnection(connection, connectionString);

            result = _connection.Update<wp_image>(image, transaction: transaction);

            return result;
        }

        public static Int64? Update(Int64 id, string category, string url, string name, string connectionString, IDbConnection connection, IDbTransaction transaction)
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

            var sQuery = "SELECT * FROM wp_image WHERE (name like @name) OR (category like @name)  ORDER BY ID DESC ";

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

        public static wp_image SaveImageFromStream(
           byte [] upload, Int64 id,
            string category, string url, string name,
            string connectionString)
        {
            Int64? parentPostId = null;
            wp_image image = null;

            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(name))
            {
                var post_parent = Posts.GetParentImage(category, connectionString, null, null);
                if (post_parent == null)
                {
                    post_parent = new wp_post()
                    {
                        post_name = category,
                        post_parent = 0,
                        post_content = category,
                        comment_count = 0,
                        guid = Guid.NewGuid().ToString(),
                        post_status = "active",
                        post_type = "image",
                        post_date = DateTime.Now,
                        post_title = category,
                        to_ping = "",
                        post_date_gmt = DateTime.Now,
                        post_modified = DateTime.Now,
                        post_modified_gmt = DateTime.Now,
                        site_id = 1,
                        comment_status = "",
                        post_mime_type = "",
                        post_password = "",
                        pinged = "",
                        ping_status = "",
                        post_author = 0,
                        post_content_filtered = "",
                        post_excerpt = "",
                    };

                    parentPostId = Posts.Create(post_parent, connectionString, null, null);
                }
                else
                {
                    parentPostId = post_parent.ID;
                }

                if (parentPostId.HasValue)
                {
                    if (upload!= null && upload.Length > 0)
                    {
                        image = new wp_image() { category = category, url = url, name = name, site_id = 1, content = upload };
                    }
                    else
                    {
                        image = Images.GetImageBytes(url, new wp_image() { category = category, url = url, name = name, site_id = 1 });
                    }

                    using (var transaction = new System.Transactions.TransactionScope())
                    {
                        image.category = category;

                        // create a new image
                        if (id == 0)
                        {
                            var newImageId = Images.Create(image, connectionString, null, null);

                            var postChild = new wp_post()
                            {
                                post_parent = parentPostId.Value,
                                post_name = newImageId.ToString(),
                                post_content = category,
                                post_category = category,
                                post_status = "active",
                                post_type = "image",
                                post_date = DateTime.Now,
                                post_title = category,

                                comment_status = "",
                                post_mime_type = "",
                                guid = Guid.NewGuid().ToString(),
                                post_author = 0,
                                post_content_filtered = "",
                                post_excerpt = "",

                                to_ping = "",
                                post_password = "",
                                pinged = "",
                                ping_status = "",
                                post_date_gmt = DateTime.Now,
                                post_modified = DateTime.Now,
                                post_modified_gmt = DateTime.Now,
                                comment_count = 0,
                                site_id = 1,
                            };

                            var newChildPostId = Posts.Create(postChild, connectionString, null, null);

                            if (newImageId > 0 && newChildPostId > 0)
                            {
                                transaction.Complete();
                            }
                        }
                        else
                        {
                            // update an existing image
                            var existing = Images.Get(id, connectionString, null, null);
                            existing.name = name;
                            existing.category = category;
                            existing.url = url;
                            existing.content = upload;
                            Images.Update(id, existing, connectionString, null, null);

                            transaction.Complete();
                        }
                    }
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
