using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public static class SpriteDividerCaller
{
    [MenuItem("Extension/Divide Textures")]
    public static void DivideImages()
    {
        int horizontalCount =14;
        int verticalCount = 2;

        IEnumerable<Texture> targets = Selection.objects.OfType<Texture>();

        if (!targets.Any())
        {
            Debug.LogWarning("Please selecting textures.");
            return;
        }

        foreach (Texture target in targets)
        {
            SpriteDivider.DividSprite(AssetDatabase.GetAssetPath(target), horizontalCount, verticalCount);
        }
    }
}


public static class SpriteDivider
{
    public static void DividSprite(string texturePath, int horizontalCount, int verticalCount)
    {
        TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.filterMode = FilterMode.Point;
        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

        Texture texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture)) as Texture;
        importer.spritePixelsPerUnit = Mathf.Max(texture.width / horizontalCount, texture.height / verticalCount);
        importer.spritesheet = CreateSpriteMetaDataArray(texture, horizontalCount, verticalCount);

        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
    }


    static SpriteMetaData[] CreateSpriteMetaDataArray(Texture texture, int horizontalCount, int verticalCount)
    {
        float spriteWidth = texture.width / horizontalCount;
        float spriteHeight = texture.height / verticalCount;

        return Enumerable
            .Range(0, horizontalCount * verticalCount)
            .Select(index => {
                int x = index % horizontalCount;
                int y = index / horizontalCount;

                return new SpriteMetaData
                {
                    name = string.Format("{0}_{1}", texture.name, index),
                    rect = new Rect(spriteWidth * x, texture.height - spriteHeight * (y + 1), spriteWidth, spriteHeight)
                };
            })
            .ToArray();
    }
}