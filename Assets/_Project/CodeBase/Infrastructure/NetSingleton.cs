using FishNet.Object;
using UnityEngine;

public class NetSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindInstance();
            }

            return _instance;
        }
    }

    private static T FindInstance()
    {
        var items = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (items.Length == 0)
        {
            Debug.Log($"No have Instances type of {typeof(T)}");
            return null;
        }
        else if (items.Length == 1)
        {
            return items[0];
        }
        else
        {
            string log = $"Multiple Instances type of {typeof(T)} | Count - {items.Length}";
            foreach (var item in items)
            {
                if (item.isActiveAndEnabled)
                {
                    Debug.Log($"{log}\nSet Instance with name - {item.name}");
                    return item;
                }
            }

            Debug.LogWarning($"{log}\nSet Instance with name - {items[0].name}");
            return items[0];
        }
    }
}