using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace CadastroDeAluunos // ← Nome do namespace (pode ser qualquer nome, serve para organizar o código)
{

    public partial class MainWindow : Window
    {

        // ObservableCollection é uma lista dinâmica — se algo é adicionado ou removido,
        // o WPF atualiza automaticamente a interface (ListBox, DataGrid etc.)
        ObservableCollection<Aluno> alunos = new();

        // Lista com alguns estados pré-definidos, apenas para exemplo
        List<Aluno> estadoAlunos = new List<Aluno>
        {
            new Aluno {Estado = "SP" },
            new Aluno {Estado = "RJ" },
            new Aluno {Estado = "MG" },
            new Aluno {Estado = "PR" },
            new Aluno {Estado = "RS" },
            new Aluno {Estado = "BA" }
        };

        // Dicionário que relaciona cidades com seus respectivos estados
        // (chave = cidade, valor = estado)
        Dictionary<string, string> cidadesEstados = new()
        {
            { "São Paulo", "SP" },
            { "Rio de Janeiro", "RJ" },
            { "Belo Horizonte", "MG" },
            { "Curitiba", "PR" },
            { "Porto Alegre", "RS" },
            { "Salvador", "BA" }
        };

        // Caminho do arquivo JSON (será configurado mais tarde)
        private string caminhoJson;

        public MainWindow()
        {
            InitializeComponent();// Inicializa os componentes da interface (XAML)

            // Liga a lista de alunos ao ListBox
            LstAlunos.ItemsSource = alunos;

        }

        // ========================================
        // EVENTO: Quando o usuário marca "Sim"
        // ========================================
        private void RdbSim_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Você marcou que concluiu o Ensino Médio.", "Informação");
        }


        // ========================================
        // EVENTO: Quando o usuário marca "Não"
        // ========================================
        private void RdbNao_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Você marcou que NÃO concluiu o Ensino Médio.", "Informação");
        }

        // ========================================
        // EVENTO: Preencher ComboBox com as cidades
        // ========================================
        private void BtPreenchercb_Click(object sender, RoutedEventArgs e)
        {
            // Limpa o ComboBox antes de preencher
            CmbCidade.Items.Clear();

            // Adiciona as cidades ao ComboBox
            foreach (var cidade in cidadesEstados.Keys)
            {
                CmbCidade.Items.Add(cidade);
            }

            MessageBox.Show("ComboBox de cidades preenchido com sucesso!", "Sucesso");
        }


        // ========================================
        // EVENTO: Quando o usuário muda a cidade
        // ========================================

        private void CmbCidade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Se houver uma cidade selecionada
            if (CmbCidade.SelectedItem != null)
            {
                string cidadeSelecionada = CmbCidade.SelectedItem.ToString()!;

                // Procura o estado correspondente e mostra na TextBox
                if (cidadesEstados.ContainsKey(cidadeSelecionada))
                {
                    TxtEstado.Text = cidadesEstados[cidadeSelecionada];
                }
            }

        }

        // ========================================
        // EVENTO: Botão de cadastrar aluno
        // ========================================
        private void BtCadastrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verifica se os campos obrigatórios foram preenchidos
                if (string.IsNullOrWhiteSpace(TxtNome.Text) ||
                    string.IsNullOrWhiteSpace(TxtIdade.Text) ||
                    string.IsNullOrWhiteSpace(TxtCurso.Text) ||
                    CmbCidade.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Atenção");
                    return;
                }

                // Converte idade de texto para número inteiro
                if (!int.TryParse(TxtIdade.Text, out int idade))
                {
                    MessageBox.Show("A idade deve ser um número válido.", "Erro");
                    return;
                }

                var cidadeSelecionada = CmbCidade.SelectedValue as ComboBoxItem;
                var cidade = cidadeSelecionada != null ? cidadeSelecionada.Content.ToString() : string.Empty;
                // Cria um novo aluno com os dados do formulário
                var novoAluno = new Aluno
                {
                    Nome = TxtNome.Text,
                    Idade = idade,
                    Curso = TxtCurso.Text,
                    Cidade = cidade,
                    Estado = TxtEstado.Text,
                    ConclusaoEM = RdbSim.IsChecked == true
                };

                // Adiciona o aluno à lista
                alunos.Add(novoAluno);

                // Limpa os campos após o cadastro
                TxtNome.Clear();
                TxtIdade.Clear();
                TxtCurso.Clear();
                TxtEstado.Clear();
                CmbCidade.SelectedIndex = -1;
                RdbSim.IsChecked = false;
                RdbNao.IsChecked = false;

                MessageBox.Show("Aluno cadastrado com sucesso!", "Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar aluno: {ex.Message}", "Erro");
            }

        }

        private void BtListar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Total de alunos cadastrados: {alunos.Count}", "Informação");
        }


        // ========================================
        // EVENTO: Botão para salvar em JSON
        // ========================================
        private void BtSalvarJSON_Click(object sender, RoutedEventArgs e)
        {

            // Define a pasta onde o arquivo será salvo
            var pasta = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

            // Caminho completo do arquivo JSON
            var caminho = System.IO.Path.Combine(pasta, "cursos.json");

            // Opções para formatar o JSON (indentação)
            var opcoes = new JsonSerializerOptions { WriteIndented = true };

            // Serializa (converte) a lista de alunos em JSON
            var json = JsonSerializer.Serialize(alunos.ToList(), opcoes);

            // Salva o arquivo no disco
            File.WriteAllText(caminho, json);

            MessageBox.Show($"Arquivo salvo: {caminho}");

        }

        // ========================================
        // EVENTO: Botão para carregar JSON
        // ========================================

        private void BtCarregarJSON_Click(object sender, RoutedEventArgs e)
        {
            var pasta = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data");
            var caminho = System.IO.Path.Combine(pasta, "cursos.json");

            // Verifica se o arquivo existe
            if (!File.Exists(caminho))
            {
                MessageBox.Show("Arquivo cursos.json não encontrado na pasta Data.");
                return;
            }

            // Lê o conteúdo do arquivo JSON
            var json = File.ReadAllText(caminho);
            // Desserializa (converte de volta) o JSON em uma lista de alunos
            var lista = JsonSerializer.Deserialize<List<Aluno>>(json);

            // Limpa a lista atual e adiciona os alunos carregados
            alunos.Clear();
            foreach (var c in lista!)
                alunos.Add(c);

            MessageBox.Show("Dados carregados do JSON.");

        }

        // ========================================
        // EVENTO: Botão para gerar relatório de alunos
        // ========================================
        private void BtGerarRelatorio_Click(object sender, RoutedEventArgs e)
        {
            // Verifica se há alunos cadastrados
            if (alunos.Count == 0)
            {
                MessageBox.Show("Nenhum curso para gerar relatório.");
                return;
            }

            // Cria (ou garante que exista) a pasta Data

            var pasta = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);
            // Caminho do relatório (arquivo .txt)
            var caminho = System.IO.Path.Combine(pasta, "relatorio_cursos.txt");

            // Usa StringBuilder para montar o texto do relatório
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Relatório de Cursos");
            sb.AppendLine("------------------");
            sb.AppendLine($"Gerado em: {System.DateTime.Now}");
            sb.AppendLine();


            // Percorre a lista e adiciona as informações de cada aluno
            foreach (var c in alunos)
            {
                sb.AppendLine($"Nome: {c.Nome}");
                sb.AppendLine($"Idade: {c.Idade}h");
                sb.AppendLine($"Cidade: {c.Cidade}");
                sb.AppendLine($"estado: {c.Estado}");
                sb.AppendLine($"Conclusao Ensino medio: {c.ConclusaoEM}");
                sb.AppendLine($"Conclusao Curso: {c.Curso}");
                sb.AppendLine("------------------");
            }

            // Salva o arquivo no disco

            File.WriteAllText(caminho, sb.ToString());

            MessageBox.Show($"Relatório gerado: {caminho}");

            // Abre o relatório automaticamente no bloco de notas

           System.Diagnostics.Process.Start("notepad.exe", caminho);
        }

    }


 }
