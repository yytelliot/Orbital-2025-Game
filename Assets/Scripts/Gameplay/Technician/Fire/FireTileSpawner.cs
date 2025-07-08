using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireTileSpawner : MonoBehaviour
{
    [Header("References")]
    public Tilemap tilemap;
    public TileBase[] allowedTiles; // Only tiles from this list can be fire targets

    [Header("Spawn Settings")]
    public GameObject firePrefab;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    [Header("Events")]
    [SerializeField] private GameEvent EmergencyRepairComplete;

    private List<Vector3> selectablePositions = new List<Vector3>();
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    public static FireTileSpawner Instance; //Singleton Pattern

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CacheSelectableTiles();

    }

    private void CacheSelectableTiles()
    {
        selectablePositions.Clear();
        BoundsInt bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase currentTile = tilemap.GetTile(pos);
            if (IsSelectableTile(currentTile))
            {
                Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
                selectablePositions.Add(worldPos);
            }
        }
    }

    public bool IsSelectableTile(TileBase tile)
    {
        foreach (TileBase allowed in allowedTiles)
        {
            if (tile == allowed)
                return true;
        }
        return false;
    }

    public void SpawnFireTiles()
    {
        FireUIHandler.Instance.addFire();
        List<Vector3> availablePositions = selectablePositions.FindAll(pos => !occupiedPositions.Contains(pos));
        int randIndex = Random.Range(0, selectablePositions.Count);

        Vector3 spawnPos = availablePositions[Random.Range(0, availablePositions.Count)];
        Vector3 offsetSpawnPos = new Vector3(spawnPos.x + xOffset, spawnPos.y + yOffset, spawnPos.z);
        GameObject fire = Instantiate(firePrefab, offsetSpawnPos, Quaternion.identity);
        occupiedPositions.Add(spawnPos);

        // Track when the fire is destroyed so we can reuse the position
        FireTracker tracker = fire.GetComponent<FireTracker>();
        tracker.Init(spawnPos, () => occupiedPositions.Remove(spawnPos));
    }

    public void FireExtinguished()
    {
        FireUIHandler.Instance.removeFire();
        EmergencyRepairComplete.RaiseNetworked(this, null);
    }
    

}
