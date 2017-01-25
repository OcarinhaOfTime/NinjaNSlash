using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {
    public TextAsset levelRaw;
    public GameObject[] tiles;
    public GameObject player;
    public CameraFollower cameraFollower;
    public bool debugMode;

    private Vector3 origin;
    private float tileWidth;
    private float tileHeight;

    private int[,] map;

    void Start() {
        var size = tiles[0].GetComponent<Renderer>().bounds.size;
        tileWidth = size.x;
        tileHeight = size.y;

        origin = - Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight)) + size / 2;

        ParseLevel();
        GenerateMap();
        if(!debugMode) {
            player.SetActive(true);
            cameraFollower.enabled = true;
        }        
    }

    void GenerateMap() {
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                if(map[i, j] == 0)
                    continue;

                var tile = Instantiate(tiles[map[i, j] - 1]);
                tile.transform.SetParent(transform);
                tile.transform.position = origin + Vector3.right * j * tileWidth + Vector3.up * i * tileHeight;
            }
        }
    }

    void CreateTile() {

    }

    void PrintMap() {
        string output = "";
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                output += map[i, j] + " ";
            }
            output += "\n";
        }

        Debug.Log(output);
    }

    public void ParseLevel() {
        string[] lines = levelRaw.text.Split('\n');
        map = new int[lines.Length, lines[0].Split(' ').Length];

        for(int i=0; i< lines.Length; i++) {
            string[] line = lines[i].Split(' ');
            for(int j=0; j<line.Length; j++) {
                map[i, j] = int.Parse(line[j]);
            }
        }
    }
    
    void TestParse() {

    }    
}
