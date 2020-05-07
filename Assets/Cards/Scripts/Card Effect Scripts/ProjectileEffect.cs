using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Connery Ray 05/07/2020
/// Added ability to specify projectile rotation
/// </summary>

[CreateAssetMenu(fileName = "New Projectile Effect", menuName = "Projectile Effect")]
public class ProjectileEffect : Effect
{
    public GameObject projectile;
    public GameObject particleEffect;
    public float projectileSpeed;
    public float baseDamage;
    public float lifeSpan;

    public override void ActivateEffect(PlayerManager user, CardObject source)
    {
        user.weaponManager.FireProjectile(projectile, baseDamage, lifeSpan, projectileSpeed);
    }
}