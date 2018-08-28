using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour {

    public Transform m_animalPrefab;
    public int m_limit = 10;
    int m_currentAmountAlive = 0;

    public float m_spawnRadius = 30;
    [MinMaxRange(1, 20)]
    public RangedFloat m_spawnTime;

    bool m_needToSpawn;
    float m_nextSpawnTime;

    void MassSpawn()
    {
        while (m_currentAmountAlive < m_limit)
        {
            SpawnOneAnimal();       
        }
    }

    void SpawnOneAnimal()
    {
        Vector3 position = GetSpawnPosition();
        Quaternion rotation = Quaternion.Euler(new Vector3(0, Random.Range(0.0f, 360.0f), 0));
        Transform newBoy = Instantiate(m_animalPrefab, position, rotation);
        SpawnerNotifier notifier = newBoy.GetComponent<SpawnerNotifier>();
        if (notifier)
            notifier.m_spawner = this;
        m_currentAmountAlive++;
    }

    Vector3 GetSpawnPosition()
    {
        Vector3 position = transform.position;
        position += Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0) * Vector3.forward * Random.Range(0.0f, m_spawnRadius);

        // Get y height
        RaycastHit hit;

        if (Physics.Raycast(position, Vector3.down, out hit, m_spawnRadius)) // Spawn slightly above ground
            position = hit.point + Vector3.up * 0.5f;

        return position;
    }

    private void Update()
    {
        if (m_currentAmountAlive < m_limit)
        {
            if (Time.time >= m_nextSpawnTime)
            {
                SpawnOneAnimal();
                m_nextSpawnTime = Time.time + Random.Range(m_spawnTime.minValue, m_spawnTime.maxValue);
            }
        }
    }

    public void OnAnimalDeath()
    {
        m_currentAmountAlive--;
        m_nextSpawnTime = Time.time + Random.Range(m_spawnTime.minValue, m_spawnTime.maxValue);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;   
        Gizmos.DrawWireSphere(transform.position, m_spawnRadius);
    }
}
