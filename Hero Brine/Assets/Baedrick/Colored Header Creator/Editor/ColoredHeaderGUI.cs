// Created by Dedrick "Baedrick" Koh
// Creates header object in hierachy
// Version 1.0.r0

using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public enum FontStyleOptions
{
	Bold = 0,
	Normal = 1,
	Italic = 2,
	BoldAndItalic = 3
}

public enum TextAlignmentOptions
{
	Center = 0,
	Left = 1,
	Right = 2
}

// Settings
public class ColoredHeaderSettings : ScriptableObject
{
	[HideInInspector] public bool dropShadow = false;
	[HideInInspector] public Color headerColor = Color.gray;
	[HideInInspector] public FontStyleOptions fontStyleOptions = FontStyleOptions.Bold;
	[HideInInspector] public TextAlignmentOptions textAlignmentOptions = TextAlignmentOptions.Center;
	[HideInInspector] public float fontSize = 12f;
	[HideInInspector] public Color fontColor = Color.white;
}

public class ColoredHeaderCreatorWindow : EditorWindow
{
	static ColoredHeaderSettings _settings;

	string bannerIdentifier = "%$";
	string bannerSeperator = "|";
	
	string headerName = "New Header";
	string headerSeperator = "^";

	static bool	dropShadow;
	static Color headerColor;
	static FontStyleOptions fontStyleOptions;
	static TextAlignmentOptions textAlignmentOptions;
	static float fontSize;
	static Color fontColor;

	// Load settings
	static void OnLoad()
	{
		string path = GetAssetPath();
		_settings = AssetDatabase.LoadAssetAtPath<ColoredHeaderSettings>(path);

		if (!_settings)
		{
			Debug.Log("No Settings asset found");
			Debug.Log("Creating a new one, please ignore the warning Unity throws");
			_settings = CreateInstance<ColoredHeaderSettings>();
			AssetDatabase.CreateAsset(_settings, path);
			AssetDatabase.Refresh();
		}
		
		RetrieveLastUsed();
	}

