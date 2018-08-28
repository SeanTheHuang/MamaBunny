using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerNotifier : MonoBehaviour {

    public AnimalSpawner m_spawner;

    private void OnDestroy()
    {
        if (m_spawner)
            m_spawner.OnAnimalDeath();
    }
}
