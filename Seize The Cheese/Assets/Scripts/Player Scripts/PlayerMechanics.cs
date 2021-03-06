﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMechanics : MonoBehaviour
{
    public float Maxhealth; // health variable
    public float Currenthealth; //current heath variable
    public MousyMovement playerMovement = null;

    //various panels that appear once the player makes contact a specific object or has died
    //public GameObject introPanel;
    //public GameObject DustPanel;
    //public GameObject healthCheesePanel;
    //public GameObject strongCheesePanel;
    //public GameObject deathPanel;
    public GameObject holder;
    //public GameObject emptySlot;

    public float amount;

    public Text txt; // text field gameobject which is used in the countdown timer appears for the power up

    // boolean variables associated to various powerups and enemies.
    public bool touchedStrongCheese = false;
    public bool touchedHealthCheese = false;
    public bool touchedDust = false;
    public bool touchedSpider = false;

    public bool onStrongCheese = false; // checks to see if the player who at the time of eating strong cheese, is currently on it 
    public bool dead = false; // checks to see if the player is dead
    public float timeRemaining = 6; // sets the countdown timer value from 6 seconds
    public bool notInDialog = true; //prevents strong cheese from counting down if in dialog

    //bryan's cheese block variables
    public bool pickedUpCheese = false;
    public bool pickedUpCheese2 = false;
    public GameObject cheeseBlock = null;
    public GameObject secondCheeseBlock = null;
    public byte collisionDirection = 0;     //0 = neither, 1 = right, 2 = left
    public bool endOfLevel = false;

    public bool hitDustBunnyVideo = false;



    //animations
    Animator animator;

 
    private void OnCollisionStay(Collision collision)
    {
        //get contacts for collision
        Vector2 direction = collision.GetContact(0).normal;

        //if hit block of cheese then call function to pick it up
        if (collision.gameObject.name.CompareTo("Cheese") == 0 && cheeseBlock == null)
        {
            //set the cheese block one touch one
            cheeseBlock = collision.gameObject;

            //hit from right side- put block on left side
            if (direction.x > 0)
            {
                collisionDirection = 2;
            }
            //hit from left side- put block on right side
            else if (direction.x < 0)
            {
                collisionDirection = 1;
            }
        }
        else if (collision.gameObject.name.CompareTo("Cheese") == 0 && pickedUpCheese == true && secondCheeseBlock == null)
        {
            //set the cheese block one touch one
            secondCheeseBlock = collision.gameObject;

            //hit from right side- put block on left side
            if (direction.x > 0)
            {
                collisionDirection = 2;
            }
            //hit from left side- put block on right side
            else if (direction.x < 0)
            {
                collisionDirection = 1;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //set the cheese block to null if not touching
        if (cheeseBlock != null && pickedUpCheese == false)
        {
            cheeseBlock = null;
        }
        else if (secondCheeseBlock != null && pickedUpCheese2 == false)
        {
            secondCheeseBlock = null;
        }
        collisionDirection = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dust")) // when player touches a 'Dust Bunny'
        {
            Debug.Log("Bunny Hit!");
            
            if (!onStrongCheese)
            {
                animator.SetBool("ishit", true);

                Currenthealth -= 5; // lowers current health by half

                //play dust bunny video
                if (hitDustBunnyVideo == false)
                {
                    playerMovement.CallVideoPlayer();
                    hitDustBunnyVideo = true;
                }

                if (Currenthealth <= 0) // if health is equal to 0, player is dead thus cursor is visible and a death panel appears
                {
                    Debug.Log("Dead");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
                    //Cursor.visible = true;
                    //deathPanel.SetActive(true);
                    //PauseGame();
                    //dead = true;

                }
                Invoke("EndHit", 1);
            }

            Destroy(collision.gameObject); // destorys the dust bunny

        }

        else if (collision.gameObject.CompareTag("HealthCheese")) // when the player interacts with a 'Health Cheese' power up
        {
            if (Currenthealth < 10) 
            {
                Currenthealth += 5;
            }
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("StrongCheese"))  // when the player interacts with 'StrongCheese' 
        {
            onStrongCheese = true;
            Destroy(collision.gameObject);

        }
    }

    void EndHit()
    {
        animator.SetBool("ishit", false);
    }

    void ResumeGame()
    {
        //introPanel.SetActive(false); // sets introPanel innactive
        //strongCheesePanel.SetActive(false); // sets introPanel innactive
        //healthCheesePanel.SetActive(false); // sets introPanel innactive
        //DustPanel.SetActive(false); // sets introPanel innactive

        if (!dead) // if is not dead continue the game
        {
            Time.timeScale = 1;
        }
    }


    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;

        playerMovement = GetComponent<MousyMovement>();

        animator = GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        if (endOfLevel == false) {
            if (pickedUpCheese && cheeseBlock != null)
            {
                cheeseBlock.transform.GetComponent<Rigidbody>().velocity = gameObject.transform.GetComponent<Rigidbody>().velocity;
            }
            if(pickedUpCheese2 && secondCheeseBlock != null)
            {
                secondCheeseBlock.transform.GetComponent<Rigidbody>().velocity = gameObject.transform.GetComponent<Rigidbody>().velocity;
            }

            if (Input.GetKeyDown(KeyCode.Return)) // if enter is pressed the game continues unless the player is dead.
            {
                //Debug.Log("Return key was pressed.");
                ResumeGame();
            }

            //pickup cheese
            if (cheeseBlock != null && pickedUpCheese == false && Input.GetKeyDown(KeyCode.E))
            {
                //indicate cheese was picked up
                pickedUpCheese = true;

                //move cheese block
                cheeseBlock.transform.GetComponent<Rigidbody>().MovePosition(new Vector3(transform.position.x + 0.5f * playerMovement.GetPlayerDirection(), transform.position.y + 0.4f, transform.position.z));

                //triggers animation
                animator.SetBool("isholdingcheese", true);

                Debug.Log("PlayerMechanics: Picked up first cheese");

                //prevent rigidbody from moving block while being carried
                cheeseBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                cheeseBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

                Debug.Log("CheeseBlock: After setting parent transform");

                //tell block it's been picked up
                cheeseBlock.GetComponent<CheeseBlock>().PickedUp(collisionDirection, false);

                //reset collision drection
                collisionDirection = 0;     
            }
            else if (secondCheeseBlock != null && pickedUpCheese == true && pickedUpCheese2 == false && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Picked up second cheese");

                pickedUpCheese2 = true;

                //move second cheese block
                secondCheeseBlock.transform.GetComponent<Rigidbody>().MovePosition(new Vector3(transform.position.x + 0.5f * playerMovement.GetPlayerDirection(), transform.position.y + 1.4f, transform.position.z));

                Debug.Log("Second Cheese pickup");

                //prevent rigidbody from moving block while being carried
                secondCheeseBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                secondCheeseBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

                //tell block it's been picked up
                secondCheeseBlock.GetComponent<CheeseBlock>().PickedUp(collisionDirection, true);

                //reset collision drection
                collisionDirection = 0;
            }
            //release cheese if holding one
            if (cheeseBlock != null && pickedUpCheese == true && Input.GetKeyDown(KeyCode.R))
            {
                DropCheese();
                //stops animation
                animator.SetBool("isholdingcheese", false);
            }

            //if strong cheese effect is active
            if (onStrongCheese)
            { // when the player picks up strong cheese a count down timer aprears inside a panel
                if (timeRemaining > 0 && notInDialog == true)
                {
                    //Debug.Log(timeRemaining);
                    holder.SetActive(true);
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }

                else if (timeRemaining <= 0) // once the time reaches 0 on the timer the panel disappears
                {
                    //Debug.Log("Done");
                    holder.SetActive(false);
                    onStrongCheese = false;
                    timeRemaining = 11;
                }
            }
        }
    }
    // calculation for the timer
    void DisplayTime(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        txt.text = string.Format("Power Timer: {00:0}", seconds);
    }
    public void SetInDialog(bool status)
    {
        notInDialog = status;
    }
    public void DropCheese()
    {
        Debug.Log("PlayerMechanics: DropCheese() called");
        pickedUpCheese = false;

        //indicate to cheese that it has been dropped
        cheeseBlock.GetComponent<CheeseBlock>().Dropped();

        //set constraints for rigidbody
        Rigidbody cheeseRigid = cheeseBlock.GetComponent<Rigidbody>();
        cheeseRigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        //null reference
        cheeseBlock = null;

        //drop second cheese
        if (secondCheeseBlock != null && pickedUpCheese2 == true)
        {
            pickedUpCheese2 = false;

            //indicate to cheese that it has been dropped
            secondCheeseBlock.GetComponent<CheeseBlock>().Dropped();

            //set constraints for rigidbody
            Rigidbody cheeseRigid2 = secondCheeseBlock.GetComponent<Rigidbody>();
            cheeseRigid2.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

            //null reference
            secondCheeseBlock = null;
        }
        UnPreventPlayerMovement(0);
        UnPreventPlayerMovement(1);
    }
    public void CheeseBlockHit(GameObject cheese, byte status)
    {
        //reference cheese block if hit
        if (status == 0 && secondCheeseBlock == null)
        {
            secondCheeseBlock = cheese;
        }
        else if (status == 1)
        {
            secondCheeseBlock = cheese;
        }
    }
    public void UpdateCheeseDirection(bool isFacingRight)
    {
        //if holding first block
        if (cheeseBlock != null && pickedUpCheese == true)
        {
            cheeseBlock.GetComponent<CheeseBlock>().UpdateCheeseDirection(isFacingRight);
        }
        //if holding a second cheese block
        if (secondCheeseBlock != null && pickedUpCheese2 == true)
        {
            secondCheeseBlock.GetComponent<CheeseBlock>().UpdateCheeseDirection(isFacingRight);
        }
    }
    public void PreventPlayerMovement(sbyte direction)
    {
        if (direction == 0)
        {
            GetComponent<MousyMovement>().PreventPlayerMovement(0);
        }
        else if (direction == 1)
        {
            GetComponent<MousyMovement>().PreventPlayerMovement(1);
        }
    }
    public void UnPreventPlayerMovement(byte direction)
    {
        if (direction == 0)
        {
            GetComponent<MousyMovement>().UnPreventPlayerMovement();
        }
        else if (direction == 1)
        {
            GetComponent<MousyMovement>().UnPreventPlayerMovement();
        }
    }
    public void UpdateCheeseBlockPosition()
    {
        if (cheeseBlock != null && pickedUpCheese)
        {
            //move cheese block
            cheeseBlock.transform.GetComponent<Rigidbody>().MovePosition(new Vector3(transform.position.x + 0.5f * playerMovement.GetPlayerDirection(), transform.position.y + 0.4f, transform.position.z));
        }
        if (secondCheeseBlock != null && pickedUpCheese2)
        {
            //move second cheese block
            secondCheeseBlock.transform.GetComponent<Rigidbody>().MovePosition(new Vector3(transform.position.x + 0.5f * playerMovement.GetPlayerDirection(), transform.position.y + 1.4f, transform.position.z));
        }
    }
}

