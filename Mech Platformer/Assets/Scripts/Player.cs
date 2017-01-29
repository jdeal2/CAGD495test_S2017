//This script was written by Josh Deal | Last edited by Josh | Modified on Jan 28, 2017
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour 
{
    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool dead;
    [HideInInspector]
    public bool has_won;
    [HideInInspector]
    public Vector3 move;

    private GameObject ground_checker;
    private Vector2 velocity;
    private float speed = 10.0f;
    private float jump_power = 800f;
    private float move_horizontal;
    private bool facing_right;
    private bool attacking;

	// Use this for initialization
	void Start () 
	{
        facing_right = true;
        grounded = true;
        dead = false;
        has_won = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
        VerticalMovement();
        HorizontalMovement();
        Death();
        Win();
        Attacking();
    }

    // This handles all the characters horizontal movement
    void HorizontalMovement()
    {
        //This is where input for horizontal movement happens
        move_horizontal = Input.GetAxis("Horizontal");

        //Horizontal Movement
        move = new Vector2(move_horizontal * speed, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<Rigidbody2D>().velocity = move;

        // Flip the player depending on horizontal movement
        if (move_horizontal > 0 && !facing_right)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move_horizontal < 0 && facing_right)
            // ... flip the player.
            Flip();
    }

    // This handles all the characters vertical movement
    void VerticalMovement()
    {
        //Jumping Movement
        if (Input.GetKey("up") && grounded)
        {
            grounded = false; // set grounded to false
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jump_power), ForceMode2D.Force); // add vertical force
        }
    }

    // handles what happens if a character dies
    void Death()
    {
        if (dead)
        {
            SceneManager.LoadScene(0); // reloads the scene
        }
    }

    // handles what happens if a character gets to the end
    void Win()
    {
        if (has_won)
        {
            SceneManager.LoadScene(1); // loads the victory scene
        }
    }

    // handles the player attack functions
	void Attacking () 
	{

	}

    // function in control of flipping the sprite
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facing_right = !facing_right;

        // Multiply the player's x local scale by -1.
        Vector3 the_scale = transform.localScale;
        the_scale.x *= -1;
        transform.localScale = the_scale;
    }
}
