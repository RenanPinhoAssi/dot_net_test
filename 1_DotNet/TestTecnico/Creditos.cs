abstract class Creditos
{
    protected const double VALOR_CREDITO_MAX = 1000000.00;
    protected const int DATA_VENCIMENTO_MIN_DIAS = 15;
    protected const int DATA_VENCIMENTO_MAX_DIAS = 40;
    protected const int PARCELAS_MIN = 5;
    protected const int PARCELAS_MAX = 72;

    protected const string CREDITO_MAX_ERRO = "O valor máximo do crédito está fora do aceito (R$ 1.000.000,00)";
    protected const string PARCELAS_ERRO = "Número de parcelos está fora do intervalo [5, 72]";
    protected const string DATA_VENCIMENTO_INCORRETA = "A data escolhida para o primeiro vencimento está fora do intervalo (DiaAtual + [15, 40])";

    // Variáveis setadas pela entrada do usuário
    public int parcelas { get; }
    public DateTime primeiro_vencimento { get; }
    public double valor_credito { get; }

    // Variáveis setadas pelo tipo de crédito
    protected double taxa_mes { get; set; }

    // Variáveis setadas automaticamente
    protected DateTime data_pedido_credito { get; }

    // Variáveis calculadas
    protected List<string> mensages_erro { set; get; }
    protected Boolean status_credito { set; get; }
    public double valor_final { get; }
    public double valor_juros { get; }

    public Creditos(int parcelas, DateTime primeiro_vencimento, double valor_credito, double taxa_mes)
    {
        this.data_pedido_credito = DateTime.Now.Date;

        this.parcelas = parcelas;
        this.primeiro_vencimento = primeiro_vencimento;
        this.valor_credito = valor_credito;
        this.taxa_mes = taxa_mes;

        this.mensages_erro = new List<string> { };
        this.status_credito = this.valida_credito();
        this.valor_final = this.calcula_valor_final();
        this.valor_juros= this.calcula_valor_final_juros();
    }

    private Double calcula_valor_final()
    {
        return this.valor_credito * (1 + this.taxa_mes * parcelas);
    }

    private Double calcula_valor_final_juros()
    {
        return this.valor_final - this.valor_credito;
    }

    private Boolean valida_credito()
    {

        if (this.parcelas < PARCELAS_MIN || this.parcelas > PARCELAS_MAX)
        {
            this.mensages_erro.Add(PARCELAS_ERRO);
        }
        if (this.valor_credito > VALOR_CREDITO_MAX)
        {
            this.mensages_erro.Add(CREDITO_MAX_ERRO);
        }
        if (this.primeiro_vencimento < this.data_pedido_credito.AddDays(DATA_VENCIMENTO_MIN_DIAS) || this.primeiro_vencimento > this.data_pedido_credito.AddDays(DATA_VENCIMENTO_MAX_DIAS))
        {
            this.mensages_erro.Add(DATA_VENCIMENTO_INCORRETA);
        }

        return !(this.mensages_erro.Count > 0);
    }

    public Boolean mostra_status()
    {
        return this.status_credito;
    }
    public List<string> mostra_status_erros()
    {
        return this.mensages_erro;
    }
}


class Credito_Direto : Creditos
{
    private const double TAXA_MES = 0.02;
    public Credito_Direto(int parcelas, DateTime primeiro_vencimento, double valor_credito) : base(parcelas, primeiro_vencimento, valor_credito, TAXA_MES)
    {

    }
}

class Credito_Consignado : Creditos
{
    private const double TAXA_MES = 0.01;
    public Credito_Consignado(int parcelas, DateTime primeiro_vencimento, double valor_credito) : base(parcelas, primeiro_vencimento, valor_credito, TAXA_MES)
    {

    }
}
class Credito_Pessoa_Fisica : Creditos
{
    private const double TAXA_MES = 0.05;
    public Credito_Pessoa_Fisica(int parcelas, DateTime primeiro_vencimento, double valor_credito) : base(parcelas, primeiro_vencimento, valor_credito, TAXA_MES)
    {

    }
}
class Credito_Pessoa_Juridica : Creditos
{
    private const double VALOR_CREDITO_MIN = 15000.00;
    private const string CREDITO_MIN_CNPJ_ERRO = "O valor mínimo crédito está fora do aceito para pessoas jurídicas (R$ 15.000,00)";
    private const double TAXA_MES = 0.03;

    public Credito_Pessoa_Juridica(int parcelas, DateTime primeiro_vencimento, double valor_credito) : base(parcelas, primeiro_vencimento, valor_credito, TAXA_MES)
    {
        if (this.valor_credito < VALOR_CREDITO_MIN)
        {
            this.mensages_erro.Add(CREDITO_MIN_CNPJ_ERRO);
            this.status_credito = false;
        }
    }
}
class Credito_Imobiliario : Creditos
{
    private const double TAXA_MES = 0.0075; // 9% ano = 0,75% mês 

    public Credito_Imobiliario(int parcelas, DateTime primeiro_vencimento, double valor_credito) : base(parcelas, primeiro_vencimento, valor_credito, TAXA_MES)
    {

    }
}