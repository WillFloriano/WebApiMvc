using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB.Models
{
    public class AlunoTurmaViewModel
    {
        public int Aluno_Id { get; set; }
        public int Turma_Id { get; set; }
        public string Nome { get; set; }
        public string Turma { get; set; }

        public string SelectedAluno { get; set; }

        public string SelectedTurma { get; set; }

        public List<SelectListItem> SelectedAlunoList { get; set; }

        public List<SelectListItem> SelectedTurmaList { get; set; }
    }    

}
