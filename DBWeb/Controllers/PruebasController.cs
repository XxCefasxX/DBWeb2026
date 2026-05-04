using Microsoft.AspNetCore.Mvc;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace DBWeb.Controllers
{
    public class PruebasController : Controller
    {
        string connStr = "server=127.0.0.1;uid=root;pwd=123456;database=tienda";
        public IActionResult Index()
        {
            var categorias = new List<Categoria>
            {
                new Categoria { IDCategoria = 1, Nombre = "Verduras" },
                new Categoria { IDCategoria = 2, Nombre = "Carnes" },
                new Categoria { IDCategoria = 3, Nombre = "Lacteos" },
                new Categoria { IDCategoria = 4, Nombre = "Abarrotes" },
            };
            ViewData["Categorias"] = categorias;
            return View();
        }

        [HttpPost]
        public IActionResult Enviar(string nombre, int cantidad, decimal precio, string categoriaID)
        {
            
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;pwd=123456;database=tienda");
            MySqlCommand comm = new MySqlCommand("insert into pruebas(nombre,cantidad,precio) values(@nombre,@cantidad,@precio);",conn);
            comm.Parameters.AddWithValue("@nombre",nombre);
            comm.Parameters.AddWithValue("@cantidad",cantidad);
            comm.Parameters.AddWithValue("@precio",precio);
            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                ViewData["Mensaje"] = $"¡Producto '{nombre}' guardado con éxito! Cantidad: {cantidad}, Precio: {precio}";
            }
            catch(Exception ex)
            {
                ViewData["Error"] = $"¡Error, el producto '{nombre}' no pudo ser registrado,  {ex.Message}";
            }
            finally
            {
                conn.Close();
            }

            return View("Index"); 
        }

        [HttpPost]
        public IActionResult GuardarCliente(string nombre, int cantidad, decimal precio)
        {


            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand comm = new MySqlCommand("insert into pruebas(nombre,cantidad,precio) values(@nombre,@cantidad,@precio);", conn);
            comm.Parameters.AddWithValue("@nombre", nombre);
            comm.Parameters.AddWithValue("@cantidad", cantidad);
            comm.Parameters.AddWithValue("@precio", precio);
            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                ViewData["Mensaje"] = $"¡Producto '{nombre}' guardado con éxito! Cantidad: {cantidad}, Precio: {precio}";
            }
            catch (Exception ex)
            {
                ViewData["Error"] = $"¡Error, el producto '{nombre}' no pudo ser registrado,  {ex.Message}";
            }
            finally
            {
                conn.Close();
            }

            return View("Index");
        }
        public IActionResult Lissta()
        {
            var categorias = new List<Categoria>
            {
                new Categoria { IDCategoria = 1, Nombre = "Verduras" },
                new Categoria { IDCategoria = 2, Nombre = "Carnes" },
                new Categoria { IDCategoria = 3, Nombre = "Lacteos" },
                new Categoria { IDCategoria = 4, Nombre = "Abarrotes" },
            };
            ViewData["Categorias"] = categorias;

            List<Producto> productos = new List<Producto>();

            MySqlConnection conect = new MySqlConnection(connStr);
            MySqlCommand comand = new MySqlCommand("select * from productos;", conect);
            try
            {
                conect.Open();
                MySqlDataReader dr = comand.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Producto producto = new Producto
                        {
                            Nombre = dr["nombre"].ToString(),
                            Categoria = dr["id_categoria"].ToString(),
                            Descripcion= dr["Descripcion"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"].ToString())
                        };
                        productos.Add(producto);
                    }
                }
                dr.Close();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                conect.Close();
            }

            ViewData["Productos"] = productos;
            return View();
        }

        public IActionResult Buscar(int categoriaId)
        {
            List<Producto> productos = new List<Producto>();

            MySqlConnection conect = new MySqlConnection(connStr);
            MySqlCommand comand = new MySqlCommand("select * from productos where id_categoria=@categoria;", conect);
            comand.Parameters.AddWithValue("@categoria", categoriaId);
            try
            {
                conect.Open();
                MySqlDataReader dr = comand.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Producto producto = new Producto
                        {
                            Nombre = dr["nombre"].ToString(),
                            Categoria = dr["id_categoria"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            Precio = Convert.ToDecimal(dr["Precio"].ToString())
                        };
                        productos.Add(producto);
                    }
                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conect.Close();
            }

            ViewData["Productos"] = productos;
            return View("Lissta");
        }


    }

   
}
public class Producto
{
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public string Categoria { get; set; }
    public string Descripcion { get; set; }

}


public class Categoria
{
    public int IDCategoria { get; set; }
    public string Nombre { get; set; }

}