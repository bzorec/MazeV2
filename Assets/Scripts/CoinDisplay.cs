using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    public CoinCollecting coinCollector;
    private TextMeshProUGUI coinText;

    private void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        int collectedCoins = coinCollector.GetCollectedCoins();
        int totalCoins = coinCollector.GetTotalCoins();
        coinText.text = "Coins: " + collectedCoins + " / " + totalCoins;
    }
}