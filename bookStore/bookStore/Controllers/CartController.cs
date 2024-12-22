using bookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System.Data;
namespace cartController.Controllers
{
    public class CartController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string cadena;
        private const string CartSessionKey = "ShoppingCart";

        public CartController(IConfiguration config)
        {
            _config = config;
            cadena = _config["ConnectionStrings:connection"];
        }

        private List<Book> GetCartItems()
        {
            var cart = HttpContext.Session.Get<List<Book>>(CartSessionKey);
            return cart ?? new List<Book>();
        }

        private void SaveCartItems(List<Book> cart)
        {
            HttpContext.Session.Set(CartSessionKey, cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int bookId)
        {
            try
            {
                var cart = GetCartItems();
                Book book = null;

                using (var connection = new NpgsqlConnection(cadena))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM sp_GetBookById(@BookId)", connection))
                    {
                        command.Parameters.AddWithValue("BookId", bookId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                book = new Book
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Author = reader.GetString(2),
                                    Price = reader.GetDecimal(3),
                                    ImageUrl = reader.IsDBNull(4) ? null : reader.GetString(4)
                                };
                            }
                        }
                    }
                }

                if (book != null)
                {
                    cart.Add(book);
                    SaveCartItems(cart);
                }

                return PartialView("_CartContent", cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al acceder a la base de datos: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int bookId)
        {
            var cart = GetCartItems();
            var book = cart.FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                cart.Remove(book);
                SaveCartItems(cart);
            }

            return PartialView("_CartContent", cart);
        }

        [HttpGet]
        public IActionResult Cart()
        {
            var cart = GetCartItems();
            return PartialView("_CartSidebar", cart);
        }
        [HttpGet]
        public IActionResult ShoppingCart()
        {
            var cart = GetCartItems();
            return View(cart);
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int bookId, int quantity)
        {
            var cart = GetCartItems();
            var book = cart.FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                // Since we're storing individual items, we need to handle quantity manually
                if (quantity > 1)
                {
                    // Add additional copies
                    for (int i = 1; i < quantity; i++)
                    {
                        cart.Add(book);
                    }
                }
                else if (quantity == 0)
                {
                    // Remove all copies of this book
                    cart.RemoveAll(b => b.Id == bookId);
                }

                SaveCartItems(cart);
            }

            return Json(new
            {
                success = true,
                total = cart.Sum(b => b.Price).ToString("C"),
                itemCount = cart.Count
            });
        }
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCartItems();
            return Json(cart?.Count ?? 0);
        }
        [HttpPost]
        public IActionResult ProcessCheckout([FromBody] CustomerDetails customerDetails)
        {
            try
            {
                var cart = GetCartItems();
                // Here you would typically:
                // 1. Validate the customer details
                // 2. Create an order in your database
                // 3. Process payment
                // 4. Clear the cart

                HttpContext.Session.Remove(CartSessionKey);

                return Json(new { success = true, message = "¡Compra realizada con éxito!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar la compra: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult FinalizarCompra()
        {
            try
            {
                var cart = GetCartItems();
                // Aquí agregarías la lógica para procesar la compra

                // Limpiar el carrito
                HttpContext.Session.Remove(CartSessionKey);

                return RedirectToAction("GraciasCompra");
            }
            catch (Exception ex)
            {
                // Manejar el error apropiadamente
                return StatusCode(500, "Error al procesar la compra: " + ex.Message);
            }
        }

        public IActionResult GraciasCompra()
        {
            return View();
        }
    }

    // Extensión para manejar la serialización de la sesión
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
