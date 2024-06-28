using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoSlot : MonoBehaviour
{
    public Text text;
    
    public void SetSlot(string n)
    {
        text.text = n;
    }
}
