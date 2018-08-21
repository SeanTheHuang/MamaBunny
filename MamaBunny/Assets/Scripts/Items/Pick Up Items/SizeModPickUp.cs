using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeModPickUp : PickUp {

    [SerializeField]
    RabboidSizeMod m_sizeMod;

    public override void OnEatenByMamaRabbit(RabboidMama _mama)
    {
        _mama.AddSizeMod(m_sizeMod);
        Destroy(gameObject);
    }
}
