using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class GameManage : MonoBehaviour
{
    // Start is called before the first frame update
    public Button start;
    public Button pauseButton;
    public Button resetButton;

    void Start()
    {
        start.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(PauseGame);
        resetButton.onClick.AddListener(ResetGame);

    }
    public void StartGame()
    {
        Debug.Log("Startbutton clicked: " + SceneUtility.GetBuildIndexByScenePath("Playground"));
        SceneManager.LoadSceneAsync(0);


        // nadomestite "Ime prizora za igro" z imenom prizora, kjer se zaène igra
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResetGame()
    {
        SceneManager.LoadScene("playground");
        Time.timeScale = 1f;

    }
    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("start_menu");
            Time.timeScale = 1f;
        }
        // Ostali obstoječi del kode Update metode...
    }

}