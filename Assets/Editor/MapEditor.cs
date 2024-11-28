#if UNITY_EDITOR
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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
        string path1 = "Assets/Resources/Maps";
        string path2 = "../simple-3d-mmorpg-server/CS_Server/Common/MapData";

        EnsureDirectoryExists(path1);
        EnsureDirectoryExists(path2);

        GenerateByPath(path1);
        GenerateByPath(path2);

        Debug.Log("MapData Generated in JSON format");
    }

    private static void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
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

            var mapData = new MapData
            {
                MapName = go.name,
                Bounds = new BoundsData
                {
                    MinX = tilemapBase.cellBounds.xMin,
                    MaxX = tilemapBase.cellBounds.xMax,
                    MinY = tilemapBase.cellBounds.yMin,
                    MaxY = tilemapBase.cellBounds.yMax
                },
                CollisionData = new List<RowData>()
            };

            for(int y = mapData.Bounds.MaxY; y >= mapData.Bounds.MinY; y--)
            {
                RowData row = new RowData(tilemapBase.cellBounds.size.x + 1);
                for (int x = mapData.Bounds.MinX, col = 0; x <= mapData.Bounds.MaxX; x++, col++)
                {
                    var tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                    row.Columns.Add(tile != null ? 1 : 0);
                }
                mapData.CollisionData.Add(row);
            }

            var json = JsonUtility.ToJson(mapData, true);
            string filePath = $"{pathPrefix}/{go.name}.json";

            try
            {
                File.WriteAllText(filePath, json);
                Debug.Log($"File saved: {filePath}");
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to save file: {filePath}, Error: {ex.Message}");
            }
        }
    }
#endif
}
