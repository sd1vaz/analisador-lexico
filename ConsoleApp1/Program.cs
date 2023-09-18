using System;
using System.Collections.Generic;
using System.IO;

class AnalisadorLexico
{
    private readonly List<string> palavrasReservadas = new List<string>
    {
        "DEFINICOES", "FIM", "INICIO", "LEIA", "IF", "ELSE", "PRINT"
    };

    private readonly List<string> operadores = new List<string>
    {
        ":=", "==", "!=", ">=", "<=", ">>", "<<", "&&", "||"
    };

    private readonly List<string> operadoresMatematicos = new List<string>
    {
        "+", "-", "/", "*", "^", "**", "%", "++", "--", "+=", "-=", "*=", "/=", "%="
    };

    private readonly List<string> simbolosAbertura = new List<string>
    {
        "(", "[", "{", "“"
    };

    private readonly List<string> simbolosFechamento = new List<string>
    {
        ")", "]", "}", "”"
    };

    private readonly List<string> inicioEstrutura = new List<string>
    {
        ":"
    };

    private readonly List<string> fimEstrutura = new List<string>
    {
        ";"
    };

    private readonly List<string> tipos = new List<string>
    {
        "INT", "FLT", "STR", "BOL"
    };

    private int parentesesAbertos = 0;
    private int parentesesFechados = 0;
    private int aspasAbertas = 0;
    private int aspasFechadas = 0;
    private int chavesAbertas = 0;
    private int chavesFechadas = 0;
    private int colchetesAbertos = 0;
    private int colchetesFechados = 0;
    private int estruturasAbertas = 0;
    private int estruturasFechadas = 0;

    public List<string> PalavrasReservadas => palavrasReservadas;
    public List<string> Operadores => operadores;
    public List<string> OperadoresMatematicos => operadoresMatematicos;
    public List<string> SimbolosAbertura => simbolosAbertura;
    public List<string> SimbolosFechamento => simbolosFechamento;
    public List<string> InicioEstrutura => inicioEstrutura;
    public List<string> FimEstrutura => fimEstrutura;
    public List<string> Tipos => tipos;
    public int ParentesesAbertos { get => parentesesAbertos; set => parentesesAbertos = value; }
    public int ParentesesFechados { get => parentesesFechados; set => parentesesFechados = value; }
    public int AspasAbertas { get => aspasAbertas; set => aspasAbertas = value; }
    public int AspasFechadas { get => aspasFechadas; set => aspasFechadas = value; }
    public int ChavesAbertas { get => chavesAbertas; set => chavesAbertas = value; }
    public int ChavesFechadas { get => chavesFechadas; set => chavesFechadas = value; }
    public int ColchetesAbertos { get => colchetesAbertos; set => colchetesAbertos = value; }
    public int ColchetesFechados { get => colchetesFechados; set => colchetesFechados = value; }
    public int EstruturasAbertas { get => estruturasAbertas; set => estruturasAbertas = value; }
    public int EstruturasFechadas { get => estruturasFechadas; set => estruturasFechadas = value; }

    public void ZeraAbertosFechados()
    {
        parentesesAbertos = 0;
        parentesesFechados = 0;
        aspasAbertas = 0;
        aspasFechadas = 0;
        chavesAbertas = 0;
        chavesFechadas = 0;
        colchetesAbertos = 0;
        colchetesFechados = 0;
    }

    public bool VerificaOperador(string palavra)
    {
        return operadores.Contains(palavra);
    }

    public bool VerificaOperadorMatematico(string palavra)
    {
        return operadoresMatematicos.Contains(palavra);
    }

    public bool VerificaString(string palavra)
    {
        return (aspasAbertas > 0) && (!simbolosFechamento.Contains(palavra)) && (!simbolosAbertura.Contains(palavra));
    }

    public bool VerificaReservada(string palavra)
    {
        return (palavra.ToUpper() == palavra) && (palavra.Length <= 12) && !palavra.Contains(";") && !palavra.Contains(":") || palavrasReservadas.Contains(palavra) && !palavra.Contains(";" ) && !palavra.Contains(":") ;
    }
    
    public bool VerificaSimboloAbertura(string palavra)
    {
        return simbolosAbertura.Contains(palavra);
    } 

    public bool VerificaSimboloFechamento(string palavra)
    {
        return simbolosFechamento.Contains(palavra);
    }

    public bool VerificaInicioEstrutura(string palavra)
    {
        return inicioEstrutura.Contains(palavra);
    }

    public bool VerificaFimEstrutura(string palavra)
    {
        return fimEstrutura.Contains(palavra);
    }

    public bool VerificaVariavel(string palavra)
    {
        if (palavra.Length < 7 || !palavra.StartsWith("var") || !tipos.Contains(palavra.Substring(3, 3)) || !palavra.Substring(6).ToLower().Equals(palavra.Substring(6)))
        {
            return false;
        }
        return true;
    }
}

