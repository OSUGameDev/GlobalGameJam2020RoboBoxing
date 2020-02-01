using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    public List<GameObject> Menus = new List<GameObject>();
    private Stack<GameObject> menuHistory = new Stack<GameObject>();
    private GameObject currentMenu;

    // Start is called before the first frame update
    void Start()
    {
        ChangeMenu(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackButton()
    {
        if(menuHistory.Count < 1)
        {
            Debug.LogError("Tried to pop menu from history with no menus");
        }
        currentMenu.SetActive(false);
        currentMenu = menuHistory.Pop();
        currentMenu.SetActive(true);
    }

    public void ChangeMenu(int menuIndex)
    {
        if(menuIndex < 0 || menuIndex >= Menus.Count)
        {
            Debug.LogError("Tried to access a menu that does not exist:" + menuIndex);
            return;
        }

        if(currentMenu != null)
        {
            menuHistory.Push(currentMenu);
        }
        Menus.ForEach(menu => menu.SetActive(false));
        currentMenu = Menus[menuIndex];
        currentMenu.SetActive(true);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
