using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CageShopLogic : MonoBehaviour {

    [MinMaxRange(0.0f, 30.0f)]
    public RangedFloat m_timeToSell;
    [MinMaxRange(0.0f, 30.0f)]
    public RangedFloat m_sellAmount;
    public BunnyPenData[] m_pens;
    public SaveInventory_Player m_inventory;
    public TextMeshProUGUI m_moneyText;
    List<BunnyPenData> m_filledList;

    float m_nextSellTime;
    bool m_pensStillLeft;

    private void Awake()
    {
        m_pensStillLeft = false;
        m_filledList = new List<BunnyPenData>();
        foreach (BunnyPenData bpd in m_pens)
        {
            if (bpd.m_bunnyInside)
            {
                m_filledList.Add(bpd);
                m_pensStillLeft = true;
            }
        }

        if (m_pensStillLeft)
            m_nextSellTime = Time.time + Random.Range(m_timeToSell.minValue, m_timeToSell.maxValue);
    }

    private void Start()
    {
        m_moneyText.text = "Coins: " + m_inventory.m_money;
    }

    private void Update()
    {
        if (!m_pensStillLeft)
            return;

        if (Time.time >= m_nextSellTime)
            SellABunny();
    }

    void SellABunny()
    {
        // Sell last pen
        if (m_filledList.Count < 2)
        {
            m_pensStillLeft = false;
            SellPen(m_filledList[0]);
        }

        int index = Random.Range(0, m_filledList.Count);
        m_nextSellTime = Time.time + Random.Range(m_timeToSell.minValue, m_timeToSell.maxValue);
        BunnyPenData pen = m_filledList[index];
        m_filledList.Remove(pen);
        SellPen(pen);
    }

    void SellPen(BunnyPenData pen)
    {
        pen.ResetVariables();
        SoundEffectsPlayer.Instance.PlaySound("kaching");
        uint sellAmount = (uint)Mathf.RoundToInt(Random.Range(m_sellAmount.minValue, m_sellAmount.maxValue));
        m_inventory.m_money += sellAmount;
        m_moneyText.text = "Coins: " + m_inventory.m_money;
    }
}
