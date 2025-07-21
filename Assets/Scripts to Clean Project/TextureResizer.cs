using UnityEngine;
using UnityEditor;

public class TextureResizer : EditorWindow
{
    private int textureMaxSize = 512;

    [MenuItem("Tools/Batch Resize Textures")]
    public static void ShowWindow()
    {
        GetWindow<TextureResizer>("Texture Resizer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Texture Resizer", EditorStyles.boldLabel);
        textureMaxSize = EditorGUILayout.IntSlider("Max Size", textureMaxSize, 32, 4096);

        if (GUILayout.Button("Resize All Textures"))
        {
            ResizeAllTextures(textureMaxSize);
        }
    }

    private static void ResizeAllTextures(int targetSize)
    {
        string[] textureGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets" });

        int resizedCount = 0;
        foreach (string guid in textureGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!path.StartsWith("Assets")) continue; // Extra safety

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null && importer.maxTextureSize > targetSize)
            {
                importer.maxTextureSize = targetSize;
                importer.textureCompression = TextureImporterCompression.Compressed;
                importer.SaveAndReimport();
                resizedCount++;
            }
        }

        Debug.Log($"Resized and compressed {resizedCount} textures to max {targetSize}px.");
    }

}
