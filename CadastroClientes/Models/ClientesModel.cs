using System.ComponentModel.DataAnnotations;

namespace CadastroClientes.Models
{
    public class ClientesModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Digite o nome completo do cliente!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite a data de nascimento do cliente!")]
        public DateTime Nascimento { get; set; }
        [Required(ErrorMessage = "Digite o telefone do cliente!")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Digite o CPF do cliente!")]
        public string CPF { get; set; }
        [Required(ErrorMessage = "Digite o email do cliente!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Digite a rua do cliente!")]
        public string Rua { get; set; }
        [Required(ErrorMessage = "Digite o bairro do cliente!")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "Digite a cidade do cliente!")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "Digite o estado do cliente!")]
        public string Estado { get; set; }
        [Required(ErrorMessage = "Digite o CEP do cliente!")]
        public string CEP { get; set; }
    }
}
