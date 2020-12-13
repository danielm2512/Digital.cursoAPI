using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Digital.curso.api.Models.Usuarios
{
    public class LoginViewModelInput
    {
        [Required(ErrorMessage = "O Login é obrigatorio")]
        public string Login { get; set; }
        [Required(ErrorMessage = "A Senha é obrigatoria")]
        public string Senha { get; set; }
    }
}
