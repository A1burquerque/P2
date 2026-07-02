using System;
using System.Globalization;
using System.Linq;

namespace AgendaElectronica
{
    class Program
    {
        static Agenda agenda = new Agenda();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool salir = false;

            while (!salir)
            {
                MostrarMenu();
                string? opcion = Console.ReadLine();
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        InsertarRegistro();
                        break;
                    case "2":
                        ModificarRegistro();
                        break;
                    case "3":
                        BuscarRegistro();
                        break;
                    case "4":
                        EliminarRegistro();
                        break;
                    case "5":
                        ListarTodos();
                        break;
                    case "0":
                        salir = true;
                        Console.WriteLine("Gracias por usar la Agenda Electronica. Hasta pronto.");
                        break;
                    default:
                        Console.WriteLine("Opcion no valida. Intente nuevamente.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=======================================");
            Console.WriteLine("        AGENDA ELECTRONICA");
            Console.WriteLine("=======================================");
            Console.WriteLine($"Registros almacenados: {agenda.Cantidad}");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("1. Insertar nuevo registro");
            Console.WriteLine("2. Modificar un registro");
            Console.WriteLine("3. Buscar un registro");
            Console.WriteLine("4. Eliminar un registro");
            Console.WriteLine("5. Listar todos los registros");
            Console.WriteLine("0. Salir");
            Console.WriteLine("=======================================");
            Console.Write("Seleccione una opcion: ");
        }

        // -----------------------------------------------------------
        // 1. INSERTAR
        // -----------------------------------------------------------
        static void InsertarRegistro()
        {
            Console.WriteLine("--- INSERTAR NUEVO REGISTRO ---\n");

            string nombre = LeerTexto("Nombre: ");
            string apellido = LeerTexto("Apellido: ");
            DateTime fechaNacimiento = LeerFecha("Fecha de nacimiento (dd/MM/yyyy): ");
            string direccion = LeerTexto("Direccion: ");
            Genero genero = LeerGenero();
            EstadoCivil estadoCivil = LeerEstadoCivil();
            string movil = LeerTexto("Movil: ");
            string telefono = LeerTexto("Telefono: ");
            string correo = LeerTexto("Correo electronico: ");

            var contacto = agenda.Insertar(nombre, apellido, fechaNacimiento, direccion,
                                            genero, estadoCivil, movil, telefono, correo);

            Console.WriteLine($"\nRegistro creado exitosamente con ID: {contacto.Id}");
        }

        // -----------------------------------------------------------
        // 2. MODIFICAR
        // -----------------------------------------------------------
        static void ModificarRegistro()
        {
            Console.WriteLine("--- MODIFICAR REGISTRO ---\n");

            int id = LeerEntero("Ingrese el ID del registro a modificar: ");
            var contacto = agenda.BuscarPorId(id);

            if (contacto == null)
            {
                Console.WriteLine("No se encontro ningun registro con ese ID.");
                return;
            }

            Console.WriteLine("\nDatos actuales:");
            Console.WriteLine(contacto);
            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para conservar el valor actual):\n");

            string nombre = LeerTextoOpcional("Nombre", contacto.Nombre);
            string apellido = LeerTextoOpcional("Apellido", contacto.Apellido);
            DateTime fechaNacimiento = LeerFechaOpcional("Fecha de nacimiento (dd/MM/yyyy)", contacto.FechaNacimiento);
            string direccion = LeerTextoOpcional("Direccion", contacto.Direccion);
            Genero genero = LeerGeneroOpcional(contacto.Genero);
            EstadoCivil estadoCivil = LeerEstadoCivilOpcional(contacto.EstadoCivil);
            string movil = LeerTextoOpcional("Movil", contacto.Movil);
            string telefono = LeerTextoOpcional("Telefono", contacto.Telefono);
            string correo = LeerTextoOpcional("Correo electronico", contacto.CorreoElectronico);

            agenda.Modificar(id, nombre, apellido, fechaNacimiento, direccion,
                              genero, estadoCivil, movil, telefono, correo);

            Console.WriteLine("\nRegistro modificado exitosamente.");
        }

        // -----------------------------------------------------------
        // 3. BUSCAR
        // -----------------------------------------------------------
        static void BuscarRegistro()
        {
            Console.WriteLine("--- BUSCAR REGISTRO ---\n");
            Console.WriteLine("1. Buscar por ID");
            Console.WriteLine("2. Buscar por texto (nombre, apellido, movil, telefono o correo)");
            Console.Write("Seleccione una opcion: ");
            string? opcion = Console.ReadLine();
            Console.WriteLine();

            if (opcion == "1")
            {
                int id = LeerEntero("Ingrese el ID: ");
                var contacto = agenda.BuscarPorId(id);
                if (contacto == null)
                    Console.WriteLine("No se encontro ningun registro con ese ID.");
                else
                    Console.WriteLine(contacto);
            }
            else if (opcion == "2")
            {
                string texto = LeerTexto("Ingrese el texto a buscar: ");
                var resultados = agenda.Buscar(texto);

                if (resultados.Count == 0)
                {
                    Console.WriteLine("No se encontraron coincidencias.");
                }
                else
                {
                    Console.WriteLine($"\nSe encontraron {resultados.Count} coincidencia(s):\n");
                    foreach (var c in resultados)
                        Console.WriteLine(c);
                }
            }
            else
            {
                Console.WriteLine("Opcion no valida.");
            }
        }

