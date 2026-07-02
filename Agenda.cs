using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AgendaElectronica
{
    /// <summary>
    /// Contiene toda la logica de negocio de la agenda:
    /// insertar, modificar, buscar y eliminar registros.
    /// Persiste los datos en un archivo JSON para que no se pierdan
    /// al cerrar el programa.
    /// </summary>
    public class Agenda
    {
        private readonly string _rutaArchivo;
        private List<Contacto> _contactos;
        private int _siguienteId;

        public Agenda(string rutaArchivo = "agenda.json")
        {
            _rutaArchivo = rutaArchivo;
            _contactos = new List<Contacto>();
            CargarDesdeArchivo();
            _siguienteId = _contactos.Count > 0 ? _contactos.Max(c => c.Id) + 1 : 1;
        }

        // ---------------------------------------------------------
        // 1. INSERTAR NUEVO REGISTRO
        // ---------------------------------------------------------
        public Contacto Insertar(
            string nombre,
            string apellido,
            DateTime fechaNacimiento,
            string direccion,
            Genero genero,
            EstadoCivil estadoCivil,
            string movil,
            string telefono,
            string correoElectronico)
        {
            var contacto = new Contacto
            {
                Id = _siguienteId++,
                Nombre = nombre,
                Apellido = apellido,
                FechaNacimiento = fechaNacimiento,
                Direccion = direccion,
                Genero = genero,
                EstadoCivil = estadoCivil,
                Movil = movil,
                Telefono = telefono,
                CorreoElectronico = correoElectronico
            };

            _contactos.Add(contacto);
            GuardarEnArchivo();
            return contacto;
        }

        // ---------------------------------------------------------
        // 2. MODIFICAR UN REGISTRO
        // ---------------------------------------------------------
        public bool Modificar(
            int id,
            string nombre,
            string apellido,
            DateTime fechaNacimiento,
            string direccion,
            Genero genero,
            EstadoCivil estadoCivil,
            string movil,
            string telefono,
            string correoElectronico)
        {
            var contacto = _contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null)
                return false;

            contacto.Nombre = nombre;
            contacto.Apellido = apellido;
            contacto.FechaNacimiento = fechaNacimiento;
            contacto.Direccion = direccion;
            contacto.Genero = genero;
            contacto.EstadoCivil = estadoCivil;
            contacto.Movil = movil;
            contacto.Telefono = telefono;
            contacto.CorreoElectronico = correoElectronico;

            GuardarEnArchivo();
            return true;
        }

        // ---------------------------------------------------------
        // 3. BUSCAR REGISTRO(S)
        // ---------------------------------------------------------
        public Contacto? BuscarPorId(int id)
        {
            return _contactos.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Busca por coincidencia parcial (sin distinguir mayusculas/minusculas)
        /// en nombre, apellido, movil, telefono o correo electronico.
        /// </summary>
        public List<Contacto> Buscar(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda))
                return new List<Contacto>();

            var texto = textoBusqueda.Trim().ToLowerInvariant();

            return _contactos.Where(c =>
                    c.Nombre.ToLowerInvariant().Contains(texto) ||
                    c.Apellido.ToLowerInvariant().Contains(texto) ||
                    c.Movil.ToLowerInvariant().Contains(texto) ||
                    c.Telefono.ToLowerInvariant().Contains(texto) ||
                    c.CorreoElectronico.ToLowerInvariant().Contains(texto))
                .ToList();
        }

        // ---------------------------------------------------------
        // 4. ELIMINAR UN REGISTRO
        // ---------------------------------------------------------
        public bool Eliminar(int id)
        {
            var contacto = _contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null)
                return false;

            _contactos.Remove(contacto);
            GuardarEnArchivo();
            return true;
        }

        // ---------------------------------------------------------
        // Utilidades
        // ---------------------------------------------------------
        public List<Contacto> ListarTodos()
        {
            return _contactos.OrderBy(c => c.Id).ToList();
        }

        public int Cantidad => _contactos.Count;

        private void GuardarEnArchivo()
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_contactos, opciones);
            File.WriteAllText(_rutaArchivo, json);
        }

        private void CargarDesdeArchivo()
        {
            if (!File.Exists(_rutaArchivo))
            {
                _contactos = new List<Contacto>();
                return;
            }

            try
            {
                string json = File.ReadAllText(_rutaArchivo);
                _contactos = JsonSerializer.Deserialize<List<Contacto>>(json) ?? new List<Contacto>();
            }
            catch (Exception)
            {
                // Si el archivo esta corrupto o vacio, se inicia con una lista limpia.
                _contactos = new List<Contacto>();
            }
        }
    }
}