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
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;

        public UsuarioController(
            IUsuarioRepository usuariorepository, 
            IConfiguration configuration,
            IAuthenticationService authenticationService)
        {
            _usuariorepository = usuariorepository;
            _configuration = configuration;
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
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(new ValidaCampoViewModelOutput(ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage))) ;
            //}

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = 1,
                Login = "Daniel",
                Email = "danielm2512@hotmail.com"

            };

            //var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfiguration:Secret").Value);
            //var symmetricSecurityKey = new SymmetricSecurityKey(secret);
            //var securityTokenDescriptor = new SecurityTokenDescriptor

            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, usuarioViewModelOutput.Codigo.ToString()),
            //        new Claim(ClaimTypes.Name, usuarioViewModelOutput.Login.ToString()),
            //        new Claim(ClaimTypes.Email, usuarioViewModelOutput.Email.ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(1),
            //    SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            //};
            //var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            //var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            //var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);
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
