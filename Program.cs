using System;
using System.Data;
using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BaltaDataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            const String connectionString = "Server=localhost;Database=balta;User Id=sa;Password=nobre123";


            using (var connection = new SqlConnection(connectionString))
            {
                //UpdateCategory(connection);
                //DeleteCategory(connection);
                //CreateCategory(connection);
                //CreateManyCategorys(connection);
                //GetCategory(connection);
                //ListCategory(connection);
                //ExecuteProcedure(connection);
                //ExecuteReadProcedure(connection);
                ExecuteScalar(connection);
            }
        }

        static void ListCategory(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [ID], [TITLE] FROM [Category]");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void GetCategory(SqlConnection connection)
        {
            var category = connection.QueryFirstOrDefault<Category>("SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id] = @id", new
            {
                id = "dbd7015c-958d-45ca-911a-ae8b1a51c8ab"
            });

            Console.WriteLine($"{category.Id} - {category.Title}");
        }

        static void CreateCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;
            var insertSql = @"INSERT INTO [CATEGORY] VALUES(@Id, @Title, @Url, @Summary, @Order, @Description, @Featured)";

            var rows = connection.Execute(insertSql, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });

            Console.WriteLine($"{rows} linhas inseridas");
        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title] = @title where [Id] = @id";

            var rows = connection.Execute(updateQuery, new
            {
                id = new Guid("e79c318f-7afd-4b87-ba03-98f5f4a330d0"),
                title = "Amazon"
            });

            Console.WriteLine($"{rows} registros atualizados");
        }

        static void DeleteCategory(SqlConnection connection)
        {
            var deleteQuery = "DELETE [Category] WHERE [Id] = @id";

            var rows = connection.Execute(deleteQuery, new
            {
                id = new Guid("d855e8a6-f631-4540-ad21-fbb0f49dc1d8")
            });

            Console.WriteLine($"{rows} registros excluídos");
        }

        static void CreateManyCategorys(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "New Amazon AWS";
            category2.Url = "new amazon";
            category2.Description = "new Categoria destinada a serviços do AWS";
            category2.Order = 8;
            category2.Summary = "new AWS";
            category2.Featured = true;

            var insertSql = @"INSERT INTO [CATEGORY] VALUES(@Id, @Title, @Url, @Summary, @Order, @Description, @Featured)";

            var rows = connection.Execute(insertSql, new[]
            {
                new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                },
                new
                {
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured
                }
            });

            Console.WriteLine($"{rows} linhas inseridas");
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            var procedure = "spDeleteStudent";
            var pars = new { StudentId = "ea1eb6bb-580c-4ec1-ae77-4e7ba97ea4e7" };

            var affectedRows = connection.Execute(procedure, pars, commandType: CommandType.StoredProcedure);

            Console.WriteLine($"{affectedRows} linhas inseridas");
        }

        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "spGetCoursesByCategory";
            var pars = new { CategoryId = "b4c5af73-7e02-4ff7-951c-f69ee1729cac" };

            var courses = connection.Query(procedure, pars, commandType: CommandType.StoredProcedure);
            foreach (var item in courses)
            {
                if (item != null)
                {
                    Console.WriteLine(item.Title);
                }
                else
                {
                    Console.WriteLine("Tem é nada aqui");
                }
            }
        }

        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "JBL";
            category.Url = "amazon/";
            category.Description = "Categoria destinada a serviços da JBL";
            category.Order = 8;
            category.Summary = "JBL Phone";
            category.Featured = false;
            var insertSql = @"INSERT INTO 
                                        [Category]
                                        OUTPUT inserted.[Id]
                                     VALUES
                                        (NEWID(), @Title, @Url, @Summary, @Order, @Description, @Featured) 
                                        SELECT SCOPE_IDENTITY()";

            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });

            Console.WriteLine($"A categoria inserida tem o id: {id}");
        }

        
    }
}