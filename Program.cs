using System;
using System.Linq;
using ClassLibrary1.Data;
using ClassLibrary1.Modesl;
using ClassLibrary1.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

class Program
{
    static void Main(string[] args)
    {
        int opcion;
        do
        {
            Console.Clear();
            Console.WriteLine("=== CAFETERÍA - GESTIÓN ===");
            Console.WriteLine("1. Alta categoría");
            Console.WriteLine("2. Alta producto");
            Console.WriteLine("3. Ver productos");
            Console.WriteLine("4. Mostrar productos más caros");
            Console.WriteLine("5. Cantidad total de productos");
            Console.WriteLine("6. Mostrar productos ordenados alfabéticamente por nombre");
            Console.WriteLine("7. Verificar si existen productos registrados");
            Console.WriteLine("0. Salir");
            Console.Write("\nElija una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                opcion = -1;
            }

            using (var db = new CafeteriaDbContext()) 
            {
                switch (opcion)
                {
                    case 1:
                        Console.Write("Ingrese el nombre de la nueva categoría: ");
                        string nomCat = Console.ReadLine() ?? string.Empty;
                        db.Categorias.Add(new Categoria { Nombre = nomCat });
                        db.SaveChanges();
                        Console.WriteLine("¡Categoría registrada con éxito!");
                        break;

                    case 2:
                        Console.Write("Ingrese el nombre del producto: ");
                        string nomProd = Console.ReadLine() ?? String.Empty;
                        Console.Write("Ingrese el precio: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal precio))
                        {
                           
                            var cats = db.Categorias.ToList();
                            if (!cats.Any())
                            {
                                Console.WriteLine("Primero debe registrar al menos una categoría.");
                                break;
                            }

                            Console.WriteLine("\nCategorías disponibles:");
                            foreach (var c in cats)
                            {
                                Console.WriteLine($"ID: {c.Id} - Nombre: {c.Nombre}");
                            }

                            Console.Write("Ingrese el ID de la categoría para este producto: ");
                            if (int.TryParse(Console.ReadLine(), out int catId))
                            {
                                db.Products.Add(new Producto { Nombre = nomProd, Precio = precio, CategoriaId = catId });
                                db.SaveChanges();
                                Console.WriteLine("¡Producto registrado con éxito!");
                            }
                        }
                        break;

                    case 3:
                        var prods = db.Products.ToList();
                        Console.WriteLine("\n--- LISTA DE PRODUCTOS ---");
                        foreach (var p in prods)
                        {
                            Console.WriteLine($"- {p.Nombre} (${p.Precio})");
                        }
                        break;

                    case 4:
                        if (db.Products.Any())
                        {
                            decimal maxPrecio = db.Products.Max(p => p.Precio);
                            var masCaros = db.Products.Where(p => p.Precio == maxPrecio).ToList();
                            Console.WriteLine("\n--- PRODUCTO(S) MÁS CARO(S) ---");
                            foreach (var p in masCaros)
                            {
                                Console.WriteLine($"- {p.Nombre} (${p.Precio})");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay productos registrados.");
                        }
                        break;

                    case 5:
                        int total = db.Products.Count();
                        Console.WriteLine($"\nCantidad total de productos: {total}");
                        break;

                    case 6:
                        var ordenados = db.Products.OrderBy(p => p.Nombre).ToList();
                        Console.WriteLine("\n--- PRODUCTOS ORDENADOS ALFABÉTICAMENTE ---");
                        foreach (var p in ordenados)
                        {
                            Console.WriteLine($"- {p.Nombre} (${p.Precio})");
                        }
                        break;

                    case 7:
                        bool existen = db.Products.Any();
                        if (existen)
                            Console.WriteLine("\nSí, existen productos registrados en el sistema.");
                        else
                            Console.WriteLine("\nNo hay productos registrados actualmente.");
                        break;

                    case 0:
                        Console.WriteLine("Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }

            if (opcion != 0)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 0);
    }
}