using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using WEB.Helper;
using WEB.Models;

namespace WEB.Controllers
{
    public class TurmaController : Controller
    {
        private readonly string ENDPOINT = "";
        private readonly HttpClient httpClient = null;
        private readonly IConfiguration _configuration;

        public TurmaController(IConfiguration config)
        {
            _configuration = config;
            ENDPOINT = $"{_configuration["AppConfig:EndpointApi:Url"]}{"turma"}";

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ENDPOINT);
        }
        public async Task<IActionResult> Turma()
        {
            try
            {
                List<TurmaViewModel> Turmas = null;

                HttpResponseMessage response = await httpClient.GetAsync(ENDPOINT);

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    Turmas = JsonConvert.DeserializeObject<List<TurmaViewModel>>(strResult);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao processar a solicitação!");
                }

                return View(Turmas);
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
                throw ex;
            }
        }
        public async Task<TurmaViewModel> GetTurmaId(int id)
        {
            try
            {
                TurmaViewModel turmas = null;

                HttpResponseMessage response = await httpClient.GetAsync($"{ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    turmas = JsonConvert.DeserializeObject<TurmaViewModel>(strResult.Replace("[", "").Replace("]", ""));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao processar a solicitação!");
                }

                return turmas;
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
                throw ex;
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create([Bind("Curso_Id, Turma, Ano, Situacao")] TurmaViewModel cTurma)
        {
            try
            {
                if(cTurma.Ano != 2023 && cTurma.Ano > 0)
                {
                    TempData["AlertMessage"] = "Não é possivel cadastrar turmas com ano anterior a 2023.";

                    return RedirectToAction("Create");
                }
                else
                {                    
                    string strJsonturma = JsonConvert.SerializeObject(cTurma);
                    byte[] buffer = Encoding.UTF8.GetBytes(strJsonturma);
                    ByteArrayContent byteArrayContent = new ByteArrayContent(buffer);
                    byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(ENDPOINT, byteArrayContent);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["AlertMessage"] = "Turma Cadastrada com sucesso.";

                        return RedirectToAction("Turma");
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Já existe uma turma cadastrada com os dados informados !.";

                        return RedirectToAction("Create");
                    }                    
                }                                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            TurmaViewModel turmaResult = await GetTurmaId(id);

            return View(turmaResult);
        }

        [HttpPost]

        public async Task<IActionResult> Edit([Bind("Id, Curso_Id, Turma, Ano, Situacao")] TurmaViewModel eTurma)
        {
            try
            {              
                string strJsonAluno = JsonConvert.SerializeObject(eTurma);
                byte[] buffer = Encoding.UTF8.GetBytes(strJsonAluno);
                ByteArrayContent byteArrayContent = new ByteArrayContent(buffer);
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"{ENDPOINT}/{eTurma.Id}", byteArrayContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Turma atualizado com sucesso.";

                    return RedirectToAction("Turma");
                }
                else if(response.StatusCode.Equals(System.Net.HttpStatusCode.BadRequest))
                {
                    TempData["AlertMessage"] = "Erro ao atulizar turma !";

                    return RedirectToAction("Edit");
                }
                else
                {
                    TempData["AlertMessage"] = "Já existe uma turma cadastrada com os dados informados !";

                    return RedirectToAction("Edit");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            TurmaViewModel turmaResult = await GetTurmaId(id);

            return View(turmaResult);
        }

        [HttpPost]

        public async Task<IActionResult> Delete([Bind("Id, Curso_Id, Turma, Ano, Situacao")] TurmaViewModel dTurma)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{ENDPOINT}/{dTurma.Id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Turma Deletada com sucesso.";

                    return RedirectToAction("Turma");
                }
                else
                {
                    TempData["AlertMessage"] = "Erro ao deletar turma.";

                    return RedirectToAction("Delet");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
