using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InventorySO/inv_player")]
public class SaveInventory_Player : ScriptableObject
{
    public List<RabboidModBase> m_inventory;
    public uint m_capacity;
    public uint m_money;
}