	static string GetAssetPath([CallerFilePath] string callerFilePath = null)
	{
		string folder = Path.GetDirectoryName(callerFilePath);

#if UNITY_EDITOR_WIN
		folder = folder.Substring(folder.LastIndexOf(@"\Assets\", System.StringComparison.Ordinal) + 1);
#else
        folder = folder.Substring(folder.LastIndexOf("/Assets/", System.StringComparison.Ordinal) + 1);
#endif

		return Path.Combine(folder, "Settings.asset");
	}

	static void UpdateLastUsed()
	{
		_settings.dropShadow = dropShadow;
		_settings.headerColor = headerColor;
		_settings.fontStyleOptions = fontStyleOptions;
		_settings.textAlignmentOptions = textAlignmentOptions;
		_settings.fontSize = fontSize;
		_settings.fontColor = fontColor;
	}

	static void RetrieveLastUsed()
	{
		dropShadow = _settings.dropShadow;
		headerColor = _settings.headerColor;
		fontStyleOptions = _settings.fontStyleOptions;
		textAlignmentOptions = _settings.textAlignmentOptions;
		fontSize = _settings.fontSize;
		fontColor = _settings.fontColor;
	}

	// Tool
	[MenuItem("Tools/Colored Header Creator")]
	public static void ShowWindow()
	{
		OnLoad();
		GetWindow<ColoredHeaderCreatorWindow>("Colored Header Creator");
	}

	void OnGUI()
	{
		GUILayout.Space(5);

		GUILayout.Label("Created by Dedrick 'Baedrick' Koh", EditorStyles.miniLabel);
		GUILayout.Label("Version 1.0.r0", EditorStyles.miniLabel);

		GUILayout.Space(10);

		GUILayout.Label("Header Settings", EditorStyles.boldLabel);

		headerName = EditorGUILayout.TextField("Header Name", headerName);
		headerColor = EditorGUILayout.ColorField("Header Color", headerColor);

		GUILayout.Space(10);

		GUILayout.Label("Text Style", EditorStyles.boldLabel);

		textAlignmentOptions = (TextAlignmentOptions)EditorGUILayout.EnumPopup("Text Alignment", textAlignmentOptions);
		fontStyleOptions = (FontStyleOptions)EditorGUILayout.EnumPopup("Font Style", fontStyleOptions);
		fontSize = EditorGUILayout.Slider("Font Size", fontSize, 1, 20);
		fontColor = EditorGUILayout.ColorField("Font Color", fontColor);
		dropShadow = EditorGUILayout.Toggle("Drop Shadow (Slow)", dropShadow);

		GUILayout.FlexibleSpace();

		if (GUILayout.Button("Create Header", GUILayout.MinHeight(50)))
		{
			string gameObjName = FormatHeader(bannerIdentifier, dropShadow, headerColor, textAlignmentOptions, fontStyleOptions, fontSize, fontColor, headerName);
			GameObject obj = new GameObject(gameObjName);
			obj.transform.position = Vector3.zero;
			
			UpdateLastUsed();
		}
		
		GUILayout.Space(2);

		if (GUILayout.Button("Reset To Default"))
		{
			headerName = "New Header";
			dropShadow = false;
			headerColor = Color.gray;
			fontStyleOptions = FontStyleOptions.Bold;
			textAlignmentOptions = TextAlignmentOptions.Center;
			fontSize = 12f;
			fontColor = Color.white;
			
			UpdateLastUsed();
		}

		GUILayout.Space(2);

		if (GUILayout.Button("Delete All Headers"))
		{
			GameObject[] obj = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
			foreach (GameObject o in obj)
			{
				if (o.name.StartsWith(bannerIdentifier + bannerSeperator, System.StringComparison.Ordinal))
				{
					DestroyImmediate(o);
				}
			}
		}
	}

	void OnDestroy()
	{
		UpdateLastUsed();
	}

	// Colour Conversion Functions
	string FloatNormalisedToHex(float value)
	{
		return (Mathf.RoundToInt(value * 255f)).ToString("X2");
	}

	string GetStringFromColour(Color color)
	{
		string red = FloatNormalisedToHex(color.r);
		string green = FloatNormalisedToHex(color.g);
		string blue = FloatNormalisedToHex(color.b);
		return red + green + blue;
	}

	// Prepare variables for header
	string FormatHeader(string bannerIdentifier, bool dropShadow, Color headerColor, TextAlignmentOptions textAlignmentOptions, FontStyleOptions fontStyleOptions, float fontSize, Color fontColor, string headerName)
	{
		string dropShadowString;
		string headerColorString;
		string fontStyleOptionsString;
		string textAlignmentOptionsString;
		string fontSizeString;
		string fontColorString;

		if (dropShadow)
		{
			dropShadowString = bannerSeperator + "1";
		}
		else
		{
			dropShadowString = bannerSeperator + "0";
		}

		switch (fontStyleOptions)
		{
			case FontStyleOptions.Bold:
				fontStyleOptionsString = bannerSeperator + "0";
				break;
			case FontStyleOptions.Normal:
				fontStyleOptionsString = bannerSeperator + "1";
				break;
			case FontStyleOptions.Italic:
				fontStyleOptionsString = bannerSeperator + "2";
				break;
			case FontStyleOptions.BoldAndItalic:
				fontStyleOptionsString = bannerSeperator + "3";
				break;
			default:
				Debug.Log("If this message comes up something went wrong.");
				fontStyleOptionsString = bannerSeperator + "0";
				break;
		}

		switch (textAlignmentOptions)
		{
			case TextAlignmentOptions.Center:
				textAlignmentOptionsString = bannerSeperator + "0";
				break;
			case TextAlignmentOptions.Right:
				textAlignmentOptionsString = bannerSeperator + "1";
				break;
			case TextAlignmentOptions.Left:
				textAlignmentOptionsString = bannerSeperator + "2";
				break;
			default:
				Debug.Log("If this message comes up something went wrong.");
				textAlignmentOptionsString = bannerSeperator + "0";
				break;
		}

		fontSizeString = bannerSeperator + Mathf.RoundToInt(fontSize).ToString();
		fontColorString = bannerSeperator + GetStringFromColour(fontColor);
		headerColorString = bannerSeperator + GetStringFromColour(headerColor);
		headerName = headerSeperator + headerName;
		
		return bannerIdentifier + dropShadowString + headerColorString + textAlignmentOptionsString + fontStyleOptionsString + fontSizeString + fontColorString + headerName;
	}
}
#endif