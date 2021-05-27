using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureToogle : MonoBehaviour
{
    public GameObject Panel;
    public Dropdown m_Dropdown;

    void Start()
    {
        m_Dropdown = GetComponent<Dropdown>();
    }

    void Update()
    {
        if (m_Dropdown.value == 0 && !Panel.activeSelf)
            Panel.SetActive(true);
        else if(m_Dropdown.value != 0 && Panel.activeSelf)
            Panel.SetActive(false);
    }
    public void TooglePanel(Dropdown change)
    {
        if (Panel != null)
            Panel.SetActive(!Panel.activeSelf);
    }
}
