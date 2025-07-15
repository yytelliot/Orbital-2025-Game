using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

public interface ITakeDamage
{
    // Stun for as long as the velocity is non-zero
    void TakeDamage(int damage);

    void HandleProjectileHit(Component sender, object payload);


}