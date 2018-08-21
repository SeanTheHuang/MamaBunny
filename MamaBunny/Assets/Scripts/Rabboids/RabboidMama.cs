using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabboidMama : MonoBehaviour {

    public Transform m_baseRabboidPrefab;

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
    }

    // TEMP CODE TO TEST SPAWNING
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            SpawnRabboid();
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

    public void SpawnRabboid()
    {
        RabboidResult results = RabboidCalculator.Instance.CalculateRabboid(m_colourModList, m_mouthModList, m_backModList, m_sizeModList);
        Transform rabboid = Instantiate(m_baseRabboidPrefab, Vector3.up * 4, Quaternion.identity);
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