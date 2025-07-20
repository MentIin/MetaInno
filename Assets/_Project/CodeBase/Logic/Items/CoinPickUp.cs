using System;
using DG.Tweening;
using FishNet.Object;
using UnityEngine;

public class CoinPickUp : NetworkBehaviour
{
    public void RequestAddCoin()
    {
        PlayerProgress.Instance.OwnerStatistics.Coins += 1;
    }

    public void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        transform.DOScale(Vector3.zero, 0.4f)
            .SetEase(Ease.OutBack)
            .SetDelay(0.1f)
            .SetLink(gameObject) // Prevents errors if object is destroyed early
            .OnComplete(() => Destroy(gameObject));
        RequestAddCoin();
    }
}
