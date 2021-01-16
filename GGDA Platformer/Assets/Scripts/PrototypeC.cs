using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

// () Indicates added code

public enum PlayerState
{
    walk,
    idle,
    jump,
    dead,
    interact,
}

public class PrototypeC : MonoBehaviour
{
    /*() Used for switch casing inputs */
    private string input;

    /*() Jump variable */
    private bool jump = false;

    private Animator anim;
    [SerializeField] private PlayerState currentState;
    [SerializeField] private float speed = 50f;

    /*() Added a force variable for jumping */
    private float force;

    private Rigidbody2D myRigidbody;

    /*() Set change variable */
    private Vector3 change = Vector3.zero;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();

        /*()  Initialized PlayerState on start */
        currentState = PlayerState.idle;

        /*() Removed the setting of animation variables */
    }

    private void FixedUpdate()
    {
        KeyboardMovement();
        ControllerMovement();

        switch (currentState)
        {
            //checks if player is in an interaction
            case PlayerState.interact:
                /*() I'm guessing there will be a little pause if the player interacts. This will handle that */
                StartCoroutine(InteractionSeq());
                break;

            case PlayerState.idle:
                anim.SetBool("moving", false);
                MoveCharacter(Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime, jump);
                break;

            case PlayerState.jump:
                jump = true;
                anim.SetBool("jump", true);
                myRigidbody.velocity = Vector2.up * 10f;
                anim.SetBool("jump", false);
                jump = false;

                currentState = PlayerState.idle;
                break;

            case PlayerState.walk:
                MoveCharacter(Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime, jump);
                break;

            case PlayerState.dead:
                // We'll go over this
                break;
        }
    }

    /*()*/
    private void KeyboardMovement()
    {
        input = Input.inputString;
        if (!string.IsNullOrEmpty(input))
        {
            switch (input)
            {
                case (" "):
                    currentState = PlayerState.jump;
                    break;

                case ("w"):
                    //PlayerState.climb = true;
                    break;

                case ("a"):
                    currentState = PlayerState.walk;
                    break;

                case ("s"):
                    //PlayerState.climb = true;
                    break;

                case ("d"):
                    currentState = PlayerState.walk;
                    break;
            }
        }
    }

    /*()*/
    private void ControllerMovement()
    {
        // Based on keyboard function. Set up a switch:case block for controller buttons
    }

    private void MoveCharacter(float move, bool jump)
    {
        /*()*/
        if (move != 0)
        {

            Debug.Log(move);
            anim.SetBool("moving", true);

            change.x = move;
            change.Normalize();

            myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        }
        else if (move == 0)
        {
            Debug.Log(move);
            currentState = PlayerState.idle;
        }
    }

    private IEnumerator InteractionSeq()
    {
        yield return new WaitForSeconds(0.8f);
    }
}
