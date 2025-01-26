using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        // Apply on-take-hit effects when health is decreased.
        var currentArmor = Inventory.instance.GetEquipmentOfType(EquipmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.skillManager.evade.CreateCloneOnEvade();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _attackMultiplier)
    {
        if (_targetStats.isInvulnerable)
        {
            return;
        }

        // We check if the target evades the attack, is so - we return and do nothing.
        if (TargetCanEvadeAttack(_targetStats))
        {
            return;
        }

        // Calculating damage
        var totalDamage = damage.GetValue() + strength.GetValue();

        if (_attackMultiplier > 0.0f)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _attackMultiplier);
        }

        // If we can crit the target - we multiply the damage by the required percentage.
        if (CanCritTarget())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        // We lower the output damage according to the target's armor
        totalDamage = TargetPhysicalDamageReduction(_targetStats, totalDamage);

        // We deal the overall damage to the target.
        _targetStats.TakeDamage(totalDamage);
    }
}