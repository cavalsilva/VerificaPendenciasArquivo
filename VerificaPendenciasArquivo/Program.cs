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
            bool continua = true;
            while (continua)
            {
                Console.Write("Digite o caminho e o nome do arquivo (Diretório + Nome do arquivo): ");
                string caminhoCompleto = Console.ReadLine();
                string nomeArquivoSaida = Path.GetFileName(caminhoCompleto);
                string arquivoLog = nomeArquivoSaida + ".log";

                string diretorio = Path.GetDirectoryName(caminhoCompleto) + "\\";

                try
                {
                    string[] linhas = File.ReadAllLines(caminhoCompleto, Encoding.GetEncoding("iso-8859-1"));
                    string[] arquivoSaida = new string[linhas.Length];
                    int indice = -1;

                    foreach (var linha in linhas)
                    {
                        int linhaTamanho = linha.Length;
                        string linhaSaida = linha;

                        //Verifica se a linha contém $ se existir não irá fazer a troca
                        if (linha.IndexOf("$") == -1)
                        {
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

                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(diretorio + arquivoLog, true))
                                        {
                                            file.WriteLine(linhaSaida);
                                        }

                                        //Console.WriteLine(linhaSaida);
                                    }

                                }
                            }
                        }
                        indice++;
                        arquivoSaida[indice] = linhaSaida;

                        //Console.WriteLine(linhaSaida);
                    }


                    //File.WriteAllLines(diretorio + nomeArquivoSaida + ".txt", arquivoSaida);
                    Encoding encoding = Encoding.GetEncoding("ISO-8859-1");

                    /*
                    using (StreamWriter file = new StreamWriter(diretorio + nomeArquivoSaida + ".txt", encoding))
                    {
                        foreach (string line in arquivoSaida)
                        {
                            file.WriteLine(line);
                        }
                    }
                    */

                    using (FileStream fs = new FileStream(diretorio + nomeArquivoSaida + ".txt", FileMode.CreateNew))
                    {
                        using (StreamWriter file = new StreamWriter(fs, encoding))
                        {
                            foreach (string line in arquivoSaida)
                            {
                                file.WriteLine(line);
                            }
                        }
                    }

                    Console.WriteLine("Arquivo gerado com sucesso.");
                    Console.WriteLine("Arquivo gerado: " + diretorio + nomeArquivoSaida + ".txt");
                    Console.WriteLine("Log gerado: " + diretorio + arquivoLog);


                    Console.WriteLine();
                    Console.Write("Deseja realizar um novo caso? (s/n): ");
                    string retorno = Console.ReadLine();

                    continua = false;
                    if (retorno.ToUpper() == "S")
                        continua = true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.StackTrace);
                    Console.ReadLine();
                }
            }

        }
    }
}
