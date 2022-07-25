using System.Globalization;
using static Creditos;

namespace TesteTecnicoDotNet
{

    class Program
    {
        const int SLEEP_TIME = 1000;
        const int ANIMATION_MULTIPLIER = 1;

        public enum CREDITOS_TEST{ A, B };

        static void Main(string[] args)
        {
            int credito_tipo;
            int parcelas = 0;
            double valor_credito = 0;
            DateTime primeiro_vencimento = DateTime.Now;
            DateTime data_atual = DateTime.Now;
            Creditos credito;
            Boolean credito_status;
            List<string> credito_erros;
            Boolean retry;

            do
            {
                Console.Clear();
                Console.WriteLine("\n-- Sistema de Simulação de Crédito --");
                System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER);
                Console.WriteLine("\n\nVamos começar?");
                System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER / 5);
                Console.WriteLine("1 - Credito Direto");
                System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER / 10);
                Console.WriteLine("2 - Credito Consignado");
                System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER / 10);
                Console.WriteLine("3 - Credito Pessoa Jurídica");
                System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER / 10);
                Console.WriteLine("4 - Credito Pessoa Física");
                System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER / 10);
                Console.WriteLine("5 - Credito Imobiliário\n");
                System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER / 10);


                do
                {
                    Console.WriteLine("Escolha o tipo de crédito:");
                    try
                    {
                        credito_tipo = Convert.ToInt32(Console.ReadLine());
                        if (credito_tipo > 5 || credito_tipo < 1)
                        {
                            throw new Exception();
                        }
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Você escolheu uma opção inválida. Vamos tentar novamente?\n");
                        System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER);

                    }
                } while (true);


                do
                {
                    Console.WriteLine("Digite o valor do crédito desejado:");
                    try
                    {
                        valor_credito = Convert.ToDouble(Console.ReadLine());
                        if (valor_credito <= 0)
                        {
                            throw new Exception();
                        }
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Você digitou um valor incorreto. Vamos tentar novamente?\n");
                        System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER);

                    }
                } while (true);

                do
                {
                    Console.WriteLine("Digite a quantidade de parcelas desejada:");
                    try
                    {
                        parcelas = Convert.ToInt32(Console.ReadLine());
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Você escolheu uma opção inválida. Vamos tentar novamente?\n");
                        System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER);

                    }
                } while (true);


                do
                {
                    Console.WriteLine("Digite uma data para o primeiro vencimento (Deve estar entre " + data_atual.AddDays(15).ToString("dd/MM/yyyy") + " a " + data_atual.AddDays(40).ToString("dd/MM/yyyy") + " dias do dia atual):");
                    try
                    {
                        primeiro_vencimento = Convert.ToDateTime(Console.ReadLine());
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Você digitou uma data incorreta. Vamos tentar novamente?\n");
                        System.Threading.Thread.Sleep(SLEEP_TIME * ANIMATION_MULTIPLIER);

                    }
                } while (true);




                switch (credito_tipo)
                {
                    case 1:
                        credito = new Credito_Direto(parcelas, primeiro_vencimento, valor_credito);
                        break;
                    case 2:
                        credito = new Credito_Consignado(parcelas, primeiro_vencimento, valor_credito);
                        break;
                    case 3:
                        credito = new Credito_Pessoa_Juridica(parcelas, primeiro_vencimento, valor_credito);
                        break;
                    case 4:
                        credito = new Credito_Pessoa_Fisica(parcelas, primeiro_vencimento, valor_credito);
                        break;
                    default:
                        credito = new Credito_Imobiliario(parcelas, primeiro_vencimento, valor_credito);
                        break;

                }

                credito_status = credito.mostra_status();
                credito_erros = credito.mostra_status_erros();

                Console.WriteLine("\nResultado da Simulação: ");
                System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);


                Console.WriteLine("\nCaracterísticas do seu crédito: ");
                System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);

                Console.WriteLine("- Valor: \t" + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", credito.valor_credito));
                System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);
                Console.WriteLine("- Parcelas: \t" + credito.parcelas);
                System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);
                Console.WriteLine("- Data Primeiro Vencimento: \t" + credito.primeiro_vencimento.ToString("dd/MM/yyyy"));


                Console.WriteLine("\nCalculos referentes aos parametros: ");
                System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);
                Console.WriteLine("- Parcelas: " + credito.parcelas + "x de " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", credito.valor_final/credito.parcelas));
                System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);
                Console.WriteLine("- Valor Total com Juros: " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", credito.valor_final));
                System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);
                Console.WriteLine("- Valor dos Juros: " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", credito.valor_juros));
                System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);
                Console.WriteLine("- Status de Aprovação: " + (credito_status ? "Aprovado" : "Reprovado"));

                if (!credito_status)
                {

                    System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);
                    Console.WriteLine("\nSeu crédito não foi aprovado pelos seguintes motivos: ");
                    foreach (var mensagem_erro in credito_erros)
                    {
                        Console.WriteLine("- " + mensagem_erro);
                        System.Threading.Thread.Sleep(SLEEP_TIME / 10 * ANIMATION_MULTIPLIER);
                    }
                    Console.WriteLine("\nAjuste os parâmetros em uma nova consulta.");
                }

                Console.WriteLine("- Para simular novamente, digite '1'");
                Console.WriteLine("- Para sair do simulador, digite qualquer tecla");

                retry = Console.ReadLine() == "1" ? true : false;

            } while (retry);

            System.Environment.Exit(0);

        }
    }
}