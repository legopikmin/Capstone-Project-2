using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff Effect", menuName = "Buff Effect")]
public class BuffEffect : Effect
{
    public Buff buffToApply;

    public override void ActivateEffect(PlayerManager user, CardObject source)
    {
        user.ApplyBuff(buffToApply);
    }
}
