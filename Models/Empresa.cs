using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Pedidos.Models
{
    public class Empresa
    {
        [Key]
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "O nome da empresa é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }


        [StringLength(255, ErrorMessage = "O endereço deve ter no máximo 255 caracteres")]
        public string Endereco { get; set; }

        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um email válido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage = "A URL do logo deve ter no máximo 500 caracteres")]
        public string LogoUrl { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; }


    }
}
