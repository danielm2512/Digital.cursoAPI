using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Digital.curso.api.Business.Entities;
using Digital.curso.api.Business.Repositories;
using Digital.curso.api.Configurations;
using Digital.curso.api.Filters;
using Digital.curso.api.Infraestruture.Data;
using Digital.curso.api.Models;
using Digital.curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace Digital.curso.api.Controllers
{
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {


        private readonly IUsuarioRepository _usuariorepository;
        private readonly IAuthenticationService _authenticationService;

        public UsuarioController(
            IUsuarioRepository usuariorepository,
            IAuthenticationService authenticationService)
        {
            _usuariorepository = usuariorepository;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Este serviço permite autenticar um usuário cadastrado e ativo
        /// </summary>
        /// <param name="loginViewModelInput">View model do login</param>
        /// <returns>Retorna ok, dados do ususario e o token em caso de sucesso</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatorios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErrorGenericoViewModel))]
        [HttpPost]
        [Route("logar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Logar(LoginViewModelInput  loginViewModelInput)
        {
            Usuario usuario = _usuariorepository.ObterUsuario(loginViewModelInput.Login);

            if (usuario == null)
            {
                return BadRequest("Houve um erro ao tentar acessar");
            }

            //if (usuario.Senha != loginViewModelInput.Senha.GerarSenhaCriptografada())
            //{
            //    return BadRequest("Houve um erro ao tentar acessar");
            //}

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = usuario.Codigo,
                Login = loginViewModelInput.Login,
                Email = usuario.Email

            };

            
            var token = _authenticationService.GerarToken(usuarioViewModelOutput);

            return Ok(new 
            { 
                Token = token,
                Usuario = usuarioViewModelOutput
            });
        }

        [HttpPost]
        [Route("Registrar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Registrar(RegistroViewModelInput loginViewModelInput)
        {
          
            //var migracoesPendentes = contexto.Database.GetPendingMigrations();
            //if (migracoesPendentes.Count() > 0)
            //{
            //    contexto.Database.Migrate();
            //}


            var usuario = new Usuario();
            usuario.Login = loginViewModelInput.Login;
            usuario.Senha = loginViewModelInput.Senha;
            usuario.Email = loginViewModelInput.Email;
            _usuariorepository.Adicionar(usuario);
            _usuariorepository.Commit();

            

            return Created("", loginViewModelInput);
        }
    }
}
