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
    public Button continueButton;
    public Button resetButton;
    public Button exitButtton;

    public int savedCurrentAmmoInMag;
    public int savedCurrentAmmo;

    void Start()
    {
        start.onClick.AddListener(StartGame);
        continueButton.onClick.AddListener(ContinueGame);
        resetButton.onClick.AddListener(ResetGame);
        exitButtton.onClick.AddListener(ExitGame);

    }
    public void StartGame()
    {
        Debug.Log("Startbutton clicked: " + SceneUtility.GetBuildIndexByScenePath("Playground"));
        SceneManager.LoadSceneAsync(0);
        // nadomestite "Ime prizora za igro" z imenom prizora, kjer se zaène igra
    }

    public void ContinueGame()
    {
        CoinCollecting coinCollecting = FindObjectOfType<CoinCollecting>();
        if (coinCollecting != null)
        {
            coinCollecting.GetCollectedCoins();
        }

        Gun gun = FindObjectOfType<Gun>();
        if (gun != null)
        {
            savedCurrentAmmoInMag = gun.gunData.currentAmmoInMag;
            savedCurrentAmmo = gun.gunData.currentAmmo;
        }

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

    void ResetGame()
    {
        Debug.Log("Startbutton clicked: " + SceneUtility.GetBuildIndexByScenePath("Playground"));
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1f;

        // Ponastavi metke
        Gun gun = FindObjectOfType<Gun>();
        if (gun != null)
        {
            gun.ResetBullets();
        }

        CoinCollecting coinCollecting = FindObjectOfType<CoinCollecting>();
        if (coinCollecting != null)
        {
            coinCollecting.ResetCoins();
        }

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