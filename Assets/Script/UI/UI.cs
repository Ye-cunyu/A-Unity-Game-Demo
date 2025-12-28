using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject defaultMenu;
    public GameObject Closebutton;
    private UI_CloseButton ui_CloseButton;

    private string sceneName = "MainMenu";
    public string menuTag = "Menu";

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenMenu();
        }
    }

    void OnEnable()
    {
        ui_CloseButton = GetComponentInChildren<UI_CloseButton>(true);
        ui_CloseButton.CloseMenu += closeMenu;
        Player.instance.PlayerIsDead += Dead;
        closeMenu();
    }
    void OnDisable()
    {
        ui_CloseButton.CloseMenu -= closeMenu;
    }
    public void SwitchTo(GameObject __menu)
    {
        closeMenu();
        Closebutton.SetActive(true);
        if (__menu != null)
            __menu.SetActive(true);
    }
    public void OpenMenu()
    {
        closeMenu();
        SwitchTo(defaultMenu);
        Closebutton.SetActive(true);
    }
    public void closeMenu()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(menuTag))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    public void ExitGame()
    {
        Debug.Log("Exit");
        SaveManager.instance.SaveGame();
        Application.Quit();
    }
    public void BacktoMenu()
    {
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(sceneName);
    }
    public void Dead()
    {
        
    }
}
