using UnityEngine;

public class ArcaneEffectController : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    [SerializeField] LayerMask whatIsEnemy;

    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private Transform closestTarget;
    private float moveSpeed;
    private bool canGrow;
    private float growSpeed;
    private float growScale;


    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, float _growSpeed, float _growScale)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        growSpeed = _growSpeed;
        growScale = _growScale;
        closestTarget = FindClosestEnemy(gameObject.transform);
    }

    private void Update()
    {
        if (crystalExistTimer > 0)
        {
            crystalExistTimer = Mathf.Max(crystalExistTimer - Time.deltaTime, 0);
        }
        
        if (crystalExistTimer == 0)
        {
            EndCrystalCycle();
        }

        // If the crystal explosion can grow, we increase the scale in a given speed to a given scale.
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(growScale, growScale), growSpeed * Time.deltaTime);
        }

        // If we allow the crystal to move, it'll move towards the closest enemy target if it exists.
        if (canMove && closestTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            // If the crystal is close enough to the target - it ends it's cycle (either explodes or dissappears)
            // * 1 is not a magic number, it is exactly right for how close the crystal should be to the target before exploding.
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                EndCrystalCycle();
            }
        }
    }

    private void AnimationExplodeEvent()
    {
        // All enemies within the attack range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                PlayerManager.instance.player.cs.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemEffect(hit.transform);
            }
        }
    }

    // If we have an amulet that should add additional effects to the ability - we execute the effect.
    protected void ItemEffect(Transform _target)
    {
        ItemData_Equipment equippedAmulet = Inventory.instance.GetEquipmentOfType(EquipmentType.Amulet);
        if (equippedAmulet != null)
        {
            Debug.Log("Amulet found!");
            equippedAmulet.Effect(_target);
        }
    }

    public void EndCrystalCycle()
    {
        if(canExplode)
        {
            canMove = false;
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);

    public Transform FindClosestEnemy(Transform _checkTransform)
    {
        // All enemies within the attack range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, SkillManager.instance.enemyDetectRadius);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        // We go over all enemies in the attack range, we find the one closest and store it in our closestEnemy varible.
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }

    public void ChooseRandomEnemy()
    {
        var radius = SkillManager.instance.blackhole.GetBlackHoleRadius();
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if (colliders.Length > 0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }
}
