using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroDeAluunos
{
    class Aluno
    {

        // Propriedades básicas do aluno
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Curso { get; set; }
        public bool ConclusaoEM { get; set; }

        // Sobrescreve o método ToString() para exibir as informações do aluno na lista
        public override string ToString()
        {
            return $"Nome: {Nome}, Idade: {Idade}, Curso: {Curso},\n Cidade: {Cidade} - {Estado},\n Conclusão EM: {(ConclusaoEM ? "Sim" : "Não")}";
        }
    }
}
