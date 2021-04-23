using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{

    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Object that entered the trigger: " + other);

        RubyController controller = other.GetComponent<RubyController>();
        
        

        if (controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                controller.PlaySound(collectedClip);
                Destroy(gameObject);
            }
        }
    }
}
