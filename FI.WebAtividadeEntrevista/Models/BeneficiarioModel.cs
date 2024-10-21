using System.ComponentModel.DataAnnotations;


namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiario
    /// </summary>
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Id do cliente
        /// </summary>
        public long IdCliente { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [RegularExpression(@"[0-9]{3}\.[0-9]{3}\.[0-9]{3}\-[0-9]{2}", ErrorMessage = "Digite um CPF válido")]
        [RegraCPF(ErrorMessage = "Digite um CPF válido")]
        [Required]
        public string CPF { get; set; }

    }
}