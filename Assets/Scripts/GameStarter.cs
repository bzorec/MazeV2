using UnityEngine.SceneManagement;
using UnityEngine;


public class GameStarter : MonoBehaviour
{
    public string menuSceneName;

    void Start()
    {
        SceneManager.LoadScene("start_menu");
    }
}
