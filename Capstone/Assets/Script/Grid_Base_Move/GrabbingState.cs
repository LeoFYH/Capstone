using UnityEngine;

public class GrabbingState : StateBase
{

    private Rigidbody2D rb;
    private PlayerScript player;


    public GrabbingState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Grabbing";


    public override void Enter()
    {
        Debug.Log("����Grabbing״̬");
        //can be damage
    }

    public override void Update()
    {
        // WallRide״̬�µ��߼�
        // add combo point
    }

    public override void Exit()
    {
        Debug.Log("�˳�Grabbing״̬");
        //exit can be damage
    }
}
