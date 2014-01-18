using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The generator used to create new levels.
/// </summary>
public class LevelGenerator : MonoBehaviour
{

    #region Member Variables

    /// <summary>
    /// The maximum allowable width for a map in tiles.
    /// </summary>
    public const int ROOM_WIDTH = 16;

    /// <summary>
    /// The maximum allowable height for a map in tiles.
    /// </summary>
    public const int ROOM_HEIGHT = 16;

	public const int LEVEL_ROOMS_X = 4;
	public const int LEVEL_ROOMS_Y = 4;

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

	public GameObject levelTypeLabel;

    #endregion

    /// <summary>
    /// Do very-first intialization.
    /// </summary>
    private void Awake()
    {
		tiles = new Tile[LEVEL_ROOMS_X * ROOM_WIDTH, LEVEL_ROOMS_Y * ROOM_HEIGHT];
		tileComponents = new GameObject[LEVEL_ROOMS_X * ROOM_WIDTH, LEVEL_ROOMS_Y * ROOM_HEIGHT];
		for (int i = 0; i < LEVEL_ROOMS_X * ROOM_WIDTH; i += 1)
        {
			for (int j = 0; j < LEVEL_ROOMS_Y * ROOM_HEIGHT; j += 1)
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
        //SetDimensions(); // Determine how large the current level is

		int[,] roomTypes = GenerateRooms (LEVEL_ROOMS_X, LEVEL_ROOMS_Y);

		for (int i = 0; i < LEVEL_ROOMS_X; i += 1)
        {
			for (int j = 0; j < LEVEL_ROOMS_Y; j += 1)
			{
				int base_x =  i * ROOM_WIDTH;
				int base_y = j * ROOM_HEIGHT;
				//Debug.Log (roomTypes[i, j].ToString() + " " + base_x.ToString () + " " + base_y.ToString());

				bool has_left_right_exit = roomTypes[i, j] > 0;
				bool has_top_exit = roomTypes[i, j] == 2;
				bool has_bottom_exit = roomTypes[i, j] == 3 || (roomTypes[i, j] == 2 && j > 0 && roomTypes[i, j-1] == 2);

				for (int x = 0; x < ROOM_WIDTH; x += 1) {
					for(int y = 0; y < ROOM_HEIGHT; y += 1) {
						int full_x = base_x + x;
						int full_y = base_y + y;

						if( (y == 7 || y == 8) && has_left_right_exit ) {
							Debug.Log ("happens");
							tiles[full_x, full_y].type = Tile.TileOption.Floor;
						} else if ((x == 7 || x == 8) && y > 8 && has_top_exit) {
							tiles[full_x, full_y].type = Tile.TileOption.Floor;
						} else if ((x == 7 || x == 8) && y < 7 && has_bottom_exit) {
							tiles[full_x, full_y].type = Tile.TileOption.Floor;
						}

						GameObject prefab = tiles[full_x, full_y].type == Tile.TileOption.Floor ? tileFloor : tileEmpty;

						tileComponents[full_x, full_y] = Instantiate(prefab, new Vector3(full_x, full_y), Quaternion.identity) as GameObject;
						tileComponents[full_x, full_y].transform.parent = this.transform;
					}
				}

				// Add a room label for debugging purposes (does not work)
				/*GameObject label = Instantiate(levelTypeLabel, new Vector3(base_x, base_y), Quaternion.identity) as GameObject;
				GUIText text = label.GetComponent<GUIText>();
				text.text = roomTypes[i, j].ToString();
				text.transform.parent = this.transform;*/
            }
        }

        return;
    }
	
	/// <summary>
	/// Generates the rooms.
	/// 
	/// Room types:
	/// 0 - no guarantees
	/// 1 - guarantees left and right exits
	/// 2 - guarantees left, right, and up exit. Guarantees down exit if another 2 below it
	/// 3 - guarantees left, right, and down exit
	/// 
	/// 4 - room with an exit
	/// </summary>
	private int[,] GenerateRooms(int width, int height)
	{
		int[,] roomTypes = new int[width, height];
		int row = 0;
		int col = Random.Range (0, width - 1);
		int? lastType = null;
		int? lastDir = null;

		while(true) {
			roomTypes[col, row] = lastType == 2 ? Random.Range(2, 3) : 1;
			lastType = roomTypes[row, col];
			int moveDir = Random.Range(1, 5);

			if(moveDir < 3 && col > 0 && (lastDir == null || lastDir < 3 || lastDir > 4)) { // Try and move left
				col -= 1;
				lastDir = 1;
				continue;
			}

			if(moveDir < 5 && col < width - 1 && (lastDir == null || lastDir > 2)) { // Try and move right
				col += 1;
				lastDir = 3;
				continue;
			} 

			// Try and move up
			lastDir = 5;
			if(row < height - 1) {
				roomTypes[col, row] = 2;
				lastType = roomTypes[col, row];
				row += 1;
			} else {
				roomTypes[col, row] = 4;
				return roomTypes;
			}
		}
	}

    /// <summary>
    /// Reset the dimensions for the current map.
    /// </summary>
    private void SetDimensions()
    {
        /*mapWidth = MAX_MAP_WIDTH;
        mapHeight = MAX_MAP_HEIGHT;*/
        return;
    }

    // Update is called once per frame
    void Update()
    {
        return;
    }

}
