using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerPosition { Left = -2, Middle = 0, Right = 2 }

public class PlayerController : MonoBehaviour
{
    private PlayerPosition playerPosition;
    private Transform playerTransform;
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private float newXPosition;
    private float xPosition;
    private PlayerCollision playerCollision;
    private Animator playerAnimator;
    private int IdDogLeft = Animator.StringToHash("DodgeLeft");
    private int IdDogRight = Animator.StringToHash("DodgeRight");
    private int IdJump = Animator.StringToHash("Jump");
    private int IdFall = Animator.StringToHash("Fall");
    private int idLanding = Animator.StringToHash("Landing");
    private int idRoll = Animator.StringToHash("Roll");

    private int _idStumbleLow = Animator.StringToHash("StumbleLow");
    private int _idStumbleCornerLeft = Animator.StringToHash("StumbleCornerLeft");
    private int _idStumbleCornerRight = Animator.StringToHash("StumbleCornerRight");
    private int _idStumbleFall = Animator.StringToHash("StumbleFall");
    private int _idStumbleOffLeft = Animator.StringToHash("StumbleOffLeft");
    private int _idStumbleOffRight = Animator.StringToHash("StumbleOffRight");
    private int _idStumbleSideLeft = Animator.StringToHash("StumbleSideLeft");
    private int _idStumbleSideRight = Animator.StringToHash("StumbleSideRight");
    public int IdStumbleFall { get => _idStumbleFall; set => _idStumbleFall = value; }
    public int IdStumbleCornerRight { get => _idStumbleCornerRight; set => _idStumbleCornerRight = value; }
    public int IdStumbleLow { get => _idStumbleLow; set => _idStumbleLow = value; }
    public int IdStumbleOffRight { get => _idStumbleOffRight; set => _idStumbleOffRight = value; }
    public int IdStumbleSideLeft { get => _idStumbleSideLeft; set => _idStumbleSideLeft = value; }
    public int IdStumbleOffLeft { get => _idStumbleOffLeft; set => _idStumbleOffLeft = value; }
    public int IdStumbleCornerLeft { get => _idStumbleCornerLeft; set => _idStumbleCornerLeft = value; }
    public int IdStumbleSideRight { get => _idStumbleSideRight; set => _idStumbleSideRight = value; }
    private int _idDeathLower = Animator.StringToHash("DeathLower");
    private int _idDeathMovingTrain = Animator.StringToHash("DeathMovingTrain");
    private int _idDeahthUpper = Animator.StringToHash("DeathUpper");
    private int _idDeathBounce = Animator.StringToHash("DeathBounce");
    public int IdDeathLower { get => _idDeathLower; set => _idDeathLower = value; }
    public int IdDeathMovingTrain { get => _idDeathMovingTrain; set => _idDeathMovingTrain = value; }
    public int IdDeahthUpper { get => _idDeahthUpper; set => _idDeahthUpper = value; }
    public int IdDeathBounce { get => _idDeathBounce; set => _idDeathBounce = value; }

    private CharacterController _characterController;
    public CharacterController myCharacterController { get => _characterController; set => _characterController = value; }

    private Vector3 motionVector;
    private float yPosition;

    [Header("Player Character Controller")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float dodgeSpeed;
    private float rollTime;
    [Header("Player States")]
    [SerializeField] private bool isJumpin;
    [SerializeField] private bool isRolling;
    [SerializeField] private bool isGrounded;


    private void Awake()
    {
        playerCollision = GetComponent<PlayerCollision>();
        playerPosition = PlayerPosition.Middle;
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        yPosition = -7;
    }
    private void Start()
    {

    }
    void Update()
    {
        GetSwipe();
        SetPlayerPosition();
        MovePlayer();
        JumPlayer();
        Roll();
        isGrounded = _characterController.isGrounded;
    }


    private void GetSwipe()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.RightArrow);
        swipeUp = Input.GetKeyDown(KeyCode.UpArrow);
        swipeDown = Input.GetKeyDown(KeyCode.DownArrow);

    }

    private void SetPlayerPosition()
    {
        if (swipeLeft && !isRolling /*&& !isJumpin*/)
        {
            if (playerPosition == PlayerPosition.Middle)
            {
                UpdatePlayerXPosition(PlayerPosition.Left);
                SetPlayerAnimator(IdDogLeft, false);
            }
            else if (playerPosition == PlayerPosition.Right)
            {
                UpdatePlayerXPosition(PlayerPosition.Middle);
                SetPlayerAnimator(IdDogLeft, false);
            }
        }
        else if (swipeRight && !isRolling /*&& !isJumpin*/)
        {
            if (playerPosition == PlayerPosition.Middle)
            {
                UpdatePlayerXPosition(PlayerPosition.Right);
                SetPlayerAnimator(IdDogRight, false);
            }
            else if (playerPosition == PlayerPosition.Left)
            {
                UpdatePlayerXPosition(PlayerPosition.Middle);
                SetPlayerAnimator(IdDogRight, false);
            }
        }

    }



    private void UpdatePlayerXPosition(PlayerPosition plPosition)
    {
        newXPosition = (int)plPosition;
        playerPosition = plPosition;
    }

    public void SetPlayerAnimator(int id, bool IsCrossFade, float FadeTime = 0.1f)
    {
        playerAnimator.SetLayerWeight(0, 1);
        if (IsCrossFade)
        {
            playerAnimator.CrossFadeInFixedTime(id, FadeTime);
        }
        else
        {
            playerAnimator.Play(id);
        }
        ResetCollision();
    }
    public void SetPlayerAnimatorWithLayer(int id)
    {
        playerAnimator.SetLayerWeight(1, 1);
        playerAnimator.Play(id, 1);
        ResetCollision();
    }
    private void ResetCollision()
    {
        Debug.Log(playerCollision.CollisionX.ToString() + " " + playerCollision.CollisionY.ToString() + " " + playerCollision.CollisionZ.ToString());
        playerCollision.CollisionX = CollisionX.None;
        playerCollision.CollisionY = CollisionY.None;
        playerCollision.CollisionZ = CollisionZ.None;
    }

    private void MovePlayer()
    {
        motionVector = new Vector3((xPosition - playerTransform.position.x), yPosition * Time.deltaTime, forwardSpeed * Time.deltaTime);
        xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed);
        //playerTransform.position = new Vector3(xPosition, 0, 0);
        _characterController.Move(motionVector);
    }
    private void JumPlayer()
    {
        if (_characterController.isGrounded)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            {
                isJumpin = false;
                SetPlayerAnimator(idLanding, false);
            }
            if (swipeUp && !isRolling)
            {
                isJumpin = true;

                yPosition = jumpPower;
                //playerAnimator.CrossFadeInFixedTime(IdJump, 0.1f);
                SetPlayerAnimator(IdJump, true);
            }
        }
        else
        {
            yPosition -= jumpPower * 2 * Time.deltaTime;
            if (_characterController.velocity.y <= 0)
            {
                SetPlayerAnimator(IdFall, false);
            }
        }
    }
    private void Roll()
    {
        rollTime -= Time.deltaTime;
        if (rollTime <= 0)
        {
            isRolling = false;
            rollTime = 0f;
            _characterController.center = new Vector3(0, 0.45f, 0);
            _characterController.height = 0.9f;
        }
        if (swipeDown && isGrounded)
        {
            isRolling = true;
            rollTime = 0.5f;
            SetPlayerAnimator(idRoll, true);
            _characterController.center = new Vector3(0, 0.2f, 0);
            _characterController.height = 0.4f;
        }
    }

}
