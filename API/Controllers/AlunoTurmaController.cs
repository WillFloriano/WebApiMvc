using API.Entities;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoTurmaController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AlunoTurmaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlunoTurma>>> GetAlunoTurma()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<AlunoTurma> alunoTurmas = await SelectAllAlunoTurmas(connection);
            return Ok(alunoTurmas);
        }

        [HttpGet("{alunoId},{turmaId}")]
        public async Task<ActionResult<List<AlunoTurma>>> GetAlunoTurmaByID(int alunoId, int turmaId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var alunoTurma = await connection.QueryAsync<AlunoTurma>("Select aluno.nome, turma.turma, * from aluno_turma left join aluno on aluno.id = aluno_turma.aluno_id left join turma on turma.id = aluno_turma.turma_id where aluno_id=" +  alunoId + "and turma_id=" + turmaId + "order by aluno.id");
            return Ok(alunoTurma);
        }


        [HttpPost]
        public async Task<ActionResult<List<AlunoTurma>>> CreateAlunoTurma(AlunoTurma alunoTurma)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("IF NOT EXISTS(SELECT 1 FROM aluno_turma WHERE aluno_id= @Aluno_Id AND turma_id= @Turma_Id) INSERT INTO aluno_turma (aluno_id, turma_id) values (@Aluno_Id, @Turma_Id);", alunoTurma);
            return Ok(await SelectAllAlunoTurmas(connection));
        }

        [HttpPut("{alunoId},{turmaId}")]
        public async Task<ActionResult<List<AlunoTurma>>> UpdateAlunoTurma(AlunoTurma alunoTurma)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE aluno_turma set aluno_id = @Aluno_Id , turma_id = @turma where aluno_id = @Aluno_Id and turma_id = @Turma_Id;", alunoTurma);
            return Ok(await SelectAllAlunoTurmas(connection));
        }

        [HttpDelete("{alunoId},{turmaId}")]
        public async Task<ActionResult<List<AlunoTurma>>> DeleteAlunoTurma(int alunoId, int turmaId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("DELETE FROM aluno_turma where aluno_id = @Aluno_Id and turma_id = @Turma_Id;", new { Aluno_Id = alunoId, Turma_id = turmaId });
            return Ok(await SelectAllAlunoTurmas(connection));
        }

        private static async Task<IEnumerable<AlunoTurma>> SelectAllAlunoTurmas(SqlConnection connection)
        {
            return await connection.QueryAsync<AlunoTurma>("Select aluno.nome, turma.turma, * from aluno_turma left join aluno on aluno.id = aluno_turma.aluno_id left join turma on turma.id = aluno_turma.turma_id order by aluno.id");
        }
    }
}
