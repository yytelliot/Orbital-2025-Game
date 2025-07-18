using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable
{
    // Stun for as long as the velocity is non-zero
    void StunUntilStop();

    // Stun for a fixed duration
    void Stun(float seconds);
}