using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores a set of consistent settings for Wally
// Fields may not have contradicting values like
//  TileCountX = 10, TileSizeX = 1, WallSizeX = 5
// Note: Using seperate floats to prohibit alteration from outside
struct WallySettings {
	public int TileCountX { get; private set; }
	public int TileCountY { get; private set; }
	public float TileSizeX { get; private set; }
	public float TileSizeY { get; private set; }
	public float TileSizeZ { get; private set; }
	public float WallSizeX { get; private set; }
	public float WallSizeY { get; private set; }


	public WallySettings(int tileCountX, int tileCountY, float tileSizeX, float tileSizeY, float tileSizeZ, float wallSizeX, float wallSizeY) {
		TileCountX = tileCountX;
		TileCountY = tileCountY;
		TileSizeX = tileSizeX;
		TileSizeY = tileSizeY;
		TileSizeZ = tileSizeZ;
		WallSizeX = wallSizeX;
		WallSizeY = wallSizeY;
	}
}

public class ConstructionManager : MonoBehaviour
{
	#region Inspector configurable variables
	[Header("Space setup")] // Settings for the whole room
    [Tooltip("Dimensions of the inner room to generate [m]")]
    public Vector3 roomDimensions;

    [Tooltip("Size of the interactive wall part (wally) [m]")]
    public Vector2 wallySize;

	[Tooltip("Prefab for the floor (face prefab upwards)")]
	public GameObject floorPrefab;

	[Tooltip("Prefab for the ceiling (face prefab downwards)")]
	public GameObject ceilingPrefab;

	[Tooltip("Prefab for the walls (face prefab towards negative z-axis)")]
	public GameObject wallPrefab;

	[Tooltip("Prefab for the frames around wally (face prefab towards negative z-axis)")]
	public GameObject framePrefab;

    [Tooltip("Prefab for the side lights")]
    public GameObject wallLightPrefab;

    [Tooltip("Prefab for the ceiling light")]
    public GameObject ceilingLightPrefab;

    [Tooltip("Prefab for a standing person")]
    public GameObject humanPrefab;

    [Tooltip("Prefab for a door")]
    public GameObject doorPrefab;

    [Tooltip("Prefab for a tiny office")]
    public GameObject officePrefab;

    [Header("Wally setup")] // Settings for the interactive wall (wally)
    [Tooltip("The tile Prefab")]
	public GameObject tilePrefab;

	[Tooltip("Amount of tiles for width and height")]
    public Vector2 amountCubes;

    [Tooltip("Size of a tile [m]")]
    public Vector3 tileSize;

    [Tooltip("Free space between tiles [m]")]
    public float spacing;

    public enum PlacingPriority {
        variableWallSize,
        variableAmountOfTiles,
        variableTileSize,
    }

    [Tooltip("Should the wall be placed by amount or by size?")]
    public PlacingPriority priority;

    [Header("Other")]
	#endregion

    [Tooltip("List of all spawned tiles")]
	public GameObject[,] tiles;
    public TileScript[,] tileScripts;

    [HideInInspector]
    public GameObject constructedRoom;

    #region Private variable declarations
    private Transform roomTransform;
	private Transform wallyFrameTransform;
	private Transform wallyTransform;
	#endregion


	#region Unity methods (Start, Update, ...)
	void Start() {
        if (tileSize.magnitude <= 0) {
            Debug.LogError("tileSize may not be zero.");
			return;
        }

        ConstructRoom();
    }

	void Update() {
	}
	#endregion

