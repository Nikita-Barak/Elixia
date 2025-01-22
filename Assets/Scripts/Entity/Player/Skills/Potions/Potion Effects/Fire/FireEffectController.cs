using System.Collections.Generic;
using UnityEngine;

public class FireEffectController : MonoBehaviour
{
    private float damageCooldown;

    private CapsuleCollider2D cd;
    private Dictionary<Enemy, float> damageCooldowns = new Dictionary<Enemy, float>();
    private bool toDamage = false;

    public void SetupFlame(float _damageCooldown)
    {
        damageCooldown = _damageCooldown;
        cd = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        // All enemies within the attack range
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(transform.position, cd.size, cd.direction, transform.eulerAngles.z);

        foreach(var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (CanDamage(enemy))
                {
                    PlayerManager.instance.player.cs.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                    damageCooldowns[enemy] = Time.time + damageCooldown;
                }
            }
        }
    }

    private bool CanDamage(Enemy enemy)
    {
        if (!toDamage)
        {
            return false;
        }
        
        // Add the enemy to the dictionary if not already present
        if (!damageCooldowns.ContainsKey(enemy))
        {
            damageCooldowns[enemy] = 0f; // Initialize with a time in the past to allow immediate damage
        }

        // Check if the enemy's cooldown has expired
        return Time.time >= damageCooldowns[enemy];
    }

    // Animation Triggers:
    public void startDamage()
    {
        toDamage = true;
    }

    public void stopDamage()
    {
        toDamage = false;
    }

    public void destroyFlame()
    {
        Destroy(gameObject);
    }
}