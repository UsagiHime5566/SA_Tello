using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TelloController.TelloBTN telloBTN;
    public void OnPointerDown(PointerEventData eventData)
    {
        CommandTello(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CommandTello(false);
    }

    void CommandTello(bool val){
        switch (telloBTN)
        {
            case TelloController.TelloBTN.Up:
                TelloController.Instance.btn_Up = val;
                break;
            case TelloController.TelloBTN.Down:
                TelloController.Instance.btn_Down = val;
                break;
            case TelloController.TelloBTN.Right:
                TelloController.Instance.btn_Right = val;
                break;
            case TelloController.TelloBTN.Left:
                TelloController.Instance.btn_Left = val;
                break;
            case TelloController.TelloBTN.W:
                TelloController.Instance.btn_W = val;
                break;
            case TelloController.TelloBTN.S:
                TelloController.Instance.btn_S = val;
                break;
            case TelloController.TelloBTN.D:
                TelloController.Instance.btn_D = val;
                break;
            case TelloController.TelloBTN.A:
                TelloController.Instance.btn_A = val;
                break;
        }
    }
}
