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
    
    [Header("空中设置")]
    public bool isInAir = false; // 是否在空中
    public float airTime = 0f; // 空中时间
    public int airCombo = 0; // 空中连击数
    public float grindJumpIgnoreTime = 0.2f; // 轨道跳后短时间内禁止重新吸附
    public bool isNearTrack = false;
    [HideInInspector]
    public float grindJumpTimer = 0f;
    [Header("Wall Setting")]
    public Wall currentWall;
    public bool isNearWall = false;
    [Header("Button Check")]
    public bool isEHeld = false;
    public bool isWHeld = false;
    [Header("Life Setting")]
    public bool canBeHurt;
    [Header("Combat Setting")]
    public bool isPowerGrinding;
    private bool isCheckingReverseWindow = false;
    public float reverseInputWindow = 0.2f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new E();
        stateMachine.AddState("Idle", new IdleState());
        stateMachine.AddState("Jump", new JumpState(this, rb));
        stateMachine.AddState("Move", new MoveState(this, rb));
        stateMachine.AddState("Grind", new GrindState(this, rb));
        stateMachine.AddState("Air", new AirState(this, rb));
        stateMachine.AddState("Trick", new TrickState(this, rb));
        stateMachine.AddState("GJump", new GJumpState(this, rb));
        stateMachine.AddState("Grab", new GrabbingState(this, rb));
        stateMachine.AddState("WallRide", new WallRideState(this, rb));
        stateMachine.AddState("Reverse", new ReverseState(this, rb));
        stateMachine.AddState("PowerGrind", new PowerGrindState(this, rb));
        
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

            if (isNearTrack && grindJumpTimer <= 0f)
            {
                Debug.Log("AAAA");
                stateMachine.SwitchState("Grind");

            }
            else if (isNearWall)
            {
                Debug.Log("CCCC");
                stateMachine.SwitchState("WallRide");
            }
            else
            {
                Debug.Log("BBB");
                stateMachine.SwitchState("Grab");

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



        //////////////////////W///////////////////////////////////////
        string currentState = stateMachine.GetCurrentStateName();
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            isWHeld = true;

            if (!isCheckingReverseWindow)
            {
                StartCoroutine(CheckReverseWindow());
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isWHeld = false;
        }
        //////////////////////////////////////////////////////////////


        ////////////////////////////Space///////////////////////////////
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            stateMachine.SwitchState("Jump");
            return;
        }
        ////////////////////////////////////////////////////////////


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
    //public Track DetectNearbyPole(float radius = 1.5f, float maxDistance = 0.8f)
    //{
    //    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

    //    Track closestTrack = null;
    //    float closestDistance = Mathf.Infinity;

    //    foreach (var hit in hits)
    //    {
    //        if (hit.isTrigger)
    //        {
    //            Track track = hit.GetComponent<Track>();
    //            if (track != null)
    //            {
    //                float dist = Vector2.Distance(transform.position, track.transform.position);
    //                if (dist < closestDistance)
    //                {
    //                    closestDistance = dist;
    //                    closestTrack = track;
    //                }
    //            }
    //        }
    //    }

    //    if (closestTrack != null && closestDistance <= maxDistance)
    //    {
    //        return closestTrack;
    //    }

    //    return null; // 超过距离，视为没有轨道
    //}

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
    //////////////////////////////////Trigger/////////////////////////////////////////////
    /// <summary>
    /// 所有的trigger都在这里
    /// </summary>
    /// <param name="other"></param>

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger)
        {
            Track track = other.GetComponent<Track>();
            if (track != null)
            {
                currentTrack = track;
                isNearTrack = true;
            }

            Wall wall = other.GetComponent<Wall>();
            if (wall != null)
            { 
                currentWall = wall;
                isNearWall = true;
            }


        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger)
        {
            Track track = other.GetComponent<Track>();
            if (track != null && track == currentTrack)
            {
                isNearTrack = false;
                currentTrack = null;
            }

            Wall wall = other.GetComponent<Wall>();
            if (wall != null)
            {
                currentWall = null;
                isNearWall = false;
            }
        }


        //////////////////////////////////////////////////////////////////////////////
        

    }

    private IEnumerator CheckReverseWindow()
    {
        isCheckingReverseWindow = true;

        float timer = 0f;
        bool reverseTriggered = false;
        float originalDirection = Mathf.Sign(rb.linearVelocity.x);

        while (timer < reverseInputWindow)
        {
            float input = Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(input) > 0.01f && Mathf.Sign(input) != originalDirection)
            {
                Debug.Log("Reverse");
                //stateMachine.SwitchState("Reverse");
                reverseTriggered = true;
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!reverseTriggered)
        {
            Debug.Log("PG");
            stateMachine.SwitchState("PowerGrind");
        }

        isCheckingReverseWindow = false;
    }

}