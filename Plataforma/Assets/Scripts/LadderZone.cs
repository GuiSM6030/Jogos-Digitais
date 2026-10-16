using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LadderZone : MonoBehaviour
{
    private readonly HashSet<PlayerMovement> playersInside = new HashSet<PlayerMovement>();

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
        if (player != null && playersInside.Add(player))
        {
            player.EnterLadderZone(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
        if (player != null)
        {
            RemovePlayer(player);
        }
    }

    private void OnDisable()
    {
        foreach (PlayerMovement player in playersInside)
        {
            if (player != null)
            {
                player.ExitLadderZone(this);
            }
        }

        playersInside.Clear();
    }

    private void RemovePlayer(PlayerMovement player)
    {
        if (playersInside.Remove(player))
        {
            player.ExitLadderZone(this);
        }
    }
}