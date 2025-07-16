using UnityEngine;
using System.Collections;
public class PlayerScript : MonoBehaviour
{
    [Header("状态机")]
    public E stateMachine;
    private Rigidbody2D rb;

    [Header("跳跃设置")]
    public float maxJumpForce = 14f;
    public float minJumpForce = 2f;
    public float doubleJumpForce = 8f;
    public float maxChargeTime = 2f;

    [Header("移动设置")]
    public float moveSpeed = 5f;
    //public float airMoveSpeed = 2f;
    public float airControlForce = 5f;     // 空中加速力
    public float maxAirHorizontalSpeed = 10f;
    [Header("轨道设置")]
    public Track currentTrack; // 当前接触的轨道
    public float grindJumpIgnoreTime = 0.2f; // 轨道跳后短时间内禁止重新吸附
    [HideInInspector]
    public float grindJumpTimer = 0f;

    [Header("Button Check")]
    public bool isEHeld = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new E();
        stateMachine.AddState("Idle", new IdleState());
        stateMachine.AddState("Jump", new JumpState(this, rb));
        stateMachine.AddState("Move", new MoveState(this, rb));
        stateMachine.AddState("DoubleJump", new DoubleJumpState(this, rb));
        stateMachine.AddState("Grind", new GrindState(this, rb));
        stateMachine.AddState("GJump", new GrindState(this, rb));
        stateMachine.SwitchState("Idle");
    }

    void Update()
    {
        stateMachine.UpdateCurrentState();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            stateMachine.SwitchState("Jump");
            return;
        }

        /////////////////////////////////E//////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.E) && !IsGrounded())
        {
            isEHeld = true;

            Track nearbyTrack = DetectNearbyPole();
            if (nearbyTrack != null || grindJumpTimer <= 0f)
            {
                currentTrack = nearbyTrack;  
                stateMachine.SwitchState("Grind"); // 卡杆状态
                Debug.Log("AAAA");
            }
            else
            {
                stateMachine.SwitchState("GrabBoard"); // 抓板状态
                Debug.Log("BBB");
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isEHeld = false;
        }
        //////////////////////////////////////////////////////////////
        if (grindJumpTimer > 0f)
        {
            grindJumpTimer -= Time.deltaTime;
        }



        ////////////////////////////Space///////////////////////////////
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            stateMachine.SwitchState("Jump");
            return;
        }
        ////////////////////////////////////////////////////////////
        string currentState = stateMachine.GetCurrentStateName();
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Idle时有输入才切Move
        if (currentState == "Idle" && Mathf.Abs(moveInput) > 0.01f && IsGrounded())
        {
            stateMachine.SwitchState("Move");
        }
        // Move时没输入才切Idle
        else if (currentState == "Move" && Mathf.Abs(moveInput) <= 0.01f && IsGrounded())
        {
            stateMachine.SwitchState("Idle");
        }
    }

    // 判断是否在地面上
    public bool IsGrounded()
    {
        // 这里简单示例，实际项目建议用射线检测
        return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
    }



    //////////////////////////////////Track logic//////////////////////////////
    public Track DetectNearbyPole(float radius = 1.5f)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.isTrigger)
            {
                Track track = hit.GetComponent<Track>();
                if (track != null) 
                {
                    return track;
                }
            }
        }

        return null;
    }

    public IEnumerator SwitchToStateDelayed(string stateName)
    {
        yield return null; 
        stateMachine.SwitchState(stateName);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
    ///////////////////////////////////////////////////////////////////////////////



}