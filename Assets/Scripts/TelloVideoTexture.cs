using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

public class TelloVideoTexture : MonoBehaviour {

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
	[DllImport("TelloVideoDecoder")]
#endif
	private static extern void SetTextureFromUnity(IntPtr texture, int w, int h);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
	[DllImport("TelloVideoDecoder")]
#endif
	private static extern IntPtr GetRenderEventFunc();

#if UNITY_WEBGL && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern void RegisterPlugin();
#endif

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
	[DllImport("TelloVideoDecoder")]
#endif
	private static extern void UnityPluginEnable();

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
	[DllImport("TelloVideoDecoder")]
#endif
	private static extern void UnityPluginDisable();

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
	[DllImport("TelloVideoDecoder")]
#endif
	private static extern void PutVideoDataFromUnity(IntPtr data, int size);

	private const int Width = 1280;
	private const int Height = 720;
	private const TextureFormat TextureFormat_ = TextureFormat.RGBA32;
	private Texture2D texture;
	public List<UnityEngine.UI.RawImage> UITarget;

	IEnumerator Start()
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		RegisterPlugin();
#endif
		UnityPluginEnable();

		CreateTextureAndPassToPlugin();

		yield return StartCoroutine("CallPluginAtEndOfFrames");
	}

	private void OnApplicationQuit()
	{
		UnityPluginDisable();
	}

	private void CreateTextureAndPassToPlugin()
	{
		texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
		texture.filterMode = FilterMode.Point;
		texture.Apply();

		var rd = GetComponent<Renderer>();
		if(rd) rd.material.mainTexture = texture;
		foreach (var rawimg in UITarget)
		{
			if(rawimg) rawimg.texture = texture;
		}

		SetTextureFromUnity(texture.GetNativeTexturePtr(), texture.width, texture.height);
	}

	private IEnumerator CallPluginAtEndOfFrames()
	{
		while (true) {
			yield return new WaitForEndOfFrame();
			GL.IssuePluginEvent(GetRenderEventFunc(), 1);
		}
	}

	public void PutVideoData(byte[] data)
	{
		IntPtr unmanagedData = Marshal.AllocHGlobal(data.Length);
		Marshal.Copy(data, 0, unmanagedData, data.Length);
		PutVideoDataFromUnity((IntPtr)unmanagedData, data.Length);
		Marshal.FreeHGlobal(unmanagedData);
	}
}
