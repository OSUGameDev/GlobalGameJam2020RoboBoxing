using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        transform.GetChild(0).gameObject.SetActive(isPaused);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            isPaused = !isPaused;
            transform.GetChild(0).gameObject.SetActive(isPaused);
            Time.timeScale = isPaused ? 0 : 1;
            AudioListener.pause = !AudioListener.pause;
        }
    }
    void OnLevelWasLoaded(int level)
    {
        Resume();
    }
    public void Resume()
    {
        isPaused = false;
        transform.GetChild(0).gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        AudioListener.pause = false;
    }
    public void ExitApplication()
    {
        Application.Quit();
    }
    public void ExitToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
