using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skillManager.sword.DotsActive(true);
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if ((player.transform.position.x > mousePos.x && player.facingDir == 1) ||
            (player.transform.position.x < mousePos.x && player.facingDir == -1))
        {
            player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.skillManager.sword.DotsActive(false);
        AudioManager.instance.PlaySFX(54, 0, null);
        player.StartCoroutine("BusyFor", .2f);
    }
}