using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Material yellow;
    public GameObject speedForce;
    public GameObject lightning;

    public static bool isRunning;
    public static bool isWalking;
    public static bool isAir;
    public static bool superSpeed;

    public float groundDistance = 0.4f;
    public float walkSpd = 6f;
    public float runSpd = 6f;
    public float superRunSpd = 100f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float speedUpTime = 10f;

    Vector3 velocity;
    float turnSmoothVelocity;
    bool isGrounded;
    bool speedForceCheck;

    private void Start()
    {
        speedForceCheck = false;
        Cursor.visible = false;
        yellow.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        isRunning = false;
        isWalking = false;
        isAir = false;
        superSpeed = false;

        if (Input.GetKeyDown(KeyCode.F) && !speedForceCheck)
        {
            speedForceCheck = true;
            yellow.EnableKeyword("_EMISSION");
            jumpHeight = 3;
            speedForce.SetActive(true);
            lightning.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.F) && speedForceCheck)
        {
            speedForceCheck = false;
            yellow.DisableKeyword("_EMISSION");
            jumpHeight = 2;
            speedForce.SetActive(false);
            lightning.SetActive(false);
        }

        if (!isGrounded)
            isAir = true;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical= Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if(!speedForceCheck) //super speed is turned off
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isRunning = true;
                    controller.Move(moveDir.normalized * runSpd * Time.deltaTime);
                }
                else
                {
                    isWalking = true;
                    controller.Move(moveDir.normalized * walkSpd * Time.deltaTime);
                }
            }
            else  //super speed is turned on
            {
                superSpeed = true;
                controller.Move(moveDir.normalized * superRunSpd * Time.deltaTime);
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}