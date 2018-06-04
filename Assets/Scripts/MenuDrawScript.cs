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

    [MenuItem("Scripts/Assign Tile Script")]    //AssignTileScript
    public static void AssignTileScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].AddComponent<Tile>();
        }
    }
}

#endif