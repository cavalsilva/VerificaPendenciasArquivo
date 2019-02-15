using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerificaPendenciasArquivo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Digite o caminho e o nome do arquivo: ");
            string caminho = Console.ReadLine();

            try
            {
                string[] linhas = File.ReadAllLines(caminho, Encoding.GetEncoding("iso-8859-1"));
                string[] arquivoSaida = new string[linhas.Length];
                int indice = -1;

                foreach (var linha in linhas)
                {
                    int linhaTamanho = linha.Length;
                    string linhaSaida = linha;

                    //Senão existir deve verificar o name
                    if (linha.IndexOf("id=") == -1)
                    {
                        if (linha.IndexOf("name=") != -1)
                        {
                            //Pega o indice da tag name
                            int idxName = linha.IndexOf("name=");


                            #region RETORNAR O NOME DO CAMPO

                            //Pega a string para retornar o atributo
                            string strAux1 = linha.Substring(idxName, linhaTamanho - idxName);

                            //Pegar a primeira aspa
                            int idxAux = strAux1.IndexOf('"');
                            string strAux2 = strAux1.Substring(idxAux + 1, strAux1.Length - idxAux - 1);

                            //Pega a próxima aspa
                            int idxUltimaAspa = strAux2.IndexOf('"');
                            string nomeCampoName = "";
                            if (idxUltimaAspa != -1)
                                nomeCampoName = strAux2.Substring(0, idxUltimaAspa);

                            #endregion

                            int totalIdx = idxName + idxAux + idxUltimaAspa + 2;

                            //MONTAR ATRIBUTO ID
                            //Retorna o fechamento da tag
                            //int idxFechamentoTag = linha.LastIndexOf(">");
                            int idxFechamentoTag = totalIdx;

                            if (idxFechamentoTag != -1)
                            {
                                string nomeCampoId = " id=" + '"' + nomeCampoName + '"';

                                string linhaInicio = linha.Substring(0, idxFechamentoTag);
                                string linhaFim = linha.Substring(idxFechamentoTag, linhaTamanho - idxFechamentoTag);

                                linhaSaida = linhaInicio + nomeCampoId + linhaFim;
                            }

                        }
                    }

                    indice++;
                    arquivoSaida[indice] = linhaSaida;

                    //Console.WriteLine(linhaSaida);
                }


                File.WriteAllLines(@"C:\temp\output.txt", arquivoSaida);

                using (StreamWriter file = new StreamWriter(@"C:\temp\output.txt"))
                {
                    foreach (string line in arquivoSaida)
                    {
                        file.WriteLine(line);
                    }
                }

                Console.WriteLine("Arquivo gerado com sucesso.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.StackTrace);
                Console.ReadLine();
            }

        }
    }
}
