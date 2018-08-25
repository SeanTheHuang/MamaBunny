using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabboidMama : MonoBehaviour {

    public Transform m_baseRabboidPrefab;
    public Transform m_rabboidEggPrefab;
    public EggSpawnPath m_spawnEggPath;

    [Header("Move variables")]
    public float m_floatDist = 0.3f;
    public float m_floatFreq = 0.3f;
    public Vector3 m_rotateAxis = Vector3.one;
    public float m_rotateSpeed = 45;

    Vector3 m_startPosition;

    List<RabboidColour> m_colourModList;
    List<RabboidBodyPart> m_backModList, m_mouthModList;
    List<RabboidSizeMod> m_sizeModList;

    private void Awake()
    {
        // Create all lists
        m_colourModList = new List<RabboidColour>();
        m_backModList = new List<RabboidBodyPart>();
        m_mouthModList = new List<RabboidBodyPart>();
        m_sizeModList = new List<RabboidSizeMod>();
        m_startPosition = transform.position;
    }

    // TEMP CODE TO TEST SPAWNING
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            SpawnRabboidEgg();

        MoveLogic();
        RotateLogic();
    }

    void MoveLogic()
    {
        transform.position = m_startPosition + Vector3.up * Mathf.Sin(Time.time * m_floatFreq) * m_floatDist;
    }

    void RotateLogic()
    {
        transform.Rotate(m_rotateAxis * Time.deltaTime * m_rotateSpeed);
    }

    #region ADD_FUCTIONS

    public void AddColour(RabboidColour _new)
    {
        m_colourModList.Add(_new);
    }

    public void AddBackMod(RabboidBodyPart _new)
    {
        m_backModList.Add(_new);
    }

    public void AddMouthMod(RabboidBodyPart _new)
    {
        m_mouthModList.Add(_new);
    }

    public void AddSizeMod(RabboidSizeMod _new)
    {
        m_sizeModList.Add(_new);
    }

    #endregion

    public void SpawnRabboidEgg()
    {
        RabboidEgg egg = Instantiate(m_rabboidEggPrefab, Vector3.zero, Quaternion.identity).GetComponent<RabboidEgg>();
        egg.StartAnimation(m_spawnEggPath.m_movePath, this);
    }

    public void SpawnRabboid(Vector3 _position)
    {
        Vector3 eulers = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        RabboidResult results = RabboidCalculator.Instance.CalculateRabboid(m_colourModList, m_mouthModList, m_backModList, m_sizeModList);
        Transform rabboid = Instantiate(m_baseRabboidPrefab, _position, Quaternion.Euler(eulers));
        rabboid.GetComponent<Rabboid>().Initialize(results);

        ClearAllLists();
    }

    void ClearAllLists()
    {
        m_colourModList.Clear();
        m_backModList.Clear();
        m_mouthModList.Clear();
        m_sizeModList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ASSUMING: Only hit item pick ups
        PickUp pickUp = other.GetComponent<PickUp>();
        pickUp.OnEatenByMamaRabbit(this);
    }
}