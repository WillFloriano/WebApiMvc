using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class AlunoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "A senha deve conter: Minimo 8 caracteres, Uma letra Maiuscula, Uma letra minuscula, 1 numero e 1 caractere especial")]
        public string Senha { get; set; }
        public string Situacao { get; set; }
    }
}
