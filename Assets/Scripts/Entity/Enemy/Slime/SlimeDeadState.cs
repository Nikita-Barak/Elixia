public class SlimeDeadState : EnemyState
{
    private readonly Slime enemy;

    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Slime _enemy) :
        base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;

        enemy.cd.enabled = false;
        stateTimer = enemy.knockUpTime;

        enemy.DestroySelfInDelay();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.linearVelocity = enemy.knockUpVelocity;
        }
    }
}