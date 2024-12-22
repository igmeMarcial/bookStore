using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;

using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using bookStore.Models;
using Newtonsoft.Json;




namespace bookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _config;
        private string cadena;

        public BookController(IConfiguration _config)
        {
            this._config = _config;
            this.cadena = _config["ConnectionStrings:connection"];
        }

        public async Task<IActionResult> Inicio()
        {
            if(HttpContext.Session.GetString("carrito") == null)
            {
                HttpContext.Session.SetString("carrito", JsonConvert.SerializeObject(new List<Book>()));
            }
            return View(await Task.Run(() => ListarLibrosEnumerable()));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IEnumerable<Book> ListarLibrosEnumerable()
        {
            List<Book> books = new List<Book>();

            using (var connection = new NpgsqlConnection(cadena))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.sp_getallbooks()", connection))
                {
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Author = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                ImageUrl = reader.IsDBNull(4) ? null : reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return books;
        }

        public IActionResult Listar()
        {
            List<Book> books = new List<Book>();

            using (var connection = new NpgsqlConnection(cadena))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM sp_GetAllBooks()", connection))
                {
                   

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Author = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                ImageUrl = reader.IsDBNull(4) ? null : reader.GetString(4)
                            });
                        }
                    }
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
            using (var connection = new NpgsqlConnection(cadena))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT sp_InsertBook(@Name, @Author, @Price, @ImageUrl)", connection))
                {
                    

                    command.Parameters.AddWithValue("Name", book.Name);
                    command.Parameters.AddWithValue("Author", book.Author);
                    command.Parameters.AddWithValue("Price", book.Price);
                    command.Parameters.AddWithValue("ImageUrl", string.IsNullOrEmpty(book.ImageUrl) ? DBNull.Value : (object)book.ImageUrl);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Listar");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            using (var connection = new NpgsqlConnection(cadena))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM sp_GetBookById(@BookId)", connection))
                {
                  

                    command.Parameters.AddWithValue("BookId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Book book = new Book()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Author = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                ImageUrl = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };

                            return View(book);
                        }
                    }
                }
            }

            return RedirectToAction("Listar");
        }

        [HttpPost]
        public IActionResult Editar(Book book)
        {
            using (var connection = new NpgsqlConnection(cadena))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT sp_UpdateBook(@Id, @Name, @Author, @Price, @ImageUrl)", connection))
                {
                    

                    command.Parameters.AddWithValue("Id", book.Id);
                    command.Parameters.AddWithValue("Name", book.Name);
                    command.Parameters.AddWithValue("Author", book.Author);
                    command.Parameters.AddWithValue("Price", book.Price);
                    command.Parameters.AddWithValue("ImageUrl", (object)book.ImageUrl ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Listar");
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            using (var connection = new NpgsqlConnection(cadena))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT sp_DeleteBook(@Id)", connection))
                {
                   

                    command.Parameters.AddWithValue("Id", id);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Listar");
        }

        //Experimental
        Book buscar(int id)
        {
            Book reg = ListarLibrosEnumerable().Where(p => p.Id == id).FirstOrDefault();
            return reg ?? new Book();
        }

        public async Task<IActionResult> Agregar(int id = 0)
        {
            return View(await Task.Run(() => buscar(id)));
        }
    }
}