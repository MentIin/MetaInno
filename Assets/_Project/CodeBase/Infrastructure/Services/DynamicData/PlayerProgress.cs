using System;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class PlayerProgress : NetSingleton<PlayerProgress>
{
    private readonly SyncDictionary<int, PlayerStatistics> _playerStatistics = new SyncDictionary<int, PlayerStatistics>();
    private PlayerStatistics _ownerStatistics;
    
    public PlayerStatistics OwnerStatistics => _ownerStatistics;

    public override void OnStartClient()
    {
        if (!_playerStatistics.ContainsKey(Owner.ClientId))
            _playerStatistics[Owner.ClientId] = new PlayerStatistics();
        
        _ownerStatistics = _playerStatistics[Owner.ClientId];
    }

    private void Update()
    {
        if (!IsClientStarted)
            return;
        
        CoinsLabel.Instance.UpdateCoinsLabel(_ownerStatistics.Coins);
    }
}

public class PlayerStatistics
{
    public int Coins = 0;
}
