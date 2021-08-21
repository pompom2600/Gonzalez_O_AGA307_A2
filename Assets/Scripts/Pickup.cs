using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupTypes { Battery, Heart}

public class Pickup : MonoBehaviour
{
    public PickupTypes pickup = PickupTypes.Battery;


    private void OnTriggerEnter2D(Collider2D other)
    {

        switch (pickup)
        {
            case PickupTypes.Battery:
                Torch playerTorch = other.gameObject.GetComponentInChildren<Torch>();
                playerTorch.RechargeBattery();


                break;
            case PickupTypes.Heart:
                PlayerMovement player = other.gameObject.GetComponentInChildren<PlayerMovement>();
                player.HealthRegn();
                break;

        }
        Destroy(gameObject);
    }




}
