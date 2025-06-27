using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicaDentista.Models
{
    public class Paciente
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;  // Inicializando com string vazia

        [Required]
        public string CPF { get; set; } = string.Empty;   // Inicializando com string vazia

        [Required]
        public string Telefone { get; set; } = string.Empty;  // Inicializando com string vazia

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
    }
}
