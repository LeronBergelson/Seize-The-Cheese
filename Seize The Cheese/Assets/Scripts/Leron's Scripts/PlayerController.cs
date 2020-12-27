﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0F;
    public float jumpSpeed = 1.0F;
    public float gravity = 1.0F;
    public float midairSpeed = 1.0F;
    public float groundSpeed = 1.0F;

    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool didJump = false;
    
    //UI
    public bool mainMenuActive = false; //used to prevent other controls + for colsing/opening main menu
    private Canvas mainMenu = null;     //used to reference the main menu's canvas to access 'MainMenu' script and make menu visible/invisible

    private Vector2 moveDirection = Vector2.zero;

    private Vector2 jumpVelocity = Vector2.zero;


    private void Start()
    {
        //locate main menu
        mainMenu = GameObject.Find("Pause Menu").GetComponent<Canvas>();

        //make main menu invisible
        mainMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // iterates through all CubeCheese and ChildCube and drops them whe K is pressed
        {
            canMoveLeft = true;
            canMoveRight = true;
        }
        
        CharacterController controller = GetComponent<CharacterController>();
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        moveDirection = transform.TransformDirection(moveDirection);

        if (controller.isGrounded)
        {
            moveDirection *= groundSpeed;
            if (Input.GetButton("Jump"))
            {
                didJump = true;
                jumpVelocity = moveDirection/1.6f;
                jumpVelocity.y = jumpSpeed;

            }
            else
            {
                didJump = false;
                jumpVelocity = Vector2.zero;

            }
        }
        else
        {
            moveDirection *= midairSpeed;
            jumpVelocity.y -= gravity * Time.deltaTime;

        }



        if (canMoveLeft && canMoveRight || Input.GetAxis("Horizontal") == 0)
                controller.Move((moveDirection + jumpVelocity) * Time.deltaTime);

        else if (canMoveLeft && Input.GetAxis("Horizontal") < 0)
                   controller.Move((moveDirection + jumpVelocity) * Time.deltaTime);

        else if (canMoveRight && Input.GetAxis("Horizontal") > 0)
                   controller.Move((moveDirection + jumpVelocity) * Time.deltaTime);

        else
        {
            if (Input.GetAxis("Horizontal") < 0 && canMoveLeft)
                controller.Move((moveDirection + jumpVelocity) * Time.deltaTime);


            if (Input.GetAxis("Horizontal") > 0 && canMoveRight)
                controller.Move((moveDirection + jumpVelocity) * Time.deltaTime);
        }
        //main menu key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if main menu isn't currently active
            if (mainMenuActive == false) {
                mainMenuActive = true;

                //make main menu visible
                mainMenu.gameObject.SetActive(true);

                //pauses game
                mainMenu.GetComponent<PauseMenu>().PauseGame();
            }
            //if main menu is currently active
            else
            {
                mainMenuActive = false;

                //make main menu invisible
                mainMenu.gameObject.SetActive(false);

                //resumes game
                mainMenu.GetComponent<PauseMenu>().ResumeGame();
            }
        }
    }
    public void ResumePlayer() //called if player uses resume in main menu (closes menu for player)
    {
        mainMenuActive = false;

        //make main menu invisible
        mainMenu.gameObject.SetActive(false);
    }
}