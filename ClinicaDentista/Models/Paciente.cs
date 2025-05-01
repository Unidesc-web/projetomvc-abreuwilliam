using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicaDentista.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, ErrorMessage = "O CPF deve ter no máximo 14 caracteres.")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Número de telefone inválido.")]
        public string Telefone { get; set; } = string.Empty;

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
    }
}
