using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using bookStore.Models;
using Newtonsoft.Json.Linq;
using System.Reflection;


namespace bookStore.Controllers
{
    public class MyBookController : Controller
    {
        public readonly IConfiguration _config;
        private string cadena;


        public MyBookController(IConfiguration _config)
        {
            this._config = _config;
            this.cadena = _config["ConnectionStrings:connection"];
        }

        

        public IActionResult Index()
        {
            return View();
        }

        /*
        public IActionResult Listar()
        {
            List<MyBookList> myBooks = new List<MyBookList>();

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAllMyBooks", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    myBooks.Add(new MyBookList()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Author = reader.GetString(2),
                        Price = reader.GetDecimal(3)
                    });
                }
            }

            return View(myBooks);
        }
        */
        public IActionResult Listar()
        {
            IEnumerable<MyBookList> myBooks = Enumerable.Empty<MyBookList>(); // Inicializa como enumerable vacío

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAllMyBooks", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    myBooks = myBooks.Append(new MyBookList()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Author = reader.GetString(2),
                        Price = reader.GetDecimal(3)
                    });
                }
            }

            return View(myBooks);
        }
    







        [HttpGet]
        public IActionResult Ingresar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ingresar(MyBookList myBook)
        {
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_InsertMyBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", myBook.Name);
                command.Parameters.AddWithValue("@Author", myBook.Author);
                command.Parameters.AddWithValue("@Price", myBook.Price);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Listar","MyBook");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetMyBookById", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    MyBookList myBook = new MyBookList()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Author = reader.GetString(2),
                        Price = reader.GetDecimal(3)
                    };

                    return View(myBook);
                }
            }

            return RedirectToAction("Listar");
        }

        [HttpPost]
        public IActionResult Editar(MyBookList myBook)
        {
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_UpdateMyBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", myBook.Id);
                command.Parameters.AddWithValue("@Name", myBook.Name);
                command.Parameters.AddWithValue("@Author", myBook.Author);
                command.Parameters.AddWithValue("@Price", myBook.Price);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Listar");
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_DeleteMyBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Listar");
        }
    }
}
