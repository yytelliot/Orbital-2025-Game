using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRegen : MonoBehaviour
{
    public ShipProperties shipProperties;
    public bool regenEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RegenCoroutine(1, 2f));
    }

    IEnumerator RegenCoroutine(int hpPerTick, float timePerTick)
    {
        while (regenEnabled)
        {
            shipProperties.AddHp(hpPerTick);
            yield return new WaitForSeconds(timePerTick);
        }

    }
}
