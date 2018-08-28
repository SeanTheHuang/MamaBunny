using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModelType
{
    BOY,
    GIRL,
    WOMEN,
    MAN
};

public class CustomerCalculator : MonoBehaviour {

    public static CustomerCalculator Instance
    { get; private set; }

    public GameObject m_boyModel;
    public GameObject m_girlModel;
    public GameObject m_womanModel;
    public GameObject m_manModel;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public GameObject CalculateCustomerModel(ModelType type)
    {
        if(type == ModelType.BOY)
        {
            return m_boyModel;
        }
        else if (type == ModelType.GIRL)
        {
            return m_girlModel;
        }
        else if (type == ModelType.WOMEN)
        {
            return m_womanModel;
        }

        return m_manModel;
    }
}
