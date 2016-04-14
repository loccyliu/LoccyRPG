using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 批量图片资源导入设置
/// 使用说明： 在Assets文件夹下创建Editor文件夹，将复制或拷贝该代码--
/// 保存成TextureImportSetting.cs放入Editor文件夹,将该脚本放入该文件--
/// 夹选择需要批量设置的贴图，单击Costom/Texture Import Settings，打--
/// 开窗口后选择对应参数，点击Set Texture ImportSettings，稍等片刻，--
/// 批量设置成功。
/// </summary>
public class CustomTextureImportSetting : EditorWindow
{
    /// <summary>
    /// 临时存储int[]
    /// </summary>
    private int[] IntArray = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

    //AnisoLevel
    private int AnisoLevel = 1;
    //Filter Mode
    private int FilterModeInt = 1;
    private string[] FilterModeString = new string[] {"Point", "Bilinear", "Trilinear"};
    //Wrap Mode
    private int WrapModeInt = 0;
    private string[] WrapModeString = new string[] {"Repeat", "Clamp"};
    //Texture Type
    private int TextureTypeInt = 8;

    private string[] TextureTypeString = new string[]
    {
        "Texture", "Normal Map", "GUI(Editor\\Legacy)", "Refelection", "Cookie", "Lightmap", "Advanced", "Cursor",
        "Sprite(2D\\uGui)"
    };

    //Max Size
    private int MaxSizeInt = 6;
    private string[] MaxSizeString = new string[] {"32", "64", "128", "256", "512", "1024", "2048", "4096"};
    //Format
    private int FormatInt = 2;

    private string[] FormatString = new string[]
    {"Compressed", "16 bits", "TrueColor", "PVRTC_RGBA2", "PVRTC_RGBA4", "RGBA24", "DXT1", "DXT5"};

    private int PlatformInt = 0;
    private string[] PlatformString = new string[] {"Default", "Android", "iPhone", "Flash"};

    private int iphoneFormatInt = 0;
    private int androidFormatInt = 0;

    /// <summary>
    /// 创建、显示窗体
    /// </summary>
	[@MenuItem("EditorTools/Texture Import Settings")]
    private static void Init()
    {
        CustomTextureImportSetting window =
            (CustomTextureImportSetting) GetWindow(typeof(CustomTextureImportSetting), true, "TextureImportSetting");
        window.Show();
    }

    /// <summary>
    /// 显示窗体里面的内容
    /// </summary>
    private void OnGUI()
    {
        //AnisoLevel
        GUILayout.BeginHorizontal();
        GUILayout.Label("Aniso Level  ");
        AnisoLevel = EditorGUILayout.IntSlider(AnisoLevel, 0, 9);
        GUILayout.EndHorizontal();

        //Filter Mode
        FilterModeInt = EditorGUILayout.IntPopup("Filter Mode", FilterModeInt, FilterModeString, IntArray);

        //Wrap Mode
        WrapModeInt = EditorGUILayout.IntPopup("Wrap Mode", WrapModeInt, WrapModeString, IntArray);

        //Texture Type
        TextureTypeInt = EditorGUILayout.IntPopup("Texture Type", TextureTypeInt, TextureTypeString, IntArray);

        //Max Size
        MaxSizeInt = EditorGUILayout.IntPopup("Max Size", MaxSizeInt, MaxSizeString, IntArray);

        //Format
        FormatInt = EditorGUILayout.IntPopup("Format", FormatInt, FormatString, IntArray);

        //PlatformInt = EditorGUILayout.IntPopup("Platform", PlatformInt, PlatformString, IntArray); 

        if (GUILayout.Button("Set Texture ImportSettings"))
            LoopSetTexture();

        GUILayout.FlexibleSpace();

        GUILayout.Label("——————————————————————————————");
        androidFormatInt = EditorGUILayout.IntPopup("androidFormat", androidFormatInt, FormatString, IntArray);
        iphoneFormatInt = EditorGUILayout.IntPopup("iphoneFormat", iphoneFormatInt, FormatString, IntArray);
        if (GUILayout.Button("Set Mobile Texture ImportSettings"))
            SetTexSetting();
    }

