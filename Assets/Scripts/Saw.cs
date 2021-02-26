using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.transform.GetComponentInParent<PlayerController>();
        if (player != null)
            player.bloodStream.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.transform.GetComponentInParent<PlayerController>();
        if (player != null)
            player.bloodStream.SetActive(false);
    }
}
