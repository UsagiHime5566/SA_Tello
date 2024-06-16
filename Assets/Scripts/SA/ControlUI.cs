using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{
    public TelloController telloController;
    public InputField Rotate;
    public InputField UpDown;
    public InputField Move;
    void Start()
    {
        Rotate.onValueChanged.AddListener(x => {
            float.TryParse(x, out float f);
            telloController.AD_Value = f;
        });
        UpDown.onValueChanged.AddListener(x => {
            float.TryParse(x, out float f);
            telloController.WS_Value = f;
        });
        Move.onValueChanged.AddListener(x => {
            float.TryParse(x, out float f);
            telloController.Move_Value = f;
        });
    }
}