    /// <summary>
    /// 获取贴图设置
    /// </summary>
    public TextureImporter GetTextureSettings(string path)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter == null)
        {
            Debug.LogError("textureImporter == null");
            return null;
        }
        //AnisoLevel
        textureImporter.anisoLevel = AnisoLevel;

        //Filter Mode
        switch (FilterModeInt)
        {
            case 0:
                textureImporter.filterMode = FilterMode.Point;
                break;
            case 1:
                textureImporter.filterMode = FilterMode.Bilinear;
                break;
            case 2:
                textureImporter.filterMode = FilterMode.Trilinear;
                break;
        }

        //Wrap Mode
        switch (WrapModeInt)
        {
            case 0:
                textureImporter.wrapMode = TextureWrapMode.Repeat;
                break;
            case 1:
                textureImporter.wrapMode = TextureWrapMode.Clamp;
                break;
        }

        //Texture Type
        switch (TextureTypeInt)
        {
            case 0:
                textureImporter.textureType = TextureImporterType.Image;
                break;
            case 1:
                textureImporter.textureType = TextureImporterType.Bump;
                break;
            case 2:
                textureImporter.textureType = TextureImporterType.GUI;
                break;
            case 3:
                textureImporter.textureType = TextureImporterType.Reflection;
                break;
            case 4:
                textureImporter.textureType = TextureImporterType.Cookie;
                break;
            case 5:
                textureImporter.textureType = TextureImporterType.Lightmap;
                break;
            case 6:
                textureImporter.textureType = TextureImporterType.Advanced;
                break;
            case 7:
                textureImporter.textureType = TextureImporterType.Cursor;
                break;
            case 8:
                textureImporter.textureType = TextureImporterType.Sprite;
                break;
        }

        //Max Size 
        switch (MaxSizeInt)
        {
            case 0:
                textureImporter.maxTextureSize = 32;
                break;
            case 1:
                textureImporter.maxTextureSize = 64;
                break;
            case 2:
                textureImporter.maxTextureSize = 128;
                break;
            case 3:
                textureImporter.maxTextureSize = 256;
                break;
            case 4:
                textureImporter.maxTextureSize = 512;
                break;
            case 5:
                textureImporter.maxTextureSize = 1024;
                break;
            case 6:
                textureImporter.maxTextureSize = 2048;
                break;
            case 7:
                textureImporter.maxTextureSize = 4096;
                break;
        }

        //Format
        switch (FormatInt)
        {
            case 0:
                textureImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;
                break;
            case 1:
                textureImporter.textureFormat = TextureImporterFormat.Automatic16bit;
                break;
            case 2:
                textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                break;
            case 3:
                textureImporter.textureFormat = TextureImporterFormat.PVRTC_RGBA2;
                break;
            case 4:
                textureImporter.textureFormat = TextureImporterFormat.PVRTC_RGBA4;
                break;
            case 5:
                textureImporter.textureFormat = TextureImporterFormat.RGB24;
                break;
            case 6:
                textureImporter.textureFormat = TextureImporterFormat.DXT1;
                break;
            case 7:
                textureImporter.textureFormat = TextureImporterFormat.DXT5;
                break;
        }
        return textureImporter;
    }

    /// <summary>
    /// 循环设置选择的贴图
    /// </summary>
    private void LoopSetTexture()
    {
        Object[] textures = GetSelectedTextures();
        Selection.objects = new Object[0];

        foreach (var texture in textures)
        {
            var path = AssetDatabase.GetAssetPath(texture);
            TextureImporter texImporter = GetTextureSettings(path);
            TextureImporterSettings tis = new TextureImporterSettings();
            texImporter.ReadTextureSettings(tis);
            texImporter.SetTextureSettings(tis);
            AssetDatabase.ImportAsset(path);
        }
    }

    /// <summary>
    /// 获取选择的贴图
    /// </summary>
    /// <returns></returns>
    private Object[] GetSelectedTextures()
    {
        return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
    }

    private TextureImporter GetIPhoneTexSettings(string path)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter == null)
        {
            Debug.LogError("textureImporter == null");
            return null;
        }
        switch (iphoneFormatInt)
        {
            case 0:
                textureImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;
                break;
            case 1:
                textureImporter.textureFormat = TextureImporterFormat.Automatic16bit;
                break;
            case 2:
                textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                break;
            case 3:
                textureImporter.textureFormat = TextureImporterFormat.PVRTC_RGBA2;
                break;
            case 4:
                textureImporter.textureFormat = TextureImporterFormat.PVRTC_RGBA4;
                break;
            case 5:
                textureImporter.textureFormat = TextureImporterFormat.RGB24;
                break;
            case 6:
                textureImporter.textureFormat = TextureImporterFormat.DXT1;
                break;
            case 7:
                textureImporter.textureFormat = TextureImporterFormat.DXT5;
                break;
        }
        return textureImporter;
    }

    private TextureImporter GetAndroidTexSettings(string path)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter == null)
        {
            Debug.LogError("textureImporter == null");
            return null;
        }
        switch (androidFormatInt)
        {
            case 0:
                textureImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;
                break;
            case 1:
                textureImporter.textureFormat = TextureImporterFormat.Automatic16bit;
                break;
            case 2:
                textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                break;
            case 3:
                textureImporter.textureFormat = TextureImporterFormat.PVRTC_RGBA2;
                break;
            case 4:
                textureImporter.textureFormat = TextureImporterFormat.PVRTC_RGBA4;
                break;
            case 5:
                textureImporter.textureFormat = TextureImporterFormat.RGB24;
                break;
            case 6:
                textureImporter.textureFormat = TextureImporterFormat.DXT1;
                break;
            case 7:
                textureImporter.textureFormat = TextureImporterFormat.DXT5;
                break;
        }
        return textureImporter;
    }

    private TextureImporterFormat GetIPhoneTexSettings() {

        TextureImporterFormat format = TextureImporterFormat.AutomaticTruecolor;

        switch (iphoneFormatInt) {
            case 0:
                format = TextureImporterFormat.AutomaticCompressed;
                break;
            case 1:
                format = TextureImporterFormat.Automatic16bit;
                break;
            case 2:
                format = TextureImporterFormat.AutomaticTruecolor;
                break;
            case 3:
                format = TextureImporterFormat.PVRTC_RGBA2;
                break;
            case 4:
                format = TextureImporterFormat.PVRTC_RGBA4;
                break;
            case 5:
                format = TextureImporterFormat.RGB24;
                break;
            case 6:
                format = TextureImporterFormat.DXT1;
                break;
            case 7:
                format = TextureImporterFormat.DXT5;
                break;
        }

        return format;
    }

    private TextureImporterFormat GetAndroidTexSettings() {

        TextureImporterFormat format = TextureImporterFormat.AutomaticTruecolor;

        switch (androidFormatInt) {
            case 0:
                format = TextureImporterFormat.AutomaticCompressed;
                break;
            case 1:
                format = TextureImporterFormat.Automatic16bit;
                break;
            case 2:
                format = TextureImporterFormat.AutomaticTruecolor;
                break;
            case 3:
                format = TextureImporterFormat.PVRTC_RGBA2;
                break;
            case 4:
                format = TextureImporterFormat.PVRTC_RGBA4;
                break;
            case 5:
                format = TextureImporterFormat.RGB24;
                break;
            case 6:
                format = TextureImporterFormat.DXT1;
                break;
            case 7:
                format = TextureImporterFormat.DXT5;
                break;
        }

        return format;
    }

    private int GetMaxsize() {

        int maxsize = 2048;

        //Max Size 
        switch (MaxSizeInt) {
            case 0:
                maxsize = 32;
                break;
            case 1:
                maxsize = 64;
                break;
            case 2:
                maxsize = 128;
                break;
            case 3:
                maxsize = 256;
                break;
            case 4:
                maxsize = 512;
                break;
            case 5:
                maxsize = 1024;
                break;
            case 6:
                maxsize = 2048;
                break;
            case 7:
                maxsize = 4096;
                break;
        }

        return maxsize;
    }

    private void SetTexSetting()
    {
        Object[] textures = GetSelectedTextures();
        Selection.objects = new Object[0];

        foreach (var texture in textures)
        {
            var path = AssetDatabase.GetAssetPath(texture);
            TextureImporter texImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (texImporter == null)
            {
                Debug.LogError("textureImporter == null");
                return;
            }

            texImporter.SetPlatformTextureSettings("Android", GetMaxsize(), GetAndroidTexSettings());

            texImporter.SetPlatformTextureSettings("iPhone", GetMaxsize(), GetIPhoneTexSettings());

            TextureImporterSettings tis = new TextureImporterSettings();
            texImporter.ReadTextureSettings(tis);
            texImporter.SetTextureSettings(tis);
            AssetDatabase.ImportAsset(path);
        }
    }
}
