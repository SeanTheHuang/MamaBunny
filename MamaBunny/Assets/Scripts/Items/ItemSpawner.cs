using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    [MinMaxRange(1, 20)]
    public RangedFloat m_spawnTime;
    public Transform m_spawnPrefab;
    public Transform[] m_spawnPoints;

    bool m_needToSpawn;
    int m_currentNumSpawned;
    float m_nextSpawnTime;

    Queue<int> m_spawnIndexQueue;

    private void Awake()
    {
        m_currentNumSpawned = m_spawnPoints.Length;
        m_spawnIndexQueue = new Queue<int>();
    }

    private void Start()
    {
        // Fill spawn points
        for (int i = 0; i < m_spawnPoints.Length; i++)
        {
            SpawnNewItem(i);
        }
    }

    private void Update()
    {
        if (!m_needToSpawn)
            return;

        if (Time.time >= m_nextSpawnTime)
        {
            int index = m_spawnIndexQueue.Dequeue();
            SpawnNewItem(index);
            m_currentNumSpawned += 1;

            if (m_currentNumSpawned >= m_spawnPoints.Length)
                m_needToSpawn = false;
            else
                m_nextSpawnTime = Time.time + Random.Range(m_spawnTime.minValue, m_spawnTime.maxValue);
        }
    }

    void SpawnNewItem(int _index)
    {
        Transform newBoy = Instantiate(m_spawnPrefab, m_spawnPoints[_index].position, m_spawnPoints[_index].rotation);
        newBoy.localEulerAngles = new Vector3(newBoy.localEulerAngles.x, Random.Range(0, 360), newBoy.localEulerAngles.z);
        newBoy.GetComponent<GunTarget>().m_parent = this;
        newBoy.GetComponent<GunTarget>().m_index = _index;
    }

    public void OnItemsDeath(int _index)
    {
        m_spawnIndexQueue.Enqueue(_index);
        m_currentNumSpawned -= 1;

        if (!m_needToSpawn)
        {
            m_needToSpawn = true;
            m_nextSpawnTime = Time.time + Random.Range(m_spawnTime.minValue, m_spawnTime.maxValue);
        }
    }
}
