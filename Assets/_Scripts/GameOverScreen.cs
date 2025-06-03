using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text pointsText;

    public void Setup(int score){
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " POINTS";
    }
}
