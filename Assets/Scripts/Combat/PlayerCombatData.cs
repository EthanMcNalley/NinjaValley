using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatData", menuName = "Scriptable Objects/PlayerCombatData")]
public class PlayerCombatData : ScriptableObject
{
    public string characterName;
    
    public float Health;
    public float SoulPointValue;
    public float StaggerValue;
    public float RecoverTime;
    public float AttackDamage;
    
}
