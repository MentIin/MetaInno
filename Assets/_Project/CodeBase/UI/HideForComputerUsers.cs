using UnityEngine;

public class HideForComputerUsers : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(Input.touchSupported);
    }
}