	public void ConstructRoom() {
		if (floorPrefab == null || ceilingPrefab == null || wallPrefab == null || framePrefab == null) {
			Debug.LogError("ConstructionManager: Space setup is missing a prefab. Aborting...");
			return;
        }
        if (tilePrefab == null)
        {
            Debug.LogError("ConstructionManager: Wally setup is missing the tile prefab. Aborting...");
            return;
        }

        if (constructedRoom != null)
            Destroy(constructedRoom);


        WallySettings ws;
        if (priority == PlacingPriority.variableWallSize)
            ws = PlaceByAmount();
        else if (priority == PlacingPriority.variableAmountOfTiles)
            ws = PlaceBySize();
        else
            ws = PlaceBySizeAndAmount();

		// Precalculate some variables for object dimensions
		// Extension on one side of the room to fit wally in
		float roomExtension = tileSize.z;
		float halfExtension = roomExtension * 0.5f;
		float halfWidth = roomDimensions.x * 0.5f;
		float halfHeight = roomDimensions.y * 0.5f;
		float halfDepth = roomDimensions.z * 0.5f;

		// Create GameObjects
		GameObject room = new GameObject("Room");
		roomTransform = room.transform;
		GameObject floor = Instantiate(floorPrefab, roomTransform);
		GameObject ceiling = Instantiate(ceilingPrefab, roomTransform);
		GameObject wallFront = Instantiate(wallPrefab, roomTransform);
		GameObject wallBack = Instantiate(wallPrefab, roomTransform);
		GameObject wallLeft = Instantiate(wallPrefab, roomTransform);
        GameObject wallRight = Instantiate(wallPrefab, roomTransform);
		GameObject wallyFrame = new GameObject("Wally Frame");
		GameObject wally = new GameObject("Wally");
		GameObject sceneObjects = new GameObject("Scene Objects");
		wallyFrameTransform = wallyFrame.transform;
        wallyTransform = wally.transform;

		// Set readable names
		floor.name = "Floor";
		ceiling.name = "Ceiling";
		wallFront.name = "Wall front";
		wallBack.name = "Wall back";
		wallLeft.name = "Wall left";
		wallRight.name = "Wall right";

		// Set up scene hierarchy (parenting)
		wallyFrameTransform.parent = roomTransform;
		wallyTransform.parent = wallyFrameTransform;
        sceneObjects.transform.parent = roomTransform;
		/* [OBSOLETE] for instantiated prefabs
		floor.transform.parent = roomTransform;
		ceiling.transform.parent = roomTransform;
		wall_front.transform.parent = roomTransform;
		wall_back.transform.parent = roomTransform;
		wall_left.transform.parent = roomTransform;
		wall_right.transform.parent = roomTransform;*/
         
		// Set correct sizes
		// The room is centered in the origin excluding the extension where wally sits
		//  so the visible space in the room is centered. The room is moved upwards so
		//  origin lays on the floor plane.
		floor.transform.localScale = new Vector3(roomDimensions.z + roomExtension, roomDimensions.x, 1);
		floor.transform.localPosition = new Vector3(0, 0, halfExtension);
		ceiling.transform.localScale = new Vector3(roomDimensions.z + roomExtension, roomDimensions.x, 1);
		ceiling.transform.localPosition = new Vector3(0, roomDimensions.y, halfExtension);
		wallFront.transform.localScale = new Vector3(roomDimensions.x, roomDimensions.y, 1);
		wallFront.transform.localPosition = new Vector3(0, halfHeight, halfDepth + roomExtension);
		wallBack.transform.localScale = new Vector3(roomDimensions.x, roomDimensions.y, 1);
		wallBack.transform.localPosition = new Vector3(0, halfHeight, -halfDepth);
		wallBack.transform.Rotate(0, 180, 0);
		wallLeft.transform.localScale = new Vector3(roomDimensions.x + roomExtension, roomDimensions.y, 1);
		wallLeft.transform.localPosition = new Vector3(-halfWidth, halfHeight, halfExtension);
		wallLeft.transform.Rotate(0, 270, 0);
		wallRight.transform.localScale = new Vector3(roomDimensions.x + roomExtension, roomDimensions.y, 1);
		wallRight.transform.localPosition = new Vector3(halfWidth, halfHeight, halfExtension);
		wallRight.transform.Rotate(0, 90, 0);
		wallyFrame.transform.localPosition = new Vector3(-halfWidth, 0, halfDepth);

        // Set up wally frame
        float frameWidth = (roomDimensions.x - ws.WallSizeX) * 0.5f;
        float frameHeight = roomDimensions.y - ws.WallSizeY;
        wallyTransform.localPosition = new Vector3(frameWidth, 0, 0);

        // Transform origin is wallyFrameTransform, the lower left corner of that wall
        GameObject frameTop = Instantiate(framePrefab, wallyFrameTransform);
        GameObject frameLeft = Instantiate(framePrefab, wallyFrameTransform);
        GameObject frameRight = Instantiate(framePrefab, wallyFrameTransform);
        frameTop.name = "Frame Top";
        frameLeft.name = "Frame Left";
        frameRight.name = "Frame Right";
        frameTop.transform.localScale = new Vector3(roomDimensions.x, frameHeight, 1);
        frameTop.transform.localPosition = new Vector3(halfWidth, ws.WallSizeY + (frameHeight * 0.5f), 0);
        frameLeft.transform.localScale = frameRight.transform.localScale = new Vector3(frameWidth, roomDimensions.y - frameHeight, 1);
        frameLeft.transform.localPosition = new Vector3(frameWidth * 0.5f, ws.WallSizeY * 0.5f, 0);
        frameRight.transform.localPosition = new Vector3(roomDimensions.x - (frameWidth * 0.5f), ws.WallSizeY * 0.5f, 0);

        CreateTiles(ws);
        wally.AddComponent<TileManager>();
        ConstructSceneObjects();


        room.transform.rotation = transform.rotation;
        constructedRoom = room;
    }

