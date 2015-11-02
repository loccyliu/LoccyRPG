/*
 * RpgFramework
 * NoWindow.cs
 * Created by com.loccy on 10/09/2015 14:41:10.
 */

using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class NoWindow : MonoBehaviour
{
	[SerializeField]
	private Material m_Material;
	private struct MARGINS
	{
		public int cxLeftWidth;
		public int cxRightWidth;
		public int cyTopHeight;
		public int cyBottomHeight;
	}

	// Define function signatures to import from Windows APIs
	[DllImport("user32.dll")]
	private static extern IntPtr GetActiveWindow();
	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
	[DllImport("Dwmapi.dll")]

	private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
	// Definitions of window styles
	const int GWL_STYLE = -16;
	const uint WS_POPUP = 0x80000000;
	const uint WS_VISIBLE = 0x10000000;
	void Start()
	{

#if !UNITY_EDITOR

		var margins = new MARGINS() { cxLeftWidth = -1 };

		// Get a handle to the window

		var hwnd = GetActiveWindow();

		// Set properties of the window

		// See: [url]https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591%28v=vs.85%29.aspx[/url]

		SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
		DwmExtendFrameIntoClientArea(hwnd, ref margins);
#endif
	}

	// Pass the output of the camera to the custom material

	// for chroma replacement

	void OnRenderImage(RenderTexture from, RenderTexture to)
	{
		Graphics.Blit(from, to, m_Material);
	}

}
