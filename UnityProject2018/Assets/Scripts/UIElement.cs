using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    public ScriptableObject DataReference;

    public abstract void UpdateUI();

    public abstract void UpdateUI(bool hasPartner);
}
