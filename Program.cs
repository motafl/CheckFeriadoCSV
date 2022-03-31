using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Programa valida se a agência digitada terá feriado nos próximos dias para gerar alerta ao cliente
//Busca é realizada em uma base CSV que contém a data dos feriados, tipos de feriados (municipal, estadual), nº agencia e nome da cidade/estado


namespace CheckFeriado
{
    class Program
    {
        static void Main(string[] args)
        {
            string f = DateTime.Today.AddDays(0).ToString("dd/MM/yyyy");
            Console.WriteLine("     Tela de Consulta de Feriado         Data: "+f+"\n");
            Console.WriteLine("Digite a agência do cliente PJ em atendimento");
            Console.Write("Buscar: ");
            string agencia = Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine(string.Join(" ", LerRegistro(agencia, "Feriado_agencia.csv", 10)));
            string dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

            Console.ReadLine();
        }
        public static void AddRegistro(string data, string Tipo, string CodigoMunicipio, string NomeCidade, string Estado, string CadastradoCAF, string CadastradoItau, string IndicadorItau, string IndicadorBBRASIL, string Agencia, string NomeAgencia, string filepath)
        {
            try
            {
                using (StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.WriteLine(data + ";" + Tipo + ";" + CodigoMunicipio + ";" + NomeCidade + ";" + Estado + ";" + CadastradoCAF + ";" + CadastradoItau + ";" + IndicadorItau + ";" + IndicadorBBRASIL + ";" + Agencia + ";" + NomeAgencia);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro", ex);
            }
        }

        public static string[] LerRegistro(string termoDePesquisa, string filepath, int colunaDoTermo)
        {
            colunaDoTermo--;
            int qtdeRegistros = 0;
            int qtdeCards = 0;
            string[] semRetorno = { "Nenhum Feriado Local nos próximos 15 dias" };
            string[] comRetorno = { "Cards gerados corretamente "};
            try
            {
                string[] linha = System.IO.File.ReadAllLines(@filepath);

                for (int i = 0; i < linha.Length; i++) // percorre a linha toda
                {
                    string[] campos = linha[i].Split(';'); // separa os dados encontrados e armazena em um campo próprio | cadeia -> linha, subcadeia -> campos
                    if (campos[colunaDoTermo] == termoDePesquisa)  /*ValidaAgencia(termoDePesquisa, campos, colunaDoTermo, qtdeRegistros) */// testa se o campo é igual ao termo pesquisado
                    {
                        //(campos[colunaDoTermo].Equals[termoDePesquisa] in campos)
                        qtdeRegistros++;
                        Console.WriteLine(qtdeRegistros+"° Registro encontrado\n");
                        for (int j = 0; j < 15; j++)
                        {
                            //string dataFutura = DateTime.Today.AddDays(60+j).ToString("dd/MM/yyyy"); //DAQUI A 60 DIAS
                            string dataFutura = DateTime.Today.AddDays(j).ToString("dd/MM/yyyy");
                            if (campos[0] == dataFutura)
                            {
                                Console.WriteLine("" +
                                    "     -->    GERAR ALERTA DE FERIADO NOS PRÓXIMOS 15 DIAS\n\n" +
                                    "     |   Informar ao cliente que sua agência não irá atender no feriado " + campos[1] +"\n     |");
                                Console.WriteLine("     |   Data do Feriado: " + dataFutura);
                                Console.WriteLine("     |   Cidade da Agência: " + campos[3]); //campos[10]
                                Console.WriteLine("     |   Número da Agência: " + campos[9]);
                                qtdeCards++;
                            }                            

                        }

                        Console.WriteLine();
                        
                    }
                }
                if (qtdeCards < 1)
                {
                    return semRetorno;
                }
                else
                {
                    Console.WriteLine("Quantidade de Cards gerados: " + qtdeCards);
                    return comRetorno;
                }
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Erro");
                return semRetorno;
                throw new ApplicationException("Erro :", ex);
            }
        }


        public static bool ValidaAgencia(string pesquisa, string[] registro, int colunaDePesquisa, int qtdeRegistros)
        {
                if (registro[colunaDePesquisa].Equals(pesquisa))
                {
                //TRANSFORMAR EM UMA NÃO BOOLEANA QUE ARMAZENA TODOS REGISTROS ENCOTRADOS EM UM VETOR PARA VALIDAÇÃO DE DATAS
                return true;
                }
            return false;
                        
        }
    }
}
