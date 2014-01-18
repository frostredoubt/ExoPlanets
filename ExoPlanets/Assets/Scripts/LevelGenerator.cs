using UnityEngine;
using System.Collections;

/// <summary>
/// The generator used to create new levels.
/// </summary>
public class LevelGenerator : MonoBehaviour
{

    #region Member Variables

    /// <summary>
    /// TODO: Temporary tile size. This should be programmatically determined according to screen size / asset size.
    /// </summary>
    public const int TILE_SIZE = 64;

    /// <summary>
    /// The maximum allowable width for a map in tiles.
    /// </summary>
    public const int MAX_MAP_WIDTH = 64;

    /// <summary>
    /// The maximum allowable height for a map in tiles.
    /// </summary>
    public const int MAX_MAP_HEIGHT = 64;

    /// <summary>
    /// The current width of the map in tiles.
    /// </summary>
    private int mapWidth;

    /// <summary>
    /// The public accessor for MapWidth.
    /// </summary>
    public int MapWidth
    {
        get
        {
            return mapWidth;
        }
    }

    /// <summary>
    /// The current height of the map in tiles.
    /// </summary>
    private int mapHeight;

    /// <summary>
    /// The public accessor for mapHeight.
    /// </summary>
    public int MapHeight
    {
        get
        {
            return mapHeight;
        }
    }

    /// <summary>
    /// The array of tiles contained in the map.
    /// </summary>
    private Tile[,] tiles;

    /// <summary>
    /// The associated component array of tiles for a map.
    /// </summary>
    public GameObject[,] tileComponents;

    #endregion

    #region Prefabs

    /// <summary>
    /// Assign the empty tile.
    /// </summary>
    public GameObject tileEmpty;

    /// <summary>
    /// Assign the floor tile.
    /// </summary>
    public GameObject tileFloor;

    #endregion

    /// <summary>
    /// Do very-first intialization.
    /// </summary>
    private void Awake()
    {
        tiles = new Tile[MAX_MAP_WIDTH, MAX_MAP_HEIGHT];
        tileComponents = new GameObject[MAX_MAP_WIDTH, MAX_MAP_HEIGHT];
        for (int i = 0; i < MAX_MAP_WIDTH; i += 1)
        {
            for (int j = 0; j < MAX_MAP_HEIGHT; j += 1)
            {
                tiles[i, j] = new Tile(Tile.TileOption.Empty);
            }
        }
        GenerateLevel();
        return;
    }

    /// <summary>
    /// The entry point where intialization occurs just prior to level-loading.
    /// </summary>
    private void Start()
    {
        return;
    }

    /// <summary>
    /// Perform the actual heavy-lifting involved with generating a new level.
    /// </summary>
    private void GenerateLevel()
    {
        SetDimensions(); // Determine how large the current level is

        for (int i = 0; i < mapWidth; i += 1)
        {
            for (int j = 0; j < mapHeight; j += 1)
            {
                tiles[i, j].type = Tile.TileOption.Floor;
                tileComponents[i, j] = Instantiate(tileFloor, new Vector3(i, j), Quaternion.identity) as GameObject;
                tileComponents[i, j].transform.parent = this.transform;
            }
        }

        return;
    }

    /// <summary>
    /// Reset the dimensions for the current map.
    /// </summary>
    private void SetDimensions()
    {
        mapWidth = MAX_MAP_WIDTH;
        mapHeight = MAX_MAP_HEIGHT;
        return;
    }

    // Update is called once per frame
    void Update()
    {
        return;
    }

}
