using UnityEngine;

public struct AttackData
{
    public readonly float damage;
    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly CombatState state;

    public AttackData(
        int comboStep,
        float damage,
        Vector3 position,
        Quaternion rotation,
        CombatState state)
    {
        this.damage = damage;
        this.position = position;
        this.rotation = rotation;
        this.state = state;
    }
}
