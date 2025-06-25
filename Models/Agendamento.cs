using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ClinicaDentista.Models; // Certifique-se de que esse namespace seja correto e necessário

namespace ClinicaDentista.Models
{
    public class Agendamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime DataHora { get; set; }

        [ForeignKey("Paciente")]
        public int PacienteId { get; set; }

        
        [ValidateNever] // 👈 Impede que o ASP.NET tente validar o objeto Paciente
        public Paciente? Paciente { get; set; }

        [Required]
        public string Status { get; set; } // Exemplo: Confirmado, Cancelado
          public Agendamento()
    {
       
        Status = "Confirmado";  // Definindo um valor padrão
    }
    }
}

