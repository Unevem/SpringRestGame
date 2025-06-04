using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text pointsText;
    public TMP_Text construcoesText; // ✅ Novo campo para mostrar construções
    [SerializeField] private string nomeDoLevelDeJogo;
    [SerializeField] private string VoltarMenu;
    [SerializeField] private GameObject painelBackground;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Setup(float score, int construcoes) // ✅ Recebe construções também
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString("F2") + " Recursos";

        if (construcoesText != null)
        {
            construcoesText.text = construcoes + " Construções";
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(nomeDoLevelDeJogo);
        Debug.Log("O Jogo foi Reiniciado");
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(VoltarMenu);
    }
}