using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    
    public GameObject bestScoreButton;

    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }
    
    public void BestScore() {
        bestScoreButton.GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("highScore").ToString();
    }
    
    public void QuitGame() {
        Application.Quit();
    }
}
