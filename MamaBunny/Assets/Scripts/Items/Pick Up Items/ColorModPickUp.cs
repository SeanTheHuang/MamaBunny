using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorModPickUp : PickUp
{
    [SerializeField]
    public RabboidColour m_colourMod;

    public override void OnEatenByMamaRabbit(RabboidMama _mama)
    {
        _mama.AddColour(m_colourMod);
        Destroy(gameObject);
    }
}