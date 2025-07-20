using System;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

public class CoinsLabel : Singleton<CoinsLabel>
{
    [SerializeField] private TextMeshProUGUI coinsLabel;

    public void UpdateCoinsLabel(int coins)
    {
        coinsLabel.text = coins.ToString();
    }
}
