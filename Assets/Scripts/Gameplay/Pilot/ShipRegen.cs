using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRegen : MonoBehaviour
{
    public ShipProperties shipProperties;
    public bool regenEnabled = true;
    public int hpPerTick = 1;
    public float timePerTick = 2f;
    private Coroutine regenCoroutine = null;


    // Start is called before the first frame update
    void Start()
    {
        regenCoroutine = StartCoroutine(RegenCoroutine());
    }

    void Update()
    {
        if (regenEnabled && regenCoroutine == null)
        {
            regenCoroutine = StartCoroutine(RegenCoroutine());
        }
    }

    IEnumerator RegenCoroutine()
    {
        while (regenEnabled)
        {
            shipProperties.AddHp(hpPerTick);
            yield return new WaitForSeconds(timePerTick);
        }

    }
}
