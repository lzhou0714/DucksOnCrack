using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using Photon.Pun;

public class GenerateMap : MonoBehaviour
{
    public List<GameObject> mapTiles = new List<GameObject>();
    public float scale = 10.0f;
    private List<Vector2> positions = new List<Vector2>();
    
    // Start is called before the first frame update
    void Start()
    {
        
        positions.Add(new Vector2(0, 0));
        positions.Add(new Vector2(-scale, 0));
        positions.Add(new Vector2(scale, 0));
        positions.Add(new Vector2(0, scale));
        positions.Add(new Vector2(-scale, scale));
        positions.Add(new Vector2(scale, scale));
        positions.Add(new Vector2(0, -scale));
        positions.Add(new Vector2(-scale, -scale));
        positions.Add(new Vector2(scale, -scale));
        
        PlaceMap();
    }
    
    void PlaceMap()
    {
        HashSet<int> PosUsed = new HashSet<int>();
        HashSet<int> TileUsed = new HashSet<int>();
        float PosInd;
        float TileInd;
        // float PosInd = Random.Range(0.0f, mapTiles.Length);
        // float TileInd = Random.Range(0.0f, mapTiles.Length);
        
        Debug.Log("ad");
        for (int i = mapTiles.Count-1; i >=0; i--)
        {
            
            PosInd = Random.Range(0.0f, i);
            TileInd = Random.Range(0.0f, i);
            int randomAngle = Random.Range(0, 4) * 90; // Generate a random multiple of 90 (0, 90, 180, or 270)
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle); 


            Vector2 position = positions[(int)PosInd];
            GameObject tile = mapTiles[(int) TileInd];
            positions.RemoveAt((int)PosInd);
            mapTiles.RemoveAt((int)TileInd);
            
            
            // PhotonNetwork.Instantiate(mapTiles[(int)TileInd].name, 
            //     (Vector3)position
            //     ,Quaternion.identity);
            Object.Instantiate(tile, 
            (Vector3)position
            ,randomRotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
