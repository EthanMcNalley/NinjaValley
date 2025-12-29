using System;
using System.Dynamic;
using UnityEngine;

public static class CombatEvents
{
    public static event Action ShadowAssassinStarted;
    public static event Action ShadowAssassinEnded;
    
    public static void RaiseShadowAssassinStarted() => ShadowAssassinStarted?.Invoke();
    public static void RaiseShadowAssassinEnded() => ShadowAssassinEnded?.Invoke();
    
}
