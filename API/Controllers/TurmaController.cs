using API.Entities;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurmaController : ControllerBase
    {
        private readonly IConfiguration _config;
        public TurmaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Turmas>>> GetTurma()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Turmas> turmas = await SelectAllTurmas(connection);
            return Ok(turmas);
        }

        [HttpGet("{turmaId}")]
        public async Task<ActionResult<List<Turmas>>> GetTurmaByID(int turmaId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var turma = await connection.QueryAsync<Turmas>("Select * from turma where id=@Id", new { Id = turmaId });
            return Ok(turma);
        }


        [HttpPost]
        public async Task<ActionResult<List<Turmas>>> CreateTurma(Turmas turma)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO turma (curso_id, turma, ano, situacao) values (@Curso_Id, @Turma, @Ano, @Situacao);", turma);

            return Ok(await SelectAllTurmas(connection));
        }

        [HttpPut("{turmaId}")]
        public async Task<ActionResult<List<Turmas>>> UpdateTurma(Turmas turma)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE turma set curso_id = @Curso_Id, turma = @Turma, ano = @Ano, situacao= @Situacao where id = @Id ;", turma);
            return Ok(await SelectAllTurmas(connection));
        }

        [HttpDelete("{turmaId}")]
        public async Task<ActionResult<List<Turmas>>> DeleteTurma(int turmaId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("DELETE FROM turma where id = @Id ;", new { Id = turmaId });
            return Ok(await SelectAllTurmas(connection));
        }

        private static async Task<IEnumerable<Turmas>> SelectAllTurmas(SqlConnection connection)
        {
            return await connection.QueryAsync<Turmas>("Select * from turma");
        }
    }
}
