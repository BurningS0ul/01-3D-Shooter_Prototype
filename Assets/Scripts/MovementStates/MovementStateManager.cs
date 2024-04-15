using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovementStateManager : MonoBehaviour
{
    #region Movement
    public float currentMoveSpeed;
    public float walkSpeed = 3,
        walkBackSpeed = 2;
    public float runSpeed = 7,
        runBackSpeed = 5;
    public float crouchSpeed = 2,
        crouchBackSpeed = 1;
    public float airSpeed = 1.5f;

    [HideInInspector]
    public Vector3 dir;
    public float hInput,
        vInput;
    CharacterController cc;
    #endregion

    #region GroundCheck
    [SerializeField]
    LayerMask groundMask;
    #endregion

    #region Gravity
    readonly float gravity = -9.81f;

    [SerializeField]
    float jumpForce = 6;
    public bool jumped;
    Vector3 spherePos;
    public Vector3 velocity;
    #endregion

    #region States
    public MovementBaseState previousState;
    public MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();
    public CrouchState Crouch = new CrouchState();
    public JumpState Jump = new JumpState();
    #endregion

    public Text EnemiesLeft;

    [HideInInspector]
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    // Update is called once per frame
    void Update()
    {
        GetDirectiondAndMove();
        Gravity();
        Falling();

        EnemiesLeft.text = "" + GameObject.FindGameObjectsWithTag("Enemy").Length + " Enemies left";

        anim.SetFloat("hInput", hInput);
        anim.SetFloat("vInput", vInput);

        currentState.UpdateState(this);

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void GetDirectiondAndMove()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        Vector3 airDir = Vector3.zero;

        if (!IsGrounded())
        {
            airDir = transform.forward * vInput + transform.right * hInput;
        }
        else
        {
            dir = transform.forward * vInput + transform.right * hInput;
        }

        cc.Move(
            (dir.normalized * currentMoveSpeed + airDir.normalized * airSpeed) * Time.deltaTime
        );
    }

    public bool IsGrounded()
    {
        spherePos = new Vector3(
            transform.position.x,
            transform.position.y - 0.08f,
            transform.position.z
        );
        return Physics.CheckSphere(spherePos, cc.radius - 0.05f, groundMask);
    }

    void Gravity()
    {
        if (!IsGrounded())
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2;
        }
        cc.Move(velocity * Time.deltaTime);
    }

    void Falling()
    {
        anim.SetBool("Falling", !IsGrounded());
    }

    public void JumpForce()
    {
        velocity.y += jumpForce;
    }

    public void Jumped()
    {
        jumped = true;
    }
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(spherePos, cc.radius - 0.05f);
    // }
}
