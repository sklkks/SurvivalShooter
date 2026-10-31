using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{    
    public TextMeshProUGUI waveInfoText;
    public TextMeshProUGUI scoreText;

    public GameObject gameOverUI;

    private void Awake()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
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
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(active);
        }
    }    

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}