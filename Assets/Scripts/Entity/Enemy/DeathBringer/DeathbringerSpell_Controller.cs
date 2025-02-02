using UnityEngine;

public class DeathbringerSpell_Controller : MonoBehaviour
{
    [SerializeField]
    private Transform check;

    [SerializeField]
    private Vector2 boxSize;

    [SerializeField]
    private LayerMask whatIsPlayer;
    
    private CharacterStats myStats;
    
    public void SetupSpell(CharacterStats _stats) => myStats = _stats;
    
    
    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }
    
    private void SelfDestroy() => Destroy(gameObject);
}
