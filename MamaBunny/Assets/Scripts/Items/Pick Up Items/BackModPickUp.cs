using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackModPickUp : PickUp
{
    [SerializeField]
    public RabboidBodyPart m_bodyPart;

    public override void OnEatenByMamaRabbit(RabboidMama _mama)
    {
        _mama.AddBackMod(m_bodyPart);
        Destroy(gameObject);
    }
}