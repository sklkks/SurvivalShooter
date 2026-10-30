using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{    
    public TextMeshProUGUI waveInfoText;
    public TextMeshProUGUI scoreText;

    public GameObject gameOverUI;

    private void OnEnable()
    {
        SetActiveGameOverUI(false);   
    }
    
    public void SetWaveText(int wave, int count)
    {
        waveInfoText.text = $"Wave: {wave}\nEnemy Left : {count}";
    }

    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }
    
}
