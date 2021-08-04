using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Torch playerTorch = other.gameObject.GetComponentInChildren<Torch>();
        playerTorch.RechargeBattery();

        Destroy(gameObject);
    }

}
