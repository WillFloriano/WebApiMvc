using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WEB.Helper;
using WEB.Models;

namespace WEB.Controllers
{
    public class AlunoController : Controller
    {
        private readonly string ENDPOINT = "";
        private readonly HttpClient httpClient = null;
        private readonly IConfiguration _configuration;

        public AlunoController(IConfiguration config)
        {
            _configuration = config;
            ENDPOINT = $"{_configuration["AppConfig:EndpointApi:Url"]}{"aluno"}";

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ENDPOINT);
        }
        public async Task<IActionResult> Aluno()
        {
            try
            {
                List<AlunoViewModel> alunos = null;

                HttpResponseMessage response = await httpClient.GetAsync(ENDPOINT);

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    alunos = JsonConvert.DeserializeObject<List<AlunoViewModel>>(strResult);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao processar a solicitação!");
                }

                return View(alunos);
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
                throw ex;
            }
        }

        public async Task<AlunoViewModel> GetAlunoId(int id)
        {
            try
            {
                AlunoViewModel alunos = null;

                HttpResponseMessage response = await httpClient.GetAsync($"{ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    alunos = JsonConvert.DeserializeObject<AlunoViewModel>(strResult.Replace("[","").Replace("]",""));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao processar a solicitação!");
                }

                return alunos;
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

        public async Task<IActionResult> Create([Bind("Nome, Usuario, Senha, Situacao")] AlunoViewModel aluno)
        {
            try
            {
                string pwHash = Password.createHash(aluno.Senha);
                aluno.Senha = pwHash;

                string strJsonAluno = JsonConvert.SerializeObject(aluno);
                byte[] buffer = Encoding.UTF8.GetBytes(strJsonAluno);
                ByteArrayContent byteArrayContent = new ByteArrayContent(buffer);
                byteArrayContent.Headers.ContentType =  new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PostAsync(ENDPOINT, byteArrayContent);

                if(response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Aluno Cadastrado com sucesso.";
                }
                else
                {
                    TempData["AlertMessage"] = "Erro ao cadastrar aluno.";
                }

                return RedirectToAction("Aluno");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            AlunoViewModel alunoResult = await GetAlunoId(id);
                        
            return View(alunoResult);
        }

        [HttpPost]

        public async Task<IActionResult> Edit([Bind("Id, Nome, Usuario, Senha, Situacao")] AlunoViewModel aluno)
        {
            try
            {
                if(!aluno.Senha.Contains("*"))
                {
                    string pwHash = Password.createHash(aluno.Senha);
                    aluno.Senha = pwHash;
                }


                string strJsonAluno = JsonConvert.SerializeObject(aluno);
                byte[] buffer = Encoding.UTF8.GetBytes(strJsonAluno);
                ByteArrayContent byteArrayContent = new ByteArrayContent(buffer);
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"{ENDPOINT}/{aluno.Id}", byteArrayContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Aluno atualizado com sucesso.";

                    return RedirectToAction("Aluno");
                }
                else
                {
                    TempData["AlertMessage"] = "Erro ao atualizar aluno.";

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
            AlunoViewModel alunoResult = await GetAlunoId(id);

            return View(alunoResult);
        }

        [HttpPost]

        public async Task<IActionResult> Delete([Bind("Id, Nome, Usuario, Senha, Situacao")] AlunoViewModel aluno)
        {
            try
            {                     
                HttpResponseMessage response = await httpClient.DeleteAsync($"{ENDPOINT}/{aluno.Id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Aluno Deletado com sucesso.";

                    return RedirectToAction("Aluno");
                }
                else
                {
                    TempData["AlertMessage"] = "Erro ao deletar aluno.";

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
