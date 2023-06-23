namespace API.Entities
{
    public class Turmas
    {
        public int Id { get; set; }
        public int Curso_Id { get; set; }
        public string Turma { get; set; }
        public int Ano { get; set; }
        public string Situacao { get; set; }
    }
}
