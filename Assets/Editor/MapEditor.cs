



#if UNITY_EDITOR
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
#endif
public class MapEditor
{
#if UNITY_EDITOR

    // % (Ctrl), # (Shift), & (Alt)
    [MenuItem("Tools/GenerateMap %#m")]
    private static void GnerateMap()
    {
        GenerateByPath("Assets/Resources/Maps");
        GenerateByPath("../simple-3d-mmorpg-server/CS_Server/Common/MapData");

        Debug.Log("MapData Gnerated");
    }

    private static void GenerateByPath(string pathPrefix)
    {
        GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Maps");
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogError("Map Prefabs not found");
            return;
        }

        foreach (GameObject go in gameObjects)
        {
            Tilemap tilemapBase = Util.FindChild<Tilemap>(go, "Tilemap_Base", true);
            Tilemap tilemap = Util.FindChild<Tilemap>(go, "Tilemap_Collision", true);

            using (var writer = File.CreateText($"{pathPrefix}/{go.name}.txt"))
            {
                writer.WriteLine(tilemapBase.cellBounds.xMin);
                writer.WriteLine(tilemapBase.cellBounds.xMax);
                writer.WriteLine(tilemapBase.cellBounds.yMin);
                writer.WriteLine(tilemapBase.cellBounds.yMax);

                for (int y = tilemapBase.cellBounds.yMax; y >= tilemapBase.cellBounds.yMin; y--)
                {
                    for (int x = tilemapBase.cellBounds.xMin; x <= tilemapBase.cellBounds.xMax; x++)
                    {
                        var tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null)
                        {
                            writer.Write("1");
                        }
                        else
                        {
                            writer.Write("0");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }
    }
#endif
}
