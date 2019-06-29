using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

[InitializeOnLoad]
public class FileExtensionGUI
{
    private static Vector2 offset = new Vector2(-25, 0);
    private static GUIStyle style;
    private static StringBuilder sb = new StringBuilder();
    private static string selectedGuid;
    private static HashSet<string> showExt = new HashSet<string>()
    {
        ".tga",
        ".psd",
        ".png",
        ".jpg",
        ".raw",
        ".fbx",
        ".obj",
    };

    static FileExtensionGUI()
    {
        EditorApplication.projectWindowItemOnGUI += HandleOnGUI;
        Selection.selectionChanged += () =>
        {
            if (Selection.activeObject != null)
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out selectedGuid, out long id);
        };

    }

    private static bool ValidString(string str)
    {
        return !string.IsNullOrEmpty(str) && str.Length > 7;
    }

    private static void HandleOnGUI(string guid, Rect selectionRect)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        string extRaw = Path.GetExtension(path);
        if (!showExt.Contains(extRaw))
            return;

        bool selected = false;
        if (ValidString(guid) && ValidString(selectedGuid))
            selected = String.Compare(guid, 0, selectedGuid, 0, 6) == 0;


        sb.Clear().Append(extRaw);
        if (sb.Length > 0)
        {
            sb.Remove(0, 1);
        }

        string ext = sb.ToString();

        if (style == null)
        {
            style = new GUIStyle(EditorStyles.label);
        }

        style.normal.textColor = selected ? new Color32(255, 255, 255, 255) : new Color32(127, 127, 127, 160);
        var size = style.CalcSize(new GUIContent(ext));
        //EditorGUI.DrawRect(selectionRect, new Color(.76f, .76f, .76f));
        selectionRect.x -= size.x + 12;


        Rect offsetRect = new Rect(selectionRect.position, selectionRect.size);
        EditorGUI.LabelField(offsetRect, ext, style);

    }
}