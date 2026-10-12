using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI - Drag from Canvas")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Game Stats")]
    public int lives = 3;
    public int score = 0;
    public float survivalTime = 0f;
    public int level = 1;
    public bool isGameOver = false;

    float throwSpeed = 8f;

    void Awake(){ Instance = this; }

    void Update(){
        if(isGameOver) return;
        
        survivalTime += Time.deltaTime;
        score += Mathf.FloorToInt(Time.deltaTime * 2);

        if(survivalTime > level * 30f){
            level++;
            throwSpeed += 1.5f;
        }

        if(timeText) timeText.text = $"Time: {Mathf.Floor(survivalTime/60):00}:{Mathf.Floor(survivalTime%60):00}";
        if(scoreText) scoreText.text = $"Points: {score}";
        if(livesText) livesText.text = new string('♥', lives) + new string('♡', 3 - lives);
        if(levelText) levelText.text = $"Level {level}";
    }

    public void PlayerHit(){
        lives--;
        if(lives <= 0) GameOver();
    }

    public void PlayerDodged(){ score += 1; }
    
    public void PlayerCaught(){
        score += 10;
        lives = Mathf.Min(3, lives+1);
    }

    void GameOver(){
        isGameOver = true;
        if(finalScoreText) finalScoreText.text = $"Survived: {Mathf.FloorToInt(survivalTime)}s\nScore: {score}";
        if(gameOverPanel) gameOverPanel.SetActive(true);
        if(AdsManager.Instance) AdsManager.Instance.ShowInterstitial();
        PlayerPrefs.SetInt("BestScore", Mathf.Max(PlayerPrefs.GetInt("BestScore",0), score));
    }

    public void ContinueWithAd(){
        if(AdsManager.Instance) AdsManager.Instance.ShowRewarded(()=>{
            lives = 1;
            isGameOver = false;
            if(gameOverPanel) gameOverPanel.SetActive(false);
        });
    }

    public void Retry(){ SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    
    public float GetThrowSpeed(){ return throwSpeed; }
}
