//This script was written by Josh Deal | Last edited by Josh | Modified on Jan 28, 2017
using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        // check to see if player is grounded
        if (col.tag == "Ground")
        {
            player.grounded = true;
        }
        // check to see if the player has hit a pit
        if (col.tag == "Pit")
        {
            player.dead = true;
        }
        else
            return;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // check to see if the player has gotten to the exit
        if (col.tag == "Exit")
        {
            player.has_won = true;
            Debug.Log(player.has_won);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // if the player leaves the ground they are not grounded
        player.grounded = false;
    }
}
