using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private int bounceAmount;

    [Header("Bounce info")] private float bounceSpeed;

    private bool canRotate = true;
    private CircleCollider2D cd;
    private List<Transform> enemyTargets;

    private float freezeTimeDur;
    private float hitCooldown;
    private float hitTimer;
    private bool isBouncing;
    private bool isReturning;
    private bool isSpining;

    [Header("Spin info")] private float maxTravelDist;

    [Header("Pierce info")] private int pierceAmount;

    private Player player;
    private Rigidbody2D rb;
    private float returnSpeed;
    private float spinDir;
    private float spinDuration;
    private float spinTimer;
    private int targetIndex;
    private bool wasStopped;
    private int enemyDetectRadius = 10;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }


    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.linearVelocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                player.transform.position, Time.deltaTime * returnSpeed);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }

        if (collision.GetComponent<Enemy>() != null)
        {
            var enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(collision);
        StuckInto(collision);
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void SpinLogic()
    {
        if (isSpining)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDist && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                float speed = 1.5f;

                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + spinDir, transform.position.y),
                    Time.deltaTime * speed);

                if (spinTimer <= 0)
                {
                    isSpining = false;
                    isReturning = true;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer <= 0)
                {
                    hitTimer = hitCooldown;

                    var colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                enemyTargets[targetIndex].position, Time.deltaTime * bounceSpeed);

            float dinstanceToHit = .1f;
            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < dinstanceToHit)
            {
                SwordSkillDamage(enemyTargets[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTargets.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDur);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, enemyDetectRadius);

                foreach (var hit in colliders)
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTargets.Add(hit.transform);
                    }
            }
        }
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDur, float _returnSpeed)
    {
        freezeTimeDur = _freezeTimeDur;
        returnSpeed = _returnSpeed;

        player = _player;

        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
        {
            anim.SetBool("Rotation", true);
        }

        spinDir = Mathf.Clamp(rb.linearVelocity.x, -1, 1);

        Invoke("DestroyMe", 7);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        isReturning = true;
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpining)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargets.Count > 0)
        {
            return;
        }

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bounceSpeed = _bounceSpeed;
        enemyTargets = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpining, float _maxTravelDist, float _spinDuration, float _hitCooldown)
    {
        isSpining = _isSpining;
        maxTravelDist = _maxTravelDist;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
}