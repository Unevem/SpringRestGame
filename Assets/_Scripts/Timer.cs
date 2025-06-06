using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Configurações")]
    public float duracaoEstacaoMinutos = 2f;
    public Text textoRelogio;
    public Text textoEstacao;
    public Text textoDesafio;

    private float tempoDecorrido = 0f;
    private float duracaoEstacaoSegundos;
    private string[] estacoes = { "PRIMAVERA", "VERÃO", "OUTONO", "INVERNO" };
    private int indiceEstacaoAtual = 0;

    [System.Serializable]
    public struct SeasonalMultiplier
    {
        public ResourceType type;
        public float multiplier;  // negativo ou positivo
    }

    public SeasonalMultiplier[] seasonalMultipliers;

    [Header("Consumo de comida")]
    public float foodConsumptionPerHousePerMinute = 2f;
    private float tempoParaConsumoDeComida = 0f;
    private const float intervaloConsumoComida = 60f;  // 1 minuto

    void Start()
    {
        duracaoEstacaoSegundos = duracaoEstacaoMinutos * 60f;
        AtualizarUI();
    }

    void Update()
    {
        tempoDecorrido += Time.deltaTime;
        tempoParaConsumoDeComida += Time.deltaTime;

        if (tempoDecorrido >= duracaoEstacaoSegundos)
        {
            AvancarEstacao();
            tempoDecorrido = 0f;
            AplicarDesafioEstacional();
            AdicionarRecurso();
        }

        if (tempoParaConsumoDeComida >= intervaloConsumoComida)
        {
            tempoParaConsumoDeComida = 0f;
            ConsumirComida();
        }

        if (indiceEstacaoAtual == 1)
        {
            VerificarIncendio();
        }

        AtualizarUI();
    }

    void VerificarIncendio()
    {
        float chance = Random.Range(0f, 100f); // Gera um número entre 0 e 100
        if (chance <= 1.5f) // Apenas 1.5% de chance por segundo
        {
            IniciarIncendio();
        }
    }

    void IniciarIncendio()
    {
        Building[] todasConstrucoes = FindObjectsOfType<Building>();

        if (todasConstrucoes.Length > 0)
        {
            Building construcoesEmChamas = todasConstrucoes[Random.Range(0, todasConstrucoes.Length)];

            if (construcoesEmChamas.CompareTag("OnFire"))
                return;

            Debug.Log($"Incêndio! {construcoesEmChamas.name} está pegando fogo!");

            construcoesEmChamas.tag = "OnFire";

            // Adiciona um efeito visual (mudar cor)
            SpriteRenderer sr = construcoesEmChamas.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.red;
            }

            // Inicia a remoção da construção após 10 segundos
            StartCoroutine(DestruirConstrucao(construcoesEmChamas, 10f));
        }
    }

    IEnumerator DestruirConstrucao(Building building, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (building != null)
        {
            Debug.Log($"{building.name} foi destruída pelo fogo!");
            Destroy(building.gameObject);
        }
    }

    void AvancarEstacao()
    {
        indiceEstacaoAtual = (indiceEstacaoAtual + 1) % 4;
        Debug.Log(estacoes[indiceEstacaoAtual] + " começou!");

        if (textoEstacao != null)
            textoEstacao.text = estacoes[indiceEstacaoAtual];
    }

    void AplicarDesafioEstacional()
    {
        switch (indiceEstacaoAtual)
        {
            case 0:
                textoDesafio.text = "Estação tranquila!";
                break;
            case 1:
                textoDesafio.text = "Incêndio! -1 casa";
                VerificarIncendio();
                break;
            case 2:
                textoDesafio.text = "Pragas! Recursos ÷2";
                break;
            case 3:
                int mortes = Random.Range(1, 4);
                textoDesafio.text = $"Frio intenso! -{mortes} habitantes";
                break;
        }
    }

    void AtualizarUI()
    {
        int minutos = (int)(tempoDecorrido / 60f);
        int segundos = (int)(tempoDecorrido % 60f);
        textoRelogio.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void AdicionarRecurso()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        var totalIncreases = gm.GetTotalResourceIncreasePerType();

        foreach (var entry in totalIncreases)
        {
            float multiplier = 1f;

            foreach (var mult in seasonalMultipliers)
            {
                if (mult.type == entry.Key)
                {
                    multiplier = mult.multiplier;
                    break;
                }
            }

            int seasonalBonus = Mathf.RoundToInt(entry.Value * multiplier);
            gm.AddResource(entry.Key, seasonalBonus);

            Debug.Log($"Bônus estacional: {entry.Key} + {seasonalBonus}");
        }
    }

    void ConsumirComida()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        int quantidadeDeCasas = ContarCasas();

        int totalConsumo = Mathf.RoundToInt(foodConsumptionPerHousePerMinute * quantidadeDeCasas);

        gm.SpendResource(ResourceType.Food, totalConsumo);

        Debug.Log($"Consumo de comida: -{totalConsumo} (Casas: {quantidadeDeCasas}, X: {foodConsumptionPerHousePerMinute})");
    }

    int ContarCasas()
    {
        int count = 0;

        Building[] todasConstrucoes = FindObjectsOfType<Building>();

        foreach (var b in todasConstrucoes)
        {
            if (b.IncomeType == ResourceType.People)  // Definimos que 'casas' são as que geram People
            {
                count++;
            }
        }

        return count;
    }
}
