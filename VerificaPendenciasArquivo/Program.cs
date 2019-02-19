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

                        //Verifica se a linha contém alguma marcação de Jquery
                        if (linha.IndexOf("$") == -1)
                        {
                            //Verifica se existe tag ID
                            if (linha.IndexOf("id=") == -1)
                            {
                                //Verifica se existe a tag NAME
                                if (linha.IndexOf("name=") != -1)
                                {
                                    //Pega o indice que se encontra a tag name
                                    int idxName = linha.IndexOf("name=");

                                    //Pega a string a partir da TAG name para encontrar o atributo
                                    string strAux1 = linha.Substring(idxName, linhaTamanho - idxName);

                                    //Pegar a primeira aspa da tag NAME
                                    int idxAux = strAux1.IndexOf('"');
                                    string strAux2 = strAux1.Substring(idxAux + 1, strAux1.Length - idxAux - 1);

                                    //Pega última aspa da tag NAME
                                    int idxUltimaAspa = strAux2.IndexOf('"');
                                    string nomeCampoName = "";
                                    if (idxUltimaAspa != -1)
                                        nomeCampoName = strAux2.Substring(0, idxUltimaAspa);

                                    //Pega o maior indice para pegar o atributo da tag name, exemplo NAME="nome atributo"
                                    int totalIdx = idxName + idxAux + idxUltimaAspa + 2;

                                    //Montar atributo da tag ID a partir da tag NAME
                                    int idxFechamentoTag = totalIdx;
                                    if (idxFechamentoTag != -1)
                                    {
                                        string nomeCampoId = " id=" + '"' + nomeCampoName + '"';

                                        string linhaInicio = linha.Substring(0, idxFechamentoTag);
                                        string linhaFim = linha.Substring(idxFechamentoTag, linhaTamanho - idxFechamentoTag);

                                        linhaSaida = linhaInicio + nomeCampoId + linhaFim;

                                        //Escreve a linha alterada no log
                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(diretorio + arquivoLog, true))
                                        {
                                            file.WriteLine(linhaSaida);
                                        }
                                    }
                                }
                            }
                        }
                        indice++;

                        //Grava a linha alterada ou não no array
                        arquivoSaida[indice] = linhaSaida;

                        //Console.WriteLine(linhaSaida);
                    }

                    //Monta novo arquivo com codificação ISO-8859-1
                    Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
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

                    //Mensagem de sucesso
                    Console.WriteLine("Arquivo gerado com sucesso.");
                    Console.WriteLine("Arquivo gerado: " + diretorio + nomeArquivoSaida + ".txt");
                    Console.WriteLine("Log gerado: " + diretorio + arquivoLog);


                    //Mensagem para inclusão de um novo caso
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
