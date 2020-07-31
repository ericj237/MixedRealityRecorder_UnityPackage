using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.XR;

public class TileManager : MonoBehaviour {

	public GameObject[,] Tiles { get; private set; }
	public TileScript[,] TileScripts { get; private set; }

	#region Private variable declarations
	// Reference to the input controllers
	private GameObject leftController;
	private GameObject rightController;
	#endregion

	#region Unity methods (Start, Update, ...)
	void Awake() {
        GameObject cMGO = GameObject.Find("Construction Manager");
        if (cMGO == null) {
            Debug.LogError("Construction manager could not be found. Aborting...");
            Destroy(this);
        }
		leftController = GameObject.Find("Controller (left)");
		rightController = GameObject.Find("Controller (right)");
	}

    void Update() {
    }
    #endregion

	public void Initialize(GameObject[,] tiles, TileScript[,] tileScripts) {
		if (Tiles != null || TileScripts != null) {
			Debug.LogError("Tile manager been initialized already. This may only be done once. Aborting...");
			return;
		}
		Tiles = tiles;
		TileScripts = tileScripts;
	}
}
