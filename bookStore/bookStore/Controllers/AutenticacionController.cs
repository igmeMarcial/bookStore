using bookStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Security.Claims;

namespace bookStore.Controllers
{
    public class AutenticacionController : Controller
    {
        private readonly IConfiguration _IConfig;
        public AutenticacionController(IConfiguration iConfig)
        {
            _IConfig = iConfig;

        }
        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            ClassUsuarioModel usu = new ClassUsuarioModel();
            return View(usu);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Index(ClassUsuarioModel reg)
        {
            string mensaje = "";
            using (NpgsqlConnection cn = new NpgsqlConnection(_IConfig["ConnectionStrings:connection"]))
            {
                if (string.IsNullOrEmpty(reg.Usuario) || string.IsNullOrEmpty(reg.Password))
                {
                    ModelState.AddModelError("", "Ingresar los datos solicitados");
                }
                else
                {
                    try
                    {
                        cn.Open();

                        // Primero validamos si el usuario ya existe y sus credenciales son correctas
                        NpgsqlCommand cmdValidar = new NpgsqlCommand("SELECT sp_ValidarUsuario(@p_correo, @p_contrasena)", cn);
                        cmdValidar.Parameters.AddWithValue("@p_correo", reg.Usuario);
                        cmdValidar.Parameters.AddWithValue("@p_contrasena", reg.Password);

                        // Ejecutar la función sp_ValidarUsuario
                        int idUsuario = (int)cmdValidar.ExecuteScalar();
                        Console.WriteLine($"ID Usuario validado: {idUsuario}");
                        if (idUsuario > 0) // Usuario válido
                        {
                            Console.WriteLine("Usuario validado correctamente.");
                            // Agregar claims (notificadores)
                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, reg.Usuario),
                        new Claim(ClaimTypes.Role, "Usuario") // Aquí puedes agregar un rol si lo tienes
                    };

                            ViewBag.mensaje = "Bienvenido, " + reg.Usuario;

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                            // Redireccionar a la página de inicio
                            // return RedirectToAction("Index", "Home");
                            return Redirect("/");
                        }
                        else
                        {
                            Console.WriteLine("Datos ingresados no son válidos.");
                            ModelState.AddModelError("", "Datos ingresados no son válidos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al abrir la conexión: {ex.Message}");
                        mensaje = ex.Message;
                    }
                }
            }

            // Mostrar mensaje de error si ocurre un problema
            ViewBag.mensaje = mensaje;
            return View();
        }

        [Route("registrar")]
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }
        [Route("registrar")]
        [HttpPost]
        public async Task<IActionResult> Registrar(ClassUsuarioModel reg)
        {
            string mensaje = "";
            using (NpgsqlConnection cn = new NpgsqlConnection(_IConfig["ConnectionStrings:connection"]))
            {
                if (string.IsNullOrEmpty(reg.Usuario) || string.IsNullOrEmpty(reg.Password))
                {
                    ModelState.AddModelError("", "Ingresar los datos solicitados");
                }
                else
                {
                    try
                    {
                        cn.Open();
                        NpgsqlCommand cmdRegistrar = new NpgsqlCommand("SELECT * FROM sp_RegistrarUsuario(@p_correo, @p_contrasena)", cn);
                        cmdRegistrar.Parameters.AddWithValue("@p_correo", reg.Usuario);
                        cmdRegistrar.Parameters.AddWithValue("@p_contrasena", reg.Password);
                        using (var dr = cmdRegistrar.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                bool registrado = dr.GetBoolean(0);
                                string mensajeDeRespuesta = dr.GetString(1);
                                Console.WriteLine($"Usuario registrado: {registrado}, Mensaje: {mensajeDeRespuesta}");
                                if (!registrado)
                                {
                                    ModelState.AddModelError("", mensajeDeRespuesta);
                                    return View();
                                }
                            }
                        }

                        return RedirectToAction("Index", "Autenticacion");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al registrar usuario: {ex.Message}");
                        mensaje = ex.Message;
                    }
                }
            }

            // Mostrar mensaje de error si ocurre un problema
            ViewBag.mensaje = mensaje;
            return View();
        }

        [Route("logout")]
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Autenticacion");
        }
        [Route("mensaje")]
        public async Task<IActionResult> Mensaje()
        {
            return View("Mensaje");

        }
    }
}
