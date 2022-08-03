using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum State
    {
        DROPBOX,
        ENERGY,

    }
    private State state = State.DROPBOX;

    private void SetState(State newState)
    {
        state = newState;
        switch (newState)
        {
            case State.DROPBOX:
                isIdle = false;
                break;
            case State.ENERGY:
                GoToEnergyArea();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private Vector2 firstPressPos;//Mouse firstpress position
    private Vector2 newPressPos;//Mouse secondpress position
    private Vector2 currentSwipe;//Mouse currentSwipe

    [SerializeField] float playerSpeed;// Player movement speed
    [SerializeField] float rotateSpeed;// Player rotation speed
    [SerializeField] float playerX1Limit, playerX2Limit;
    [SerializeField] float playerY1Limit, playerY2Limit;

    private Vector3 lastPos;
    public bool isIdle;

    private CharacterController _controller;
    private Animator _animator;

    private EnergyBar energyBar;
    [SerializeField] Transform EnergyArea;



    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        energyBar = FindObjectOfType<EnergyBar>();

        //lastPos = transform.position;

        playerSpeed = PlayerPrefs.GetFloat("Speed", 3);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            firstPressPos = Input.mousePosition;//get first mouse position

        }

        if (Input.GetMouseButton(0)&& !isIdle)
        {
            Swipe();

            if (lastPos != transform.position) //run anim
            {
                _animator.ResetTrigger("idle");
                _animator.SetTrigger("run");

            }

        }
        else  //idle anim
        {
            _animator.ResetTrigger("run");
            _animator.SetTrigger("idle");
        }


        lastPos = transform.position;//check last position



        PlayerLimits();

        if (transform.hasChanged)
        {
            energyBar.DecreaseEnergyBar();
            transform.hasChanged = false;
            if (energyBar.currentEnergy <= 0)
            {
                SetState(State.ENERGY);
            
            }
        }

      


    }

    private void PlayerLimits()
    {
        if (transform.position.x > playerX1Limit)
        {
            transform.position = new Vector3(playerX1Limit, transform.position.y, transform.position.z);//Position XLimit
        }
        else if (transform.position.x < playerX2Limit)
        {
            transform.position = new Vector3(playerX2Limit, transform.position.y, transform.position.z);//Position XLimit
        }
        if (transform.position.z < playerY1Limit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, playerY1Limit);//Position ZLimit
        }
        else if (transform.position.z > playerY2Limit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, playerY2Limit);//Position ZLimit
        }

    }

    private void Swipe()
    {
        newPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);//get second position

        currentSwipe = new Vector2(newPressPos.x - firstPressPos.x, newPressPos.y - firstPressPos.y);//swipe distance

        currentSwipe.Normalize(); //Touch input

        Vector3 movementInput = new Vector3(currentSwipe.x, 0, currentSwipe.y);
        Vector3 movementDirection = movementInput.normalized;//Touch input

        if (movementDirection != Vector3.zero)//Rotation
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotateSpeed * Time.fixedDeltaTime);//Smooth rotation
        }

        _controller.Move(movementDirection * playerSpeed * Time.fixedDeltaTime); //Movement



    }

    private void GoToEnergyArea()
    {
        isIdle = true;
        transform.DOMove(new Vector3(EnergyArea.position.x,transform.position.y, EnergyArea.position.z), 0.5f).OnComplete(()=> {

            StartCoroutine(CheckEnegryIsFull());
        });
 
       
    }

    private IEnumerator CheckEnegryIsFull()
    {
        yield return new WaitForSeconds(1);
   
        SetState(State.DROPBOX);
        
    }

    public void UpgradeSpeed(float decrease)
    {
        playerSpeed += decrease;
        PlayerPrefs.SetFloat("Speed", playerSpeed);
        PlayerPrefs.Save();
        isIdle = false;
        print(playerSpeed);
    }
}
