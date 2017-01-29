//This script was written by Josh Deal | Last edited by Josh | Modified on Jan 28, 2017
using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    GameObject player;
	private Vector2 focus_area_size; // controls the size of the focus for the camera
	private float vertical_offset = 0f; // controls how much the camera is offset vertically from the focus
	private float look_ahead_x = 6f; // controls how far the camera will look ahead of the focus
	private float look_smooth_time_x = 0.6f; // controls the time it takes the camera to move toward the look_ahead_x
	private float vertical_smooth_time = 0.1f; // controls how fast the camera will move in the y direction

	FocusArea focus_area;

	private float current_look_aheadX;
	private float target_look_ahead_x;
	private float look_ahead_dir_x;
	private float smooth_look_velocity_x;
	private float smooth_velocity_y;
	private bool look_ahead_stopped;

	// Use this for initialization
	void Start ()
    {
        focus_area_size = new Vector2(2, 6);
		player = GameObject.FindGameObjectWithTag ("Player");
		focus_area = new FocusArea (player.GetComponent<BoxCollider2D>().bounds, focus_area_size);
	}

	void FixedUpdate ()
    {
		focus_area.Update (player.GetComponent<BoxCollider2D>().bounds);

		//sets the vertical offset of the camera relative to player
		Vector2 focusPosition = focus_area.center + Vector2.up * vertical_offset;


		//Checks the look ahead depending on focus velocity
		//if the focusArea is moving in x, continue looking ahead
		if (focus_area.velocity.x != 0)
        {
			look_ahead_dir_x = Mathf.Sign (focus_area.velocity.x);
			if ((Mathf.Sign (player.GetComponent<Player>().move.x) == Mathf.Sign (focus_area.velocity.x)) && player.GetComponent<Player>().move.x != 0)
            {
				look_ahead_stopped = false;
				target_look_ahead_x = look_ahead_dir_x * look_ahead_x;
			} 
		}

		//if the focusArea is not moving in x, ease into movement
		else
        {
			if (!look_ahead_stopped)
            {
				look_ahead_stopped = true;
				target_look_ahead_x = current_look_aheadX + (look_ahead_dir_x * look_ahead_x - current_look_aheadX) / 4;
			}
		}

		//sets the look ahead depending on direction moving and distance set
		current_look_aheadX = Mathf.SmoothDamp (current_look_aheadX, target_look_ahead_x, ref smooth_look_velocity_x, look_smooth_time_x);

		focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smooth_velocity_y, vertical_smooth_time);
		focusPosition += Vector2.right * current_look_aheadX; //Executes LookAhead
		transform.position = (Vector3)focusPosition + Vector3.forward * -10; //Transforms Camera position
	}

	/*void OnDrawGizmos()
    {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focus_area.center, focus_area_size);
	}*/

    // this struct defines the variables of the focus 
	struct FocusArea
    {
		public Vector2 center;
		public Vector2 velocity;
		public float left, right;
		public float top, bottom;

        // this is where the size and center of the focus is determined
		public FocusArea(Bounds target_bounds, Vector2 size)
        {
            // defines the left boundary of focus
			left = target_bounds.center.x - size.x/2;
            // defines the right boundary of focus
            right = target_bounds.center.x + size.x/2;
            // defines the bottom boundary of focus
            bottom = target_bounds.min.y;
            // defines the top boundary of focus
            top = target_bounds.min.y + size.y;

			velocity = Vector2.zero; // set the default velocity to 0
            center = new Vector2((left+right)/2, (top +bottom)/2); // defines where the center of the focus is by the boundaries
		}

		public void Update(Bounds target_bounds)
        {
			float shiftX = 0;
            // if the target is moving left and the target focus is less than the current bounded position
			if (target_bounds.min.x < left)
            {
                // move the target focus to the left
				shiftX = target_bounds.min.x - left;
			}
            // if the target is moving right and the target focus is greater than the current bounded position
            else if (target_bounds.max.x > right)
            {
                // move the target focus to the right
				shiftX = target_bounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			float shiftY = 0;
            // if the target is moving down and the target focus is less than the current bounded position
            if (target_bounds.min.y < bottom)
            {
                // move the target focus down
				shiftY = target_bounds.min.y - bottom;
			}
            // if the target is moving up and the target focus is greater than the current bounded position
            else if (target_bounds.max.y > top)
            {
                // move the target focus up
				shiftY = target_bounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;

			center = new Vector2 ((left + right) / 2, (top + bottom) / 2); // update the center of the focus depending on where the boundary has moved
			velocity = new Vector2 (shiftX, shiftY); // the new velocity equals the change in movement in th x and y directions
		}
	}
}