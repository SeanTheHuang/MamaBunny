using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthModPickUp : PickUp
{
    [SerializeField]
    RabboidBodyPart m_mouthMod;

    public override void OnEatenByMamaRabbit(RabboidMama _mama)
    {
        _mama.AddMouthMod(m_mouthMod);
        Destroy(gameObject);
    }
}
