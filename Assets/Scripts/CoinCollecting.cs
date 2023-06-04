using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinCollecting : MonoBehaviour
{
    public AudioClip CoinSound;

    private int _collectedCoins = 0; // Counter for collected coins
    private AudioSource _mAudioSource;
    private Rigidbody _mRigidBody;
    private int _totalCoins = 0; // Counter for total coins in the scene

    private void Start()
    {
        // Find all coins in the scene based on their tag
        var coins = GameObject.FindGameObjectsWithTag("Coin");
        _totalCoins = coins.Length;
        _mRigidBody = GetComponent<Rigidbody>();
        _mAudioSource = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Coin")) return;

        if (_mAudioSource != null && CoinSound != null) _mAudioSource.PlayOneShot(CoinSound);
        _collectedCoins++;

        Destroy(other.gameObject);
    }

    // Call this method to get the number of collected coins
    public int GetCollectedCoins()
    {
        return _collectedCoins;
    }


    // Call this method to get the number of collected coins
    public int GetTotalCoins()
    {
        return _totalCoins;
    }

    public void ResetCoins()
    {
        _collectedCoins = 0;
    }
    private void Update()
    {
        if (_collectedCoins == _totalCoins)
        {
            // Call scen "finish_game"
            Debug.Log("Startbutton clicked: " + SceneUtility.GetBuildIndexByScenePath("finish_game"));
            SceneManager.LoadSceneAsync(0);
        }
    }
}