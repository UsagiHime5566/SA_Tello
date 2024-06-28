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
    public float everyAdjust = 0.2f;
    void Start()
    {
        Rotate.onValueChanged.AddListener(x => {
            float.TryParse(x, out float f);
            telloController.AD_Value = f;
            SystemConfig.Instance.SaveData("rot", f);
        });
        UpDown.onValueChanged.AddListener(x => {
            float.TryParse(x, out float f);
            telloController.WS_Value = f;
            SystemConfig.Instance.SaveData("updown", f);
        });
        Move.onValueChanged.AddListener(x => {
            float.TryParse(x, out float f);
            telloController.Move_Value = f;
            SystemConfig.Instance.SaveData("move", f);
        });

        Rotate.text = SystemConfig.Instance.GetData<float>("rot", 1).ToString("0.0");
        UpDown.text = SystemConfig.Instance.GetData<float>("updown", 1).ToString("0.0");
        Move.text = SystemConfig.Instance.GetData<float>("move", 1).ToString("0.0");

    }

    public void DownValues(){
        Rotate.text = Mathf.Max(0, telloController.AD_Value-everyAdjust).ToString("0.0");
        UpDown.text = Mathf.Max(0, telloController.WS_Value-everyAdjust).ToString("0.0");
        Move.text = Mathf.Max(0, telloController.Move_Value-everyAdjust).ToString("0.0");
    }

    public void UpValues(){
        Rotate.text = Mathf.Min(3, telloController.AD_Value+everyAdjust).ToString("0.0");
        UpDown.text = Mathf.Min(3, telloController.WS_Value+everyAdjust).ToString("0.0");
        Move.text = Mathf.Min(3, telloController.Move_Value+everyAdjust).ToString("0.0");
    }
}
