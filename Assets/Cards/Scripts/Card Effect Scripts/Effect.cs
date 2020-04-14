using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    /// <summary>
    /// Activate the functionality of this effect
    /// </summary>
    /// <param name="user">The player that used the card this effect is attached to</param>
    /// <param name="source">The card that this effect is attached to</param>
    public abstract void ActivateEffect(PlayerManager user, CardObject source);
}