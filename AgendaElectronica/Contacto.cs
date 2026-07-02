using System;

namespace AgendaElectronica
{
    /// <summary>
    /// Enumeracion para el genero del contacto.
    /// </summary>
    public enum Genero
    {
        Masculino,
        Femenino,
        Otro
    }

    /// <summary>
    /// Enumeracion para el estado civil del contacto.
    /// </summary>
    public enum EstadoCivil
    {
        Soltero,
        Casado,
        Divorciado,
        Viudo
    }

    /// <summary>
    /// Representa un registro de la agenda electronica.
    /// </summary>
    public class Contacto
    {
        // Identificador unico interno (no visible como campo de negocio,
        // pero necesario para poder Modificar/Eliminar de forma precisa).
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public Genero Genero { get; set; }
        public EstadoCivil EstadoCivil { get; set; }
        public string Movil { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"[{Id}] {Nombre} {Apellido} | Nac: {FechaNacimiento:dd/MM/yyyy} | " +
                   $"Dir: {Direccion} | Genero: {Genero} | Edo. Civil: {EstadoCivil} | " +
                   $"Movil: {Movil} | Tel: {Telefono} | Email: {CorreoElectronico}";
        }
    }
}
