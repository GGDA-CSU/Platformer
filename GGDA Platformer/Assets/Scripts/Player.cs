using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
    public PlayerController myController;
    public GameController CurrentController;

    private float horizontal = 0f;
    public int speed = 12;
    private bool jump = false;

    private Animator anim;
    private Rigidbody2D rb;

    public GameObject GameSpawn;

    // Player Controller Instance (Mostly for Testing)
    private static Player _instance = null;
    public static Player Instance { get { return _instance; } }

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        SpawnHere(GameSpawn);
    }

    void Awake()
    {
        CurrentController = FindObjectOfType<GameController>();

        GameSpawn = GameObject.FindGameObjectWithTag("Respawn");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump"))
        {
            //anim.SetFloat("Jump", 1);
            jump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            //anim.SetFloat("Jump", 0);
        }
    }

    void FixedUpdate()
    {
        myController.Move(horizontal * Time.fixedDeltaTime, jump);
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));

        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // When player jumps off platform, change parent
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    public void SpawnHere(GameObject Spawn)
    {
        transform.position = new Vector3(Spawn.transform.position.x, Spawn.transform.position.y, Spawn.transform.position.z);
    }
}
