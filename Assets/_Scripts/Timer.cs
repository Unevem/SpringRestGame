using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Configurações")]
    public float duracaoEstacaoMinutos = 3f;
    public Text textoRelogio;
    public Text textoEstacao;
    public Text textoDesafio;

    [Header("Recursos do Jogo")]
    public int casas = 10;
    public int recursos = 100;
    public int populacao = 50;

    private float tempoDecorrido = 0f;
    private float duracaoEstacaoSegundos;
    private string[] estacoes = { "PRIMAVERA", "VERÃO", "OUTONO", "INVERNO" };
    private int indiceEstacaoAtual = 0;

    // Start is called before the first frame update
    void Start()
    {
        duracaoEstacaoSegundos = duracaoEstacaoMinutos * 60f;
        AtualizarUI();
    }

    // Update is called once per frame
    void Update()
    {
        tempoDecorrido += Time.deltaTime;

        if (tempoDecorrido >= duracaoEstacaoSegundos)
        {
            AvancarEstacao();
            tempoDecorrido = 0f;
            AplicarDesafioEstacional();
        }

        AtualizarUI();
    }

    void AvancarEstacao()
    {
        indiceEstacaoAtual = (indiceEstacaoAtual + 1) % 4;
        Debug.Log(estacoes[indiceEstacaoAtual] + " começou!");
    }

    void AplicarDesafioEstacional()
    {
        switch (indiceEstacaoAtual)
        {
            case 0: // Primavera
                textoDesafio.text = "Estação tranquila!";
                break;

            case 1: // Verão
                casas = Mathf.Max(0, casas - 1);
                textoDesafio.text = "Incêndio! -1 casa";
                break;

            case 2: // Outono
                recursos = Mathf.FloorToInt(recursos / 2f);
                textoDesafio.text = "Pragas! Recursos ÷2";
                break;

            case 3: // Inverno
                int mortes = Random.Range(1, 4);
                populacao = Mathf.Max(0, populacao - mortes);
                textoDesafio.text = $"Frio intenso! -{mortes} habitantes";
                break;
        }
    }


    void AtualizarUI()
    {
        // Formata o tempo: MM:SS
        int minutos = (int)(tempoDecorrido / 60f);
        int segundos = (int)(tempoDecorrido % 60f);
        textoRelogio.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        // Atualiza textos
        textoEstacao.text = $"{estacoes[indiceEstacaoAtual]}\n" +
                          $"Casas: {casas}\n" +
                          $"Recursos: {recursos}\n" +
                          $"População: {populacao}";
    }
}