        // -----------------------------------------------------------
        // 4. ELIMINAR
        // -----------------------------------------------------------
        static void EliminarRegistro()
        {
            Console.WriteLine("--- ELIMINAR REGISTRO ---\n");

            int id = LeerEntero("Ingrese el ID del registro a eliminar: ");
            var contacto = agenda.BuscarPorId(id);

            if (contacto == null)
            {
                Console.WriteLine("No se encontro ningun registro con ese ID.");
                return;
            }

            Console.WriteLine("\nRegistro a eliminar:");
            Console.WriteLine(contacto);
            Console.Write("\n¿Confirma que desea eliminar este registro? (S/N): ");
            string? confirmacion = Console.ReadLine();

            if (confirmacion != null && confirmacion.Trim().ToUpper() == "S")
            {
                agenda.Eliminar(id);
                Console.WriteLine("Registro eliminado exitosamente.");
            }
            else
            {
                Console.WriteLine("Operacion cancelada.");
            }
        }

        // -----------------------------------------------------------
        // Listar todos (funcionalidad extra de apoyo)
        // -----------------------------------------------------------
        static void ListarTodos()
        {
            Console.WriteLine("--- LISTADO DE REGISTROS ---\n");
            var todos = agenda.ListarTodos();

            if (todos.Count == 0)
            {
                Console.WriteLine("No hay registros almacenados.");
                return;
            }

            foreach (var c in todos)
                Console.WriteLine(c);
        }

        // -----------------------------------------------------------
        // Metodos auxiliares de lectura y validacion de datos
        // -----------------------------------------------------------
        static string LeerTexto(string mensaje)
        {
            string? valor;
            do
            {
                Console.Write(mensaje);
                valor = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(valor));

            return valor.Trim();
        }

        static string LeerTextoOpcional(string etiqueta, string valorActual)
        {
            Console.Write($"{etiqueta} [{valorActual}]: ");
            string? valor = Console.ReadLine();
            return string.IsNullOrWhiteSpace(valor) ? valorActual : valor.Trim();
        }

        static DateTime LeerFecha(string mensaje)
        {
            DateTime fecha;
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();
                if (DateTime.TryParseExact(entrada, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out fecha))
                {
                    return fecha;
                }
                Console.WriteLine("Formato de fecha invalido. Use dd/MM/yyyy.");
            }
        }

        static DateTime LeerFechaOpcional(string etiqueta, DateTime valorActual)
        {
            Console.Write($"{etiqueta} [{valorActual:dd/MM/yyyy}]: ");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
                return valorActual;

            if (DateTime.TryParseExact(entrada, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime fecha))
            {
                return fecha;
            }

            Console.WriteLine("Formato invalido, se conserva el valor actual.");
            return valorActual;
        }

        static int LeerEntero(string mensaje)
        {
            int valor;
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();
                if (int.TryParse(entrada, out valor))
                    return valor;

                Console.WriteLine("Debe ingresar un numero valido.");
            }
        }

        static Genero LeerGenero()
        {
            Console.WriteLine("Genero:");
            Console.WriteLine("  1. Masculino");
            Console.WriteLine("  2. Femenino");
            Console.WriteLine("  3. Otro");

            while (true)
            {
                Console.Write("Seleccione una opcion: ");
                string? entrada = Console.ReadLine();
                switch (entrada)
                {
                    case "1": return Genero.Masculino;
                    case "2": return Genero.Femenino;
                    case "3": return Genero.Otro;
                    default:
                        Console.WriteLine("Opcion no valida.");
                        break;
                }
            }
        }

        static Genero LeerGeneroOpcional(Genero valorActual)
        {
            Console.WriteLine($"Genero actual: {valorActual}");
            Console.WriteLine("  1. Masculino  2. Femenino  3. Otro  (Enter = conservar)");
            Console.Write("Seleccione una opcion: ");
            string? entrada = Console.ReadLine();

            return entrada switch
            {
                "1" => Genero.Masculino,
                "2" => Genero.Femenino,
                "3" => Genero.Otro,
                _ => valorActual
            };
        }

        static EstadoCivil LeerEstadoCivil()
        {
            Console.WriteLine("Estado civil:");
            Console.WriteLine("  1. Soltero/a");
            Console.WriteLine("  2. Casado/a");
            Console.WriteLine("  3. Divorciado/a");
            Console.WriteLine("  4. Viudo/a");

            while (true)
            {
                Console.Write("Seleccione una opcion: ");
                string? entrada = Console.ReadLine();
                switch (entrada)
                {
                    case "1": return EstadoCivil.Soltero;
                    case "2": return EstadoCivil.Casado;
                    case "3": return EstadoCivil.Divorciado;
                    case "4": return EstadoCivil.Viudo;
                    default:
                        Console.WriteLine("Opcion no valida.");
                        break;
                }
            }
        }

        static EstadoCivil LeerEstadoCivilOpcional(EstadoCivil valorActual)
        {
            Console.WriteLine($"Estado civil actual: {valorActual}");
            Console.WriteLine("  1. Soltero/a  2. Casado/a  3. Divorciado/a  4. Viudo/a  (Enter = conservar)");
            Console.Write("Seleccione una opcion: ");
            string? entrada = Console.ReadLine();

            return entrada switch
            {
                "1" => EstadoCivil.Soltero,
                "2" => EstadoCivil.Casado,
                "3" => EstadoCivil.Divorciado,
                "4" => EstadoCivil.Viudo,
                _ => valorActual
            };
        }
    }
}