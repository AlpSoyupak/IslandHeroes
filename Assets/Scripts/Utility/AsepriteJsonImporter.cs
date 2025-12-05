using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AsepriteJsonImporter : EditorWindow
{
    [MenuItem("Tools/Aseprite/Import JSON Sprite Sheet")]
    static void Init()
    {
        var window = ScriptableObject.CreateInstance<AsepriteJsonImporter>();
        window.titleContent = new GUIContent("Aseprite JSON Importer");
        window.ShowUtility();
    }

    Texture2D spriteSheet;
    TextAsset jsonFile;

    void OnGUI()
    {
        GUILayout.Label("Aseprite Sprite Sheet Importer", EditorStyles.boldLabel);
        spriteSheet = (Texture2D)EditorGUILayout.ObjectField("Sprite Sheet", spriteSheet, typeof(Texture2D), false);
        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);

        if (spriteSheet != null && jsonFile != null)
        {
            if (GUILayout.Button("Import & Slice"))
                Import();
        }
    }

    void Import()
    {
        string path = AssetDatabase.GetAssetPath(spriteSheet);
        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        importer.spriteImportMode = SpriteImportMode.Multiple;

        // Parse frames
        var json = JsonUtility.FromJson<AsepriteData>(jsonFile.text);
        List<SpriteMetaData> metas = new List<SpriteMetaData>();

        foreach (var f in json.frames)
        {
            SpriteMetaData meta = new SpriteMetaData();
            meta.name = f.filename;
            meta.rect = new Rect(f.frame.x, json.meta.size.h - f.frame.y - f.frame.h, f.frame.w, f.frame.h);
            meta.pivot = new Vector2(0.5f, 0.5f);
            meta.alignment = (int)SpriteAlignment.Center;
            metas.Add(meta);
        }

        importer.spritesheet = metas.ToArray();
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();

        Debug.Log("Aseprite sheet sliced successfully!");
    }

    [System.Serializable]
    public class AsepriteData
    {
        public Frame[] frames;
        public Meta meta;
    }

    [System.Serializable]
    public class Frame
    {
        public string filename;
        public FrameData frame;
    }

    [System.Serializable]
    public class FrameData
    {
        public int x, y, w, h;
    }

    [System.Serializable]
    public class Meta
    {
        public Size size;
    }

    [System.Serializable]
    public class Size
    {
        public int w, h;
    }
}
