using API.Entities;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {

        private readonly IConfiguration _config;
        public AlunoController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Aluno>>> GetAluno()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Aluno> alunos = await SelectAllAlunos(connection);
            return Ok(alunos);
        }

        [HttpGet("{alunoId}")]
        public async Task<ActionResult<List<Aluno>>> GetAlunoByID(int alunoId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var aluno = await connection.QueryAsync<Aluno>("Select * from aluno where id=@Id", new { Id = alunoId });
            return Ok(aluno);
        }


        [HttpPost]
        public async Task<ActionResult<List<Aluno>>> CreateAluno(Aluno aluno)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO aluno (nome, usuario, senha, situacao) values (@Nome, @Usuario, @Senha, @Situacao);", aluno);
            return Ok(await SelectAllAlunos(connection));
        }

        [HttpPut("{alunoId}")]
        public async Task<ActionResult<List<Aluno>>> UpdateAluno(Aluno aluno)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            if (aluno.Senha.Contains("*"))
            {
                await connection.ExecuteAsync("UPDATE aluno set nome = @Nome , usuario = @Usuario, situacao = @Situacao where id = @Id ;", aluno);
            }
            else
            {
                await connection.ExecuteAsync("UPDATE aluno set nome = @Nome , usuario = @Usuario, senha = @Senha, situacao = @Situacao where id = @Id ;", aluno);
            }
            
            return Ok(await SelectAllAlunos(connection));
        }

        [HttpDelete("{alunoId}")]
        public async Task<ActionResult<List<Aluno>>> DeleteAluno(int alunoId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("DELETE FROM aluno where id = @Id ;", new { Id = alunoId });
            return Ok(await SelectAllAlunos(connection));
        }

        private static async Task<IEnumerable<Aluno>> SelectAllAlunos(SqlConnection connection)
        {
            return await connection.QueryAsync<Aluno>("Select * from aluno");
        }
    }
}
