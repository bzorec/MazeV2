using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Finish : MonoBehaviour
{
    // Start is called before the first frame update
    public Button start;
    public Button exitButtton;
    void Start()
    {
        start.onClick.AddListener(StartGame);
        exitButtton.onClick.AddListener(ExitGame);
    }

    public void StartGame()
    {
        Debug.Log("Startbutton clicked: " + SceneUtility.GetBuildIndexByScenePath("Playground"));
        SceneManager.LoadSceneAsync(0);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
