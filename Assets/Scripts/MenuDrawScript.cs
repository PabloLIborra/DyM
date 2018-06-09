using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

public class MenuDrawScript
{
    [MenuItem("Scripts/Assign Tile Material")]  //AssignTileMaterial
    public static void AssignTileMaterial()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        Material mat = Resources.Load<Material>("Tile");

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<Renderer>().material = mat;
        }
    }

    [MenuItem("Scripts/Remove Tile Material")]    //RemoveTileMaterial
    public static void RemoveTileMaterial()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        for (int i = 0; i < tiles.Length; i++)
        {
            Material mat = tiles[i].GetComponent<Renderer>().material;

            if (mat != null)
            {
                Object.DestroyImmediate(mat as Object, true);
            }

        }
    }

    [MenuItem("Scripts/Assign Tile Script")]    //AssignTileScript
    public static void AssignTileScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].AddComponent<Tile>();
        }
    }

    [MenuItem("Scripts/Remove Tile Script")]    //RemoveTileScript
    public static void RemoveTileScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        for (int i = 0; i < tiles.Length; i++)
        {
            Component comp = tiles[i].GetComponent<Tile>();

            if(comp != null)
            {
                Object.DestroyImmediate(comp as Object, true);
            }

        }
    }

    [MenuItem("Scripts/Rename Tiles Script")]    //RemoveTileScript
    public static void RenameTileScript()
    {
        GameObject[] rows = GameObject.FindGameObjectsWithTag("Row");

        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].name = "Row" + i;
            for (int x = 0; x < rows[i].transform.childCount; x++)
            {
                rows[i].transform.GetChild(x).name = "Tile" + i + x;
            }
        }
        
    }
}

#endif