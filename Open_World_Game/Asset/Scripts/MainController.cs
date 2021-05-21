using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{    
    public Camera Mycamera;
    public float speed = 2f;

    public float speedMultiplier = 5f;

    bool mSprinting = false;

    float mDesiredRotation = 0f;
    float mDesiredAnimationSpeed = 0f;

    public float Rotationspeed = 15f;

    public float AnimationBlendSpeed = 2f;

    public float JumpSpeed = 15f;

    float mSpeedy = 0f;
    float mGravity = -9.81f;

    bool mJumping= false;

    Animator Myanimator;
    CharacterController Mycontroller;
    // Start is called before the first frame update
    void Start()
    {
        Mycontroller = GetComponent<CharacterController>();
        Myanimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump") && !mJumping)
        {
            mJumping = true;
            Myanimator.SetTrigger("Jump");

            mSpeedy += JumpSpeed;
        }

        if(!Mycontroller.isGrounded)
        {
            mSpeedy += mGravity * Time.deltaTime;
        }
        
        else if(mSpeedy < 0)
        {
            mSpeedy = 0;
        }

        Myanimator.SetFloat("SpeedY", mSpeedy / JumpSpeed);

        if(mJumping && mSpeedy < 0)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f, LayerMask.GetMask("Default")))
            {
                mJumping = false;
                Myanimator.SetTrigger("Land");
            }
        }

        if(Input.GetKey("c"))
        {
            Myanimator.SetBool("Crouch", true);
            speed = 1f;
        }
        
        if(!Input.GetKey("c"))
        {
            Myanimator.SetBool("Crouch", false);
            speed = 2f;
        }

        if(Input.GetMouseButton(1))
        {
            Myanimator.SetBool("Kick", true);
        }
        
        if(!Input.GetMouseButton(1))
        {
            Myanimator.SetBool("Kick", false);
        }

        if(Input.GetMouseButton(0))
        {
            Myanimator.SetBool("Punch", true);
        }
        
        if(!Input.GetMouseButton(0))
        {
            Myanimator.SetBool("Punch", false);
        }

        if(Input.GetKey("1"))
        {
            Myanimator.SetBool("d1", true);
        }
        
        if(!Input.GetKey("1"))
        {
            Myanimator.SetBool("d1", false);
        }

        if(Input.GetKey("2"))
        {
            Myanimator.SetBool("d2", true);
        }
        
        if(!Input.GetKey("2"))
        {
            Myanimator.SetBool("d2", false);
        }

        mSprinting = Input.GetKey(KeyCode.LeftShift);

        Vector3 movement = new Vector3(x, 0, z).normalized;
        
        Vector3 rotationMovement = Quaternion.Euler(0, Mycamera.transform.rotation.eulerAngles.y, 0) * movement;

        Vector3 VerticalMovement = Vector3.up *mSpeedy;

        Mycontroller.Move((VerticalMovement + (rotationMovement * (mSprinting ? speedMultiplier : speed))) * Time.deltaTime );

        if(rotationMovement.magnitude >0)
        {
        mDesiredRotation = Mathf.Atan2(rotationMovement.x, rotationMovement.z) * Mathf.Rad2Deg;
        mDesiredAnimationSpeed = mSprinting ? 1 : .5f;
        }
        else
        {
            mDesiredAnimationSpeed = 0;
        }
        Myanimator.SetFloat("Speed", Mathf.Lerp(Myanimator.GetFloat("Speed"), mDesiredAnimationSpeed, AnimationBlendSpeed * Time.deltaTime));

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, mDesiredRotation, 0);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Rotationspeed * Time.deltaTime);
    }
}
