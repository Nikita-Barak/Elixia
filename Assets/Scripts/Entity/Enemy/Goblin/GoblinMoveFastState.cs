using UnityEngine;

public class GoblinMoveFastState : EnemyState
{
    private Goblin enemy;
    private Transform player;
    private int moveDir;
    private bool movingFast = false;
    private bool bombTriggered;

    public GoblinMoveFastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Goblin _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
        enemy.moveSpeed = enemy.fastMoveSpeed;
        stateTimer = enemy.delayBeforeMoveFast;
    }

    public override void Update()
    {
        base.Update();

        if (!movingFast)
        {
            if (stateTimer == 0)
            {
                movingFast = true;
                enemy.anim.SetBool(animBoolName, false);
                enemy.anim.SetBool("MoveFast", true);
            }
        }
        else
        {
            if (!bombTriggered)
            {
                // Determine the direcion in which the player is at, and move towards him.
                if (player.position.x > enemy.transform.position.x)
                {
                    moveDir = 1;
                }
                else if (player.position.x < enemy.transform.position.x)
                {
                    moveDir = -1;
                }

                enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocityY);

                if (enemy.IsWallDetected() || !enemy.IsGroundDetected() || Vector2.Distance(player.position, enemy.transform.position) < enemy.bombTriggerDistance)
                {
                    enemy.anim.SetTrigger("Bomb");
                    bombTriggered = true;
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
