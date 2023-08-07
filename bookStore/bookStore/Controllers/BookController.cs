using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using bookStore.Models;
using Newtonsoft.Json;




namespace bookStore.Controllers
{
    public class BookController : Controller
    {
        public readonly IConfiguration _config;
        private string cadena;


        public BookController(IConfiguration _config)
        {
            this._config = _config;
            this.cadena = _config["ConnectionStrings:connection"];
        }
        public async Task<IActionResult> Inicio()
        {
            if(HttpContext.Session.GetString("carrito")== null)
            {
                HttpContext.Session.SetString("carrito",JsonConvert.SerializeObject(new List<Book>()));
            }
            return View(await Task.Run(()=> ListarLibrosEnumerable()));
        }


        public IActionResult Index()
        {
            return View();
        }


        public IEnumerable<Book> ListarLibrosEnumerable()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAllBooks", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    books.Add(new Book()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Author = reader.GetString(2),
                        Price = reader.GetDecimal(3)
                    });
                }
            }

            return books;
        }

        public IActionResult Listar()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAllBooks", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    books.Add(new Book()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Author = reader.GetString(2),
                        Price = reader.GetDecimal(3)
                    });
                }
            }

            return View(books);
        }
        public IActionResult Ingresar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ingresar(Book book)
        {
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_InsertBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", book.Name);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Price", book.Price);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Listar");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetBookById", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Book book = new Book()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Author = reader.GetString(2),
                        Price = reader.GetDecimal(3)
                    };

                    return View(book);
                }
            }

            return RedirectToAction("Listar");
        }
        [HttpPost]
        public IActionResult Editar(Book book)
        {
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_UpdateBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", book.Id);
                command.Parameters.AddWithValue("@Name", book.Name);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Price", book.Price);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Listar");
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetBookById", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Book book = new Book()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Author = reader.GetString(2),
                        Price = reader.GetDecimal(3)
                    };

                    return View(book);
                }
            }

            return RedirectToAction("Listar");
        }

        [HttpPost]
        public IActionResult EliminarConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_DeleteBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Listar");
        }
        Book buscar(int id)
        {
            Book reg= ListarLibrosEnumerable().Where(p=>p.Id == id).FirstOrDefault();
            if(reg == null)
            {
                reg = new Book();

            }
            return reg;
        }
        public async Task<IActionResult> Agregar(int id = 0)
        {
            return View(await Task.Run(() => buscar(id)));
        }
    }
}
