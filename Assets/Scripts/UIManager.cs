using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject _gameOverScreen;

    private void Awake()
    {
        _gameOverScreen = GameObject.Find("GameOverScreen");
        _gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
        _gameOverScreen.SetActive(true);
    }

}