class Program
{
    static void Main()
    {
        
        AnalisadorLexico analisadorLexico = new AnalisadorLexico();
        //string file_path = "C:\\Users\\Caio-\\OneDrive\\Documentos\\compilador\\";// colocar nesta linha o endereço do arquivo o base esse endereço sero o mesmo aonde sera gerado o arquivo de output
        Console.WriteLine("Insira o caminho do seu arquivo input: ");
        string file = Console.ReadLine();
        string file_path = file.Replace("\\","\\\\" ) ; 

        StreamReader arqv_entrada = new StreamReader(file_path + "\\input.txt");
        StreamWriter arqv_saida = new StreamWriter(file_path + "\\output.txt");

        while (!arqv_entrada.EndOfStream)
        {
            string linha = arqv_entrada.ReadLine();
            string[] palavras = linha.Split();
            string linha_saida = " ";

            analisadorLexico.ZeraAbertosFechados();

            foreach (string palavra in palavras)
            {
                if (analisadorLexico.VerificaOperador(palavra))
                {
                    linha_saida += $"<{palavra}> ";
                }
                
                else if (analisadorLexico.VerificaOperadorMatematico(palavra))
                {
                    linha_saida += $"<{palavra}> ";
                }
                else if (analisadorLexico.VerificaString(palavra))
                {
                    linha_saida += $"<{palavra}> ";
                }
               
                else if (analisadorLexico.VerificaSimboloAbertura(palavra))
                {
                    if (palavra == analisadorLexico.SimbolosAbertura[0])
                    {
                        analisadorLexico.ParentesesAbertos += 1;
                    }
                    else if (palavra == analisadorLexico.SimbolosAbertura[1])
                    {
                        analisadorLexico.ColchetesAbertos += 1;
                    }
                    else if (palavra == analisadorLexico.SimbolosAbertura[2])
                    {
                        analisadorLexico.ChavesAbertas += 1;
                    }
                    else if (palavra == analisadorLexico.SimbolosAbertura[3])
                    {
                        analisadorLexico.AspasAbertas += 1;
                    }


                    linha_saida += $"<{palavra}> ";
                }
                else if (analisadorLexico.VerificaSimboloFechamento(palavra))
                {
                    if (palavra == analisadorLexico.SimbolosFechamento[0])
                    {
                        if (analisadorLexico.ParentesesAbertos > analisadorLexico.ParentesesFechados)
                        {
                            analisadorLexico.ParentesesAbertos -= 1;
                            analisadorLexico.ParentesesFechados += 1;
                            linha_saida += $"<{palavra}> ";
                        }
                        else
                        {
                            linha_saida += "<ERRO> ";
                        }
                    }
                    else if (palavra == analisadorLexico.SimbolosFechamento[1])
                    {
                        if (analisadorLexico.ColchetesAbertos > analisadorLexico.ColchetesFechados)
                        {
                            analisadorLexico.ColchetesAbertos -= 1;
                            analisadorLexico.ColchetesFechados += 1;
                            linha_saida += $"<{palavra}> ";
                        }
                        else
                        {
                            linha_saida += "<ERRO> ";
                        }
                    }
                    else if (palavra == analisadorLexico.SimbolosFechamento[2])
                    {
                        if (analisadorLexico.ChavesAbertas > analisadorLexico.ChavesFechadas)
                        {
                            analisadorLexico.ChavesAbertas -= 1;
                            analisadorLexico.ChavesFechadas += 1;
                            linha_saida += $"<{palavra}> ";
                        }
                        else
                        {
                            linha_saida += "<ERRO> ";
                        }
                    }
                    else if (palavra == analisadorLexico.SimbolosFechamento[3])
                    {
                        if (analisadorLexico.AspasAbertas > analisadorLexico.AspasFechadas)
                        {
                            analisadorLexico.AspasAbertas -= 1;
                            analisadorLexico.AspasFechadas += 1;
                            linha_saida += $"<{palavra}> ";
                        }
                        else
                        {
                            linha_saida += "<ERRO> ";
                        }
                    }
                }
                else if (analisadorLexico.VerificaInicioEstrutura(palavra))
                {
                    linha_saida += $"<{palavra}> ";
                }
                else if (analisadorLexico.VerificaFimEstrutura(palavra))
                {
                    linha_saida += $"<{palavra}> ";
                }
                else if (analisadorLexico.VerificaVariavel(palavra))
                {
                    linha_saida += "<id> ";
                }
                else if (int.TryParse(palavra, out int result))
                {
                    linha_saida += $"<{palavra}> ";
                }
                else if (analisadorLexico.VerificaReservada(palavra))
                {
                    linha_saida += $"<RESERVADA>({palavra}) " +
                        "";
                }
                else
                {
                    linha_saida += "<ERRO> ";
                }
            }

            int total_abertos = analisadorLexico.ParentesesAbertos + analisadorLexico.AspasAbertas + analisadorLexico.ChavesAbertas + analisadorLexico.ColchetesAbertos;
            for (int i = 0; i < total_abertos; i++)
            {
                linha_saida += "<ERRO> ";
            }
            string[] linhas = File.ReadAllLines(file_path + "\\input.txt");

           

            if (linha == linhas[linhas.Length - 1])
            {
                for (int i = 0; i < analisadorLexico.EstruturasAbertas; i++)
                {
                    linha_saida += "<ERRO> ";
                }
                linha_saida += "\n";
            }
            else
            {
                linha_saida += "\n";
            }

            arqv_saida.Write(linha_saida);
        }
        arqv_entrada.Close();
        arqv_saida.Close();
        Console.WriteLine("\n arquivo output gerado");
        Console.WriteLine("\n\nPressione qualquer tecla para fechar o programa...");
        Console.ReadKey();
    }
}