    void ConstructSceneObjects() {
        if (wallLightPrefab == null || ceilingLightPrefab == null || humanPrefab == null || officePrefab == null || doorPrefab == null)
        {
            Debug.LogError("ConstructionManager: Object setup is missing a prefab. Aborting...");
            return;
        }

        // Precalculate some variables for object dimensions
        float halfWidth = roomDimensions.x * 0.5f;
        float halfDepth = roomDimensions.z * 0.5f;
        float quarterWidth = halfWidth * 0.5f;
        float quarterDepth = halfDepth * 0.5f;

        // Create Gameobjects
        GameObject sceneObjects = GameObject.Find("Scene Objects");
        Transform sceneObjectsTransform = sceneObjects.transform;
        GameObject wallLightBack1 = Instantiate(wallLightPrefab, sceneObjectsTransform);
        GameObject wallLightBack2 = Instantiate(wallLightPrefab, sceneObjectsTransform);
        GameObject wallLightLeft1 = Instantiate(wallLightPrefab, sceneObjectsTransform);
        GameObject wallLightLeft2 = Instantiate(wallLightPrefab, sceneObjectsTransform);
        GameObject wallLightRight1 = Instantiate(wallLightPrefab, sceneObjectsTransform);
        GameObject wallLightRight2 = Instantiate(wallLightPrefab, sceneObjectsTransform);
        GameObject ceilingLight = Instantiate(ceilingLightPrefab, sceneObjectsTransform);
        GameObject human = Instantiate(humanPrefab, sceneObjectsTransform);
        GameObject door = Instantiate(doorPrefab, sceneObjectsTransform);
        GameObject office = Instantiate(officePrefab, sceneObjectsTransform);


        // Set readable names
        wallLightBack1.name = "Wall Light Back 1";
        wallLightBack2.name = "Wall Light Back 2";
        wallLightLeft1.name = "Wall Light Left 1";
        wallLightLeft2.name = "Wall Light Left 2";
        wallLightRight1.name = "Wall Light Right 1";
        wallLightRight2.name = "Wall Light Right 2";
        ceilingLight.name = "Ceiling Light";
        human.name = "Eric";
        door.name = "Entrance Door";
        office.name = "Office";

        // Positioning the objects
        float heightWallLights = 2.6f;
        wallLightBack1.transform.localPosition = new Vector3(quarterWidth, heightWallLights, -halfDepth);
        wallLightBack2.transform.localPosition = new Vector3(-quarterWidth, heightWallLights, -halfDepth);
        wallLightLeft1.transform.localPosition = new Vector3(halfWidth, heightWallLights, -quarterDepth);
        wallLightLeft1.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
        wallLightLeft2.transform.localPosition = new Vector3(halfWidth, heightWallLights, quarterDepth);
        wallLightLeft2.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
        wallLightRight1.transform.localPosition = new Vector3(-halfWidth, heightWallLights, -quarterDepth);
        wallLightRight1.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
        wallLightRight2.transform.localPosition = new Vector3(-halfWidth, heightWallLights, quarterDepth);
        wallLightRight2.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));

        ceilingLight.transform.localPosition = new Vector3(0, roomDimensions.y, 1f);
        
        human.transform.localPosition = new Vector3(-quarterWidth-0.5f, 0,quarterDepth);
        human.transform.localRotation = Quaternion.Euler(new Vector3(0, 137, 0));

        office.transform.localPosition = new Vector3(quarterWidth, 0, -quarterDepth);

        door.transform.localPosition = new Vector3(0, 0, -halfDepth);
    }

    #region placing Variants
    WallySettings PlaceByAmount()
    {
        Debug.Log("Construct wall by using given amount of tiles");
        float _xSize = (tileSize.x + spacing) * amountCubes.x;
        float _ySize = (tileSize.y + spacing) * amountCubes.y;
		tiles = new GameObject[(int)Mathf.Ceil(amountCubes.x), (int)Mathf.Ceil(amountCubes.y)];

		return new WallySettings((int)amountCubes.x, (int)amountCubes.y, tileSize.x, tileSize.y, tileSize.z, wallySize.x, wallySize.y);
    }

    WallySettings PlaceBySize()
    {
        Debug.Log("Fitting amount of tiles to given wally size and tile size");
        // How many cubes fit in the designated size?
        int _xAmount = (int)(wallySize.x / (tileSize.x + spacing) - spacing);
        int _yAmount = (int)(wallySize.y / (tileSize.y + spacing) - spacing);

		return new WallySettings(_xAmount, _yAmount, tileSize.x, tileSize.y, tileSize.z, wallySize.x, wallySize.y);
    }

    WallySettings PlaceBySizeAndAmount()
    {
		Debug.Log("Construct Wall by Size and Amount");
        float _tSizeX = ((wallySize.x - (amountCubes.x - 1) * spacing) / (amountCubes.x));
        float _tSizeY = ((wallySize.y - (amountCubes.y - 1) * spacing) / (amountCubes.y));

        tileSize.x = _tSizeX;
        tileSize.y = _tSizeY;

		return new WallySettings((int)amountCubes.x, (int)amountCubes.y, tileSize.x, tileSize.y, tileSize.z, wallySize.x, wallySize.y);
    }

	void CreateTiles(WallySettings ws) {
		// Prepare storage for references to each tile GameObject
		tiles = new GameObject[ws.TileCountX, ws.TileCountY];
		// Prepare storage for references to the TileScript of each tile
		tileScripts = new TileScript[ws.TileCountX, ws.TileCountY];
		// Set some common variables before the loop
		float offsetX = ws.TileSizeX * 0.5f;
		float offsetY = ws.TileSizeY * 0.5f;
		Vector3 tileSize = new Vector3(ws.TileSizeX, ws.TileSizeY, ws.TileSizeZ);
		// Create tiles in scene
		for (int x = 0; x < ws.TileCountX; x++) {
			for (int y = 0; y < ws.TileCountY; y++) {
				// Create GameObject from prefab while setting the correct parent
				GameObject _nTile = Instantiate(tilePrefab, wallyTransform);
				// Keep reference to the GameObject
				tiles[x, y] = _nTile;
				// Temporarily store a reference to the TileScript of this tile
				tileScripts[x, y] = _nTile.GetComponent<TileScript>();
				_nTile.transform.localPosition = new Vector3(offsetX + x * (ws.TileSizeX + spacing), offsetY + y * (ws.TileSizeY + spacing), 0);
				// Resize instance to proper tile dimensions
				_nTile.transform.localScale = tileSize;
			}
		}

		// Set up neighbour references for all tiles
		for (int x = 0; x < ws.TileCountX; x++) {
			for (int y = 0; y < ws.TileCountY; y++) {
				// Create temporary neighbour-list for current tile
				List<TileScript> _tN = new List<TileScript>();
				for (int y2 = y + 1; y2 >= y - 1; y2--) {
					for (int x2 = x - 1; x2 <= x + 1; x2++) {
						if (x2 >= 0 && x2 < ws.TileCountX && y2 >= 0 && y2 < ws.TileCountY) {
							// Avoid adding a tile to its own neighbour list
							if (x2 == x && y2 == y) {
								continue;
							}
							if (tiles[x2, y2] != null) {
								_tN.Add(tileScripts[x2, y2]);
							}
						}
					}
				}
				TileScript currentTile = tileScripts[x, y];
				currentTile.x = x;
				currentTile.y = y;
				currentTile.neighbours = _tN;
			}
		}
	}
    #endregion
}
