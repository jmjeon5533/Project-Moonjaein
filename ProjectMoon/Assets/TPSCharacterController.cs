using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCharacterController : MonoBehaviour
{
    [SerializeField]
    Transform characterBody;
    [SerializeField]
    Transform cameraArm;
    [SerializeField]
    bool isjump = false;

    public float Rayline;

    Rigidbody rigid;

    public float MoveSpeed = 5;
    public float JumpPower = 50;

    Animator anim;
    void Start()
    {
        anim = characterBody.GetComponent<Animator>();
        rigid = characterBody.GetComponent<Rigidbody>();

    }
    
    void Update()
    {
        LookAround();
        Move();
        Jump();
        Raycast();
    }
    void LookAround()
    {
        Vector2 mouseDelta
            = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180)
        {
            x = Mathf.Clamp(x, -1f, 70f);

        }
        else if(x > 25)
        {
            x = Mathf.Clamp(x, 335, 361);

        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

    }
    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        //anim.SetBool("isMove", isMove);
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = moveDir;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += moveDir * Time.deltaTime * MoveSpeed * 2;

            }
            else
            {
                transform.position += moveDir * Time.deltaTime * MoveSpeed;
            }
        }
        Debug.DrawRay(cameraArm.position,
            new Vector3(cameraArm.forward.x,0f,cameraArm.forward.z).normalized, Color.red);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isjump)
        {
            rigid.AddForce(new Vector3(0, JumpPower, 0));
            isjump = true;
        }
    }
    void Raycast()
    {
        Debug.DrawRay(characterBody.position, Vector3.down * Rayline, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(characterBody.position, Vector3.down, Rayline, LayerMask.GetMask("Ground")))
        {
            isjump = false;
        }
    }
}
