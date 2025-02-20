using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuParent;
    [SerializeField]
    private GameObject settingsParent;
    [SerializeField]
    private GameObject levelSelectionParent;

    [SerializeField]
    private GameObject mainMenuBackground;
    [SerializeField]
    private GameObject settingsBackground;
    [SerializeField]
    private GameObject levelSelectBackgroundParent;
    
    public void MainMenu()
    {
        DeactivateAll();
        
        mainMenuParent.SetActive(true);
        mainMenuBackground.SetActive(true);
    }

    public void Settings()
    {
        DeactivateAll();
        
        settingsParent.SetActive(true);
        settingsBackground.SetActive(true);
    }

    public void LevelSelection()
    {
        DeactivateAll();
        
        levelSelectionParent.SetActive(true);
        levelSelectBackgroundParent.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void DeactivateAll()
    {
        settingsParent.SetActive(false);
        settingsBackground.SetActive(false);
        
        levelSelectionParent.SetActive(false);
        levelSelectBackgroundParent.SetActive(false);
        
        mainMenuParent.SetActive(false);
        mainMenuBackground.SetActive(false);
    }
}
