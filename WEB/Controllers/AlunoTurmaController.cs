using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using WEB.Helper;
using WEB.Models;
using WEB.Controllers;

namespace WEB.Controllers
{
    public class AlunoTurmaController : Controller
    {
        private readonly string ENDPOINT = "";
        private readonly string ENDPOINTALUNO = "";
        private readonly string ENDPOINTTURMA = "";
        private readonly HttpClient httpClient = null;
        private readonly IConfiguration _configuration;

        public AlunoTurmaController(IConfiguration config)
        {
            _configuration = config;
            ENDPOINT = $"{_configuration["AppConfig:EndpointApi:Url"]}{"alunoturma"}";
            ENDPOINTALUNO = $"{_configuration["AppConfig:EndpointApi:Url"]}{"aluno"}";
            ENDPOINTTURMA = $"{_configuration["AppConfig:EndpointApi:Url"]}{"turma"}";

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ENDPOINT);
        }
        public async Task<IActionResult> AlunoTurma()
        {
            try
            {
                List<AlunoTurmaViewModel> alunoTurma = null;

                HttpResponseMessage response = await httpClient.GetAsync(ENDPOINT);

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    alunoTurma = JsonConvert.DeserializeObject<List<AlunoTurmaViewModel>>(strResult);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao processar a solicitação!");
                }

                return View(alunoTurma);
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
                throw ex;
            }
        }

        public async Task<List<AlunoViewModel>> Aluno()
        {
            try
            {
               List<AlunoViewModel> aluno = null;

                HttpResponseMessage response = await httpClient.GetAsync(ENDPOINTALUNO);

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    aluno = JsonConvert.DeserializeObject<List<AlunoViewModel>>(strResult);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao processar a solicitação!");
                }

                return aluno;
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
                throw ex;
            }
        }

        public async Task <List<TurmaViewModel>> Turma()
        {
            try
            {
                List<TurmaViewModel> turma = null;

                HttpResponseMessage response = await httpClient.GetAsync(ENDPOINTTURMA);

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    turma = JsonConvert.DeserializeObject<List<TurmaViewModel>>(strResult);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao processar a solicitação!");
                }

                return turma;
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
                throw ex;
            }
        }

        public async Task<AlunoTurmaViewModel> AlunoId(int idAluno, int idTurma)
        {
            try
            {
                AlunoTurmaViewModel alunoTurma = null;

                HttpResponseMessage response = await httpClient.GetAsync($"{ENDPOINT}/{idAluno},{idTurma}");

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    alunoTurma = JsonConvert.DeserializeObject<AlunoTurmaViewModel>(strResult.Replace("[","").Replace("]",""));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao processar a solicitação!");
                }

                return alunoTurma;
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
                throw ex;
            }
        }        

        public async Task<IActionResult> Create()
        {
            List<AlunoViewModel> listAluno = await Aluno();
            var alunosTurmas = new AlunoTurmaViewModel();
            alunosTurmas.SelectedAlunoList = new List<SelectListItem>();
            alunosTurmas.SelectedTurmaList = new List<SelectListItem>();

            foreach (var aluno in listAluno)
            {
                alunosTurmas.SelectedAlunoList.Add(new SelectListItem { Text = aluno.Nome, Value = aluno.Id.ToString() });                
            }


            List<TurmaViewModel> listTurma = await Turma();

            foreach (var turma in listTurma)
            {
                alunosTurmas.SelectedTurmaList.Add(new SelectListItem { Text = turma.Turma, Value = turma.Id.ToString() });
            }

            return View(alunosTurmas);

        }

        [HttpPost]

        public async Task<IActionResult> Create([Bind("Aluno_Id, Turma_Id, Nome, Turma, SelectedAluno, SelectedTurma, SelectedAlunoList, SelectedTurmaList")] AlunoTurmaViewModel alunoTurma)
        {
            try
            {
                alunoTurma.Nome = "";
                alunoTurma.Turma = "";
                
                string strJsonAluno = JsonConvert.SerializeObject(alunoTurma);
                byte[] buffer = Encoding.UTF8.GetBytes(strJsonAluno);
                ByteArrayContent byteArrayContent = new ByteArrayContent(buffer);
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PostAsync(ENDPOINT, byteArrayContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Vinculo criado com sucesso.";
                }
                else
                {
                    TempData["AlertMessage"] = "Erro ao associar aluno e turma.";
                }

                return RedirectToAction("AlunoTurma");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Edit(int idAluno, int idTurma)
        {
            AlunoTurmaViewModel alunoResult = await AlunoId(idAluno, idTurma);
            alunoResult.SelectedTurmaList = new List<SelectListItem>();            

            List<TurmaViewModel> listTurma = await Turma();

            foreach (var turma in listTurma)
            {
                alunoResult.SelectedTurmaList.Add(new SelectListItem { Text = turma.Turma, Value = turma.Id.ToString() });
                alunoResult.SelectedTurma = turma.Turma;
            }

            return View(alunoResult);
        }

        [HttpPost]

        public async Task<IActionResult> Edit([Bind("Aluno_Id, Turma_Id, Nome, Turma, SelectedAluno, SelectedTurma, SelectedAlunoList, SelectedTurmaList")] AlunoTurmaViewModel alunoTurma)
        {
            try
            {
                string strJsonAluno = JsonConvert.SerializeObject(alunoTurma);
                byte[] buffer = Encoding.UTF8.GetBytes(strJsonAluno);
                ByteArrayContent byteArrayContent = new ByteArrayContent(buffer);
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"{ENDPOINT}/{alunoTurma.Aluno_Id},{alunoTurma.Turma}", byteArrayContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Vinculo atualizado com sucesso.";

                    return RedirectToAction("AlunoTurma");
                }
                else
                {
                    TempData["AlertMessage"] = "Erro ao atualizar vinculo.";

                    return RedirectToAction("Delet");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> Delete(int idAluno, int idTurma)
        {
            AlunoTurmaViewModel alunoResult = await AlunoId(idAluno, idTurma);

            return View(alunoResult);
        }

        [HttpPost]

        public async Task<IActionResult> Delete([Bind("Aluno_Id, Turma_Id, Nome, Turma, SelectedAluno, SelectedTurma, SelectedAlunoList, SelectedTurmaList")] AlunoTurmaViewModel alunoTurma)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{ENDPOINT}/{alunoTurma.Aluno_Id},{alunoTurma.Turma_Id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Aluno Deletado com sucesso.";

                    return RedirectToAction("AlunoTurma");
                }
                else
                {
                    TempData["AlertMessage"] = "Erro ao deletar aluno.";

                    return RedirectToAction("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
