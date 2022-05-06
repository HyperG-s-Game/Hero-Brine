using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

static public class Styles
{
    public static GUIStyle fontStyle;
}

    [InitializeOnLoad]
public static class ColoredHeaderDisplay
{
	static ColoredHeaderDisplay()
	{
		//creates "new" objects in the hierarchy
		EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
	}

	static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
	{
		GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

		if (gameObject != null && gameObject.name.StartsWith("%$|", System.StringComparison.Ordinal))
		{
			string[] headerComponenets = gameObject.name.Split('|');
            string[] headerName = gameObject.name.Split('^');
 
            Color headerColor = GetColourFromString(headerComponenets[2]);
            string textAlignmentOptions = headerComponenets[3];
            string fontStyleOptions = headerComponenets[4];
            string fontSize = headerComponenets[5];
            string fontColor = headerComponenets[6];

            if(fontStyleOptions.Length == 1)
            {
                // Set text style
                Styles.fontStyle = new GUIStyle(EditorStyles.label);

                switch (textAlignmentOptions)
                {
                    case "0":
                        Styles.fontStyle.alignment = TextAnchor.MiddleCenter;
                        break;
                    case "1":
                        Styles.fontStyle.alignment = TextAnchor.MiddleRight;
                        break;
                    case "2":
                        Styles.fontStyle.alignment = TextAnchor.MiddleLeft;
                        break;
                    default:
                        Debug.Log("Please do not modify the banner gameobject.");
                        Styles.fontStyle.alignment = TextAnchor.MiddleCenter;
                        break;
                }

                switch (fontStyleOptions)
                {
                    case "0":
                        Styles.fontStyle.fontStyle = FontStyle.Bold;
                        break;
                    case "1":
                        Styles.fontStyle.fontStyle = FontStyle.Normal;
                        break;
                    case "2":
                        Styles.fontStyle.fontStyle = FontStyle.Italic;
                        break;
                    case "3":
                        Styles.fontStyle.fontStyle = FontStyle.BoldAndItalic;
                        break;
                    default:
                        Debug.Log("Please do not modify the banner gameobject.");
                        Styles.fontStyle.fontStyle = FontStyle.Bold;
                        break;
                }

                Styles.fontStyle.fontSize = int.Parse(fontSize);
                Styles.fontStyle.normal.textColor = GetColourFromString(fontColor);

                if (headerComponenets[1] == "1")
                {
                    headerComponenets[1] = headerComponenets[1].Replace(" ", "");
                    EditorGUI.DrawRect(selectionRect, headerColor);
                    EditorGUI.DropShadowLabel(selectionRect, headerName[1].ToUpperInvariant(), Styles.fontStyle);
                }
                else if (headerComponenets[1] == "0")
                {
                    headerComponenets[1] = headerComponenets[1].Replace(" ", "");
                    EditorGUI.DrawRect(selectionRect, headerColor);
                    EditorGUI.LabelField(selectionRect, headerName[1].ToUpperInvariant(), Styles.fontStyle);
                }
            }
            else
            {
                
			}

            // Colour Conversions
            float HexToFloatNormalised(string hex)
            {
                hex = hex.Replace(" ", "");
                return System.Convert.ToInt32(hex, 16) / 255f;
            }

            Color GetColourFromString(string hexString)
            {
                hexString = hexString.Replace(" ", "");
                float red = HexToFloatNormalised(hexString.Substring(0, 2));
                float green = HexToFloatNormalised(hexString.Substring(2, 2));
                float blue = HexToFloatNormalised(hexString.Substring(4, 2));
                return new Color(red, green, blue);
            }
        }
	}
}
#endif