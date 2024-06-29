using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TelloLib;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System;

public class TelloController : SingletonMonoBehaviour<TelloController> {

	public ControlUI controlUI;
	public InputField INP_SDKCommand;
	private static bool isLoaded = false;
	private TelloVideoTexture telloVideoTexture;

	public enum TelloBTN
	{
		Up,Down,Right,Left,
		W,S,D,A,Q,E
	}

	public float AD_Value = 1;
	public float WS_Value = 1;
	public float Move_Value = 1;
	public float AD_ValueOne = 1;
	public float WS_ValueOne = 1;
	public float Move_ValueOne = 1;
	public bool btn_Up;
	public bool btn_Down;
	public bool btn_Right;
	public bool btn_Left;
	public bool btn_W;
	public bool btn_S;
	public bool btn_D;
	public bool btn_A;
	public bool btn_Q;
	public bool btn_E;
	public bool keyMode = false;

	// FlipType is used for the various flips supported by the Tello.
	public enum FlipType
	{

		// FlipFront flips forward.
		FlipFront = 0,

		// FlipLeft flips left.
		FlipLeft = 1,

		// FlipBack flips backwards.
		FlipBack = 2,

		// FlipRight flips to the right.
		FlipRight = 3,

		// FlipForwardLeft flips forwards and to the left.
		FlipForwardLeft = 4,

		// FlipBackLeft flips backwards and to the left.
		FlipBackLeft = 5,

		// FlipBackRight flips backwards and to the right.
		FlipBackRight = 6,

		// FlipForwardRight flips forewards and to the right.
		FlipForwardRight = 7,
	};

	// VideoBitRate is used to set the bit rate for the streaming video returned by the Tello.
	public enum VideoBitRate
	{
		// VideoBitRateAuto sets the bitrate for streaming video to auto-adjust.
		VideoBitRateAuto = 0,

		// VideoBitRate1M sets the bitrate for streaming video to 1 Mb/s.
		VideoBitRate1M = 1,

		// VideoBitRate15M sets the bitrate for streaming video to 1.5 Mb/s
		VideoBitRate15M = 2,

		// VideoBitRate2M sets the bitrate for streaming video to 2 Mb/s.
		VideoBitRate2M = 3,

		// VideoBitRate3M sets the bitrate for streaming video to 3 Mb/s.
		VideoBitRate3M = 4,

		// VideoBitRate4M sets the bitrate for streaming video to 4 Mb/s.
		VideoBitRate4M = 5,

	};

	override protected void Awake()
	{
		if (!isLoaded) {
			DontDestroyOnLoad(this.gameObject);
			isLoaded = true;
		}
		base.Awake();

		Tello.onConnection += Tello_onConnection;
		Tello.onUpdate += Tello_onUpdate;
		Tello.onVideoData += Tello_onVideoData;

		if (telloVideoTexture == null)
			telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

	}

	private void OnEnable()
	{
		if (telloVideoTexture == null)
			telloVideoTexture = FindObjectOfType<TelloVideoTexture>();
	}

	private void Start()
	{
		if (telloVideoTexture == null)
			telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

		Tello.startConnecting();

		INP_SDKCommand.onEndEdit.AddListener(x => {
			Tello.Client.SendUTF(x);
			INP_SDKCommand.text = "";
		});

		keyMode = false;
	}

	void OnApplicationQuit()
	{
		Tello.stopConnecting();
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Q)) {
			controlUI.DownValues();
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			controlUI.UpValues();
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			Tello.takeOff();
		} else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) {
			Tello.land();
		}

		float lx = 0f;
		float ly = 0f;
		float rx = 0f;
		float ry = 0f;

		if(keyMode){
			return;
		}

		if (Input.GetKey(KeyCode.UpArrow) || btn_Up) {
			ry = Move_Value;
		}
		if (Input.GetKey(KeyCode.DownArrow) || btn_Down) {
			ry = -Move_Value;
		}
		if (Input.GetKey(KeyCode.RightArrow) || btn_Right) {
			rx = Move_Value;
		}
		if (Input.GetKey(KeyCode.LeftArrow) || btn_Left) {
			rx = -Move_Value;
		}
		if (Input.GetKey(KeyCode.W) || btn_W) {
			ly = WS_Value;
		}
		if (Input.GetKey(KeyCode.S) || btn_S) {
			ly = -WS_Value;
		}
		if (Input.GetKey(KeyCode.D) || btn_D) {
			lx = AD_Value;
		}
		if (Input.GetKey(KeyCode.A) || btn_A) {
			lx = -AD_Value;
		}
		Tello.controllerState.setAxis(lx, ly, rx, ry);
	}

	private void Tello_onUpdate(int cmdId)
	{
		//throw new System.NotImplementedException();
		//Debug.Log("Tello_onUpdate : " + Tello.state);
		//controlUI.ShowInfo($"s {Tello.state}");
		controlUI.ShowBoxInfo(Tello.state.ToStringList());
	}

	private void Tello_onConnection(Tello.ConnectionState newState)
	{
		//throw new System.NotImplementedException();
		//Debug.Log("Tello_onConnection : " + newState);
		if (newState == Tello.ConnectionState.Connected) {
            Tello.queryAttAngle();
            Tello.setMaxHeight(50);

			Tello.setPicVidMode(1); // 0: picture, 1: video
			Tello.setVideoBitRate((int)VideoBitRate.VideoBitRateAuto);
			//Tello.setEV(0);
			Tello.requestIframe();
		}
	}

	private void Tello_onVideoData(byte[] data)
	{
		//Debug.Log("Tello_onVideoData: " + data.Length);
		if (telloVideoTexture != null)
			telloVideoTexture.PutVideoData(data);
	}

	public void TelloTakeOff(){
		Tello.takeOff();
	}
	public void TelloLand(){
		Tello.land();
	}

	public void Tello_W(){
		Tello_Single(0, WS_ValueOne, 0, 0);
	}
	public void Tello_S(){
		Tello_Single(0, -WS_ValueOne, 0, 0);
	}
	public void Tello_A(){
		Tello_Single(-AD_ValueOne, 0, 0, 0);
	}
	public void Tello_D(){
		Tello_Single(AD_ValueOne, 0, 0, 0);
	}
	public void Tello_UP(){
		Tello_Single(0, 0, 0, Move_ValueOne);
	}
	public void Tello_DOWN(){
		Tello_Single(0, 0, 0, -Move_ValueOne);
	}
	public void Tello_LEFT(){
		Tello_Single(0, 0, -Move_ValueOne, 0);
	}
	public void Tello_RIGHT(){
		Tello_Single(0, 0, Move_ValueOne, 0);
	}

	public async void Tello_Single(float lx, float ly, float rx, float ry){
		keyMode = true;
		Tello.controllerState.setAxis(lx, ly, rx, ry);

		await Task.Delay(500);

		Tello.controllerState.setAxis(0, 0, 0, 0);
		keyMode = false;
	}
}
