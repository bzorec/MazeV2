using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

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
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
