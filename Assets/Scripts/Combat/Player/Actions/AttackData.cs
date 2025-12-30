using UnityEngine;

public struct AttackData
{
    public readonly float damage;
    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly CombatStateID stateID;

    public AttackData(
        float damage,
        Vector3 position,
        Quaternion rotation,
        CombatStateID stateID)
    {
        this.damage = damage;
        this.position = position;
        this.rotation = rotation;
        this.stateID = stateID;
    }
    
    public enum CombatStateID
    {
        GroundAttack1,
        GroundAttack2,
        GroundAttack3,
        Idle,
        Dodge
    }
}
