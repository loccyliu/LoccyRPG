using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Const
{
	/// 调试模式-用于内部测试
	public static bool DebugMode = false;
	/// 调试模式
	public static bool UpdateMode = false;
	/// The type of the log.
	public static LogType logType = LogType.LogScreen;
	/// The timer interval.
	public static int TimerInterval = 1;
	/// FPS
	public static int GameFrameRate = 30;
	/// The use pbc.
	public static bool UsePbc = true;
	/// The use lpeg.
	public static bool UseLpeg = true;
	/// Protobuff-lua-gen
	public static bool UsePbLua = true;
	/// CJson
	public static bool UseCJson = true;
	/// 使用LUA编码
	public static bool LuaEncode = false;
	/// 用户ID
	public static string UserId = string.Empty;
	/// 应用程序名称
	public static string AppName = "rpgframework";
	/// 应用程序前缀
	public static string AppPrefix = AppName + "_";
	/// 素材扩展名
	public static string ExtName = ".unity3d";
	/// 素材目录
	public static string AssetDirname = "StreamingAssets";
	/// 测试更新地址
	public static string WebUrl = "http://www.cn/res/";
	/// Socket服务器端口
	public static int SocketPort = 0;
	/// Socket服务器地址
	public static string SocketAddress = string.Empty;

	public static string PopViewRoot = "Canvas/PopViews";
	public static string CDN = "";
	public static string NextLevel = "Start";
}