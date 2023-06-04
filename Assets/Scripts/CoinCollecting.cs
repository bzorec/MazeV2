using System;
using UnityEngine;

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
}