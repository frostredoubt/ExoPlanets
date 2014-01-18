using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;


/// <summary>
/// The generator used to create new levels.
/// </summary>
public class LevelGenerator : MonoBehaviour
{

    #region Member Variables and Constants

    /// <summary>
    /// The width of a level in templates.
    /// </summary>
    public const int LEVEL_TEMPLATE_WIDTH = 4;

    /// <summary>
    /// The height of a level in templates.
    /// </summary>
    public const int LEVEL_TEMPLATE_HEIGHT = 4;

    #endregion

    #region Editor-Exposed Member Variables

    // Note: These members should be set through the inspector, not the code

    /// <summary>
    /// Assign the empty tile.
    /// </summary>
    public GameObject tileEmpty;

    /// <summary>
    /// Assign the floor tile.
    /// </summary>
    public GameObject tileFloor;

    /// <summary>
    /// The list of level templates that are usable by the generator.
    /// </summary>
    public GameObject[] levelTemplates;

    #endregion

    /// <summary>
    /// Do very-first intialization.
    /// </summary>
    private void Awake()
    {

        // Create the associative list of templates containing exit types
        foreach (Template.Exit exit in Enum.GetValues(typeof(Template.Exit)))
        {
            Template.exitLookupSet.Add(exit, new HashSet<GameObject>());
        }
        for (int i = 0; i < levelTemplates.Length; i += 1)
        {
            TemplateAnnouncement templateAnnouncement = levelTemplates[i].GetComponent<TemplateAnnouncement>();
            for (int j = 0; j < templateAnnouncement.supportedExits.Length; j += 1)
            {
                Template.exitLookupSet[templateAnnouncement.supportedExits[i]].Add(levelTemplates[i]);
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
        TemplateRequirementSet[,] templateRequirements = GenerateTemplates(LEVEL_TEMPLATE_WIDTH, LEVEL_TEMPLATE_HEIGHT);

        StringBuilder test = new StringBuilder();
        /*
        for (int i = 0; i < templateRequirements.GetLength(0); i += 1)
        {
            test.
        }*/

        //for (int i = 0; i < LEVEL_TEMPLATE_WIDTH; i += 1)
        //{
        //    for (int j = 0; j < LEVEL_TEMPLATE_HEIGHT; j += 1)
        //    {
        //        int base_x = i * TEMPLATE_TILE_WIDTH;
        //        int base_y = j * TEMPLATE_TILE_HEIGHT;
        //        //Debug.Log (roomTypes[i, j].ToString() + " " + base_x.ToString () + " " + base_y.ToString());

        //        bool has_left_right_exit = roomTypes[i, j] > 0;
        //        bool has_top_exit = roomTypes[i, j] == 2;
        //        bool has_bottom_exit = roomTypes[i, j] == 3 || (roomTypes[i, j] == 2 && j > 0 && roomTypes[i, j - 1] == 2);

        //        for (int x = 0; x < TEMPLATE_TILE_WIDTH; x += 1)
        //        {
        //            for (int y = 0; y < TEMPLATE_TILE_HEIGHT; y += 1)
        //            {
        //                int full_x = base_x + x;
        //                int full_y = base_y + y;

        //                if ((y == 7 || y == 8) && has_left_right_exit)
        //                {
        //                    Debug.Log("happens");
        //                    tiles[full_x, full_y].type = Tile.TileOption.Floor;
        //                }
        //                else if ((x == 7 || x == 8) && y > 8 && has_top_exit)
        //                {
        //                    tiles[full_x, full_y].type = Tile.TileOption.Floor;
        //                }
        //                else if ((x == 7 || x == 8) && y < 7 && has_bottom_exit)
        //                {
        //                    tiles[full_x, full_y].type = Tile.TileOption.Floor;
        //                }

        //                GameObject prefab = tiles[full_x, full_y].type == Tile.TileOption.Floor ? tileFloor : tileEmpty;

        //                tileComponents[full_x, full_y] = Instantiate(prefab, new Vector3(full_x, full_y), Quaternion.identity) as GameObject;
        //                tileComponents[full_x, full_y].transform.parent = this.transform;
        //            }
        //        }

        //        // Add a room label for debugging purposes (does not work)
        //        /*GameObject label = Instantiate(levelTypeLabel, new Vector3(base_x, base_y), Quaternion.identity) as GameObject;
        //        GUIText text = label.GetComponent<GUIText>();
        //        text.text = roomTypes[i, j].ToString();
        //        text.transform.parent = this.transform;*/
        //    }
        //}

        return;
    }

    /// <summary>
    /// Generates the templates used by a level.
    /// </summary>
    /// <param name="levelTemplateHeight">The height of the level to generate in templates.</param>
    /// <param name="levelTemplateWidth">The width of the level to generate in templates.</param>
    private TemplateRequirementSet[,] GenerateTemplates(int levelTemplateHeight, int levelTemplateWidth)
    {
        TemplateRequirementSet[,] templateRequirements = new TemplateRequirementSet[levelTemplateHeight, levelTemplateWidth];
        Template.Direction? lastDirection = null;
        int currentRow = 0, currentColumn = UnityEngine.Random.Range(0, levelTemplateWidth);

        // Set up new requirement sets for all templates
        for (int i = 0; i < levelTemplateHeight; i += 1)
        {
            for (int j = 0; j < levelTemplateWidth; j += 1)
            {
                templateRequirements[i, j] = new TemplateRequirementSet();
            }
        }

        templateRequirements[currentRow, currentColumn].features.Add(Template.Feature.Entrance); // Set up the entrance

        while (true)
        {
            Template.Direction? moveDirection = GetMoveDirection(currentRow, currentColumn, levelTemplateHeight, levelTemplateWidth, lastDirection); // Get the direction to move in

            if (moveDirection == null) // If the direction is null, then we should be placing an exit and ending
            {
                templateRequirements[currentRow, currentColumn].features.Add(Template.Feature.Exit);
                return templateRequirements;
            }
            templateRequirements[currentRow, currentColumn].exitDirections.Add(moveDirection.Value); // Add the move direction

            switch (moveDirection.Value) // Move in the desired direction
            {
                case Template.Direction.Left:
                    currentColumn -= 1;
                    break;
                case Template.Direction.Right:
                    currentColumn += 1;
                    break;
                case Template.Direction.Up:
                    currentRow += 1;
                    break;
                default:
                    throw new InvalidOperationException("Selected move direction is invalid for the current state of the level generator.");
            }
        }
    }

    /// <summary>
    /// Get a random generation direction to follow based on current position in the level and the last direction moved in.
    /// </summary>
    /// <param name="currentRow">The current row position in templates.</param>
    /// <param name="currentColumn">The current column position in templates.</param>
    /// <param name="levelTemplateHeight">The maximum height of the map in templates.</param>
    /// <param name="levelTemplateWidth">The maximum width of the map in templates.</param>
    /// <param name="lastDirection">The last direction moved in (possibly null).</param>
    private Template.Direction? GetMoveDirection(int currentRow, int currentColumn, int levelTemplateHeight, int levelTemplateWidth, Template.Direction? lastDirection)
    {
        List<Template.Direction> validDirections = new List<Template.Direction>();
        Template.Direction chosenDirection;
        int randomUpper = 1;

        if ((lastDirection == null || lastDirection != Template.Direction.Right) && (currentColumn > 0)) // Add moving left as a valid direction
        {
            validDirections.Add(Template.Direction.Left); // Cheat to allow a random distribution
            validDirections.Add(Template.Direction.Left);
            randomUpper += 2;
        }
        if ((lastDirection == null || lastDirection != Template.Direction.Left) && (currentColumn < levelTemplateWidth - 1)) // Add moving left as a valid direction
        {
            validDirections.Add(Template.Direction.Right); // Cheat to allow a random distribution
            validDirections.Add(Template.Direction.Right);
            randomUpper += 2;
        }

        validDirections.Add(Template.Direction.Up); // Moving upwards is always a valid "choice", but if chosen on the top row, we return null (which tells the map to place an exit)
        
        chosenDirection = validDirections[UnityEngine.Random.Range(0, randomUpper)];
        if (chosenDirection == Template.Direction.Up && currentRow == levelTemplateHeight - 1) // If we're moving up on the final row, simply terminate
        {
            return null;
        }
        return chosenDirection;
    }

    // Update is called once per frame
    void Update()
    {
        return;
    }

}
