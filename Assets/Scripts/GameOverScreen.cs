using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI finalTimeText;
    public GameObject panel; // painel de fim de jogo

    // Chame isso quando o jogador zerar o jogo
    public void ShowEndScreen(GameTimer timer)
    {
        timer.StopTimer();
        finalTimeText.text = "Seu tempo: " + timer.GetFormattedTime();
        panel.SetActive(true);
    }
}