using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    [System.Serializable]
    public class ProjectileHitPayload
    {
        public GameObject target;
        public int damage;
    }
}