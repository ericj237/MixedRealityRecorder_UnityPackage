using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TileScript : MonoBehaviour
{
	[System.NonSerialized] // Don't store in scene (thus don't show in inspector either)
	public int x, y;
	[System.NonSerialized]
	public List<TileScript> neighbours;

	#region Inspector configurable variables
	public bool actionsActiveByDefault = true; // Change in Inspector

	[Header("Motor Physics")]
	[Tooltip("Only change here on prefabs, not on single instances. Can be toggled at runtime by pressing p")]
	public bool useMotorPhysics = true;
	[Tooltip("How often a tile can drive the distance of its full extent per second. E.g.: A value of 4 means it can go from 0 to full in quarter a second.")]
	[Range(0f, 100f)]
	public float maxSpeed = 1f;
	#endregion

	public float TargetPercentage {get; private set;}

    #region Private variable declarations
    // Reference to the moving part of this tile
    private Transform extendableArm;
	// List of action scripts
	List<IAction> actionScripts;

	// Temporarily store percentage results from actions
	List<float> percentages;

    // Used to toggle actions on or off
    private bool[] actionActive = new bool[10];

	// Physics helpers
	private float lastFramePercentage;
	#endregion

	#region Unity methods (Start, Update, ...)
	void Awake() {
        extendableArm = transform.Find("Extendable Arm");
		// Grab all action scripts on this tile
		MonoBehaviour[] list = transform.GetComponents<MonoBehaviour>();
		actionScripts = new List<IAction>();
		percentages = new List<float>();
		foreach (MonoBehaviour mb in list) {
			if (mb is IAction) {
				actionScripts.Add((IAction)((System.Object)mb));
			}
		}
		// Activate all action scripts by default (if actionsActiveByDefault is true)
		for (int i = 0; i <= 9; i++) {
            actionActive[i] = actionsActiveByDefault;
        }
	}

    void Update() {
        HandleInput();
		HandleActions();
        DriveExtendableArm();
    }
	#endregion

    private void HandleInput() {
        if (Input.GetKeyDown("1")) {
            ToggleInput(1);
        }
        if (Input.GetKeyDown("2")) {
            ToggleInput(2);
        }
        if (Input.GetKeyDown("3")) {
            ToggleInput(3);
        }
        if (Input.GetKeyDown("4")) {
            ToggleInput(4);
        }
        if (Input.GetKeyDown("5")) {
            ToggleInput(5);
        }
        if (Input.GetKeyDown("6")) {
            ToggleInput(6);
        }
        if (Input.GetKeyDown("7")) {
            ToggleInput(7);
        }
        if (Input.GetKeyDown("8")) {
            ToggleInput(8);
        }
        if (Input.GetKeyDown("9")) {
            ToggleInput(9);
        }
        if (Input.GetKeyDown("0")) {
            ToggleInput(0);
        }
		if (Input.GetKeyDown("p")) {
			useMotorPhysics = !useMotorPhysics;
		}
    }

    private void ToggleInput(int i) {
        actionActive[i] = !actionActive[i];
    }

	// Iterate over action scripts 
	private void HandleActions() {
		// Retrieve percentages of all actions
		percentages.Clear();
		foreach (IAction action in actionScripts) {
			MonoBehaviour mb = action as MonoBehaviour;
            int actionIndex = actionScripts.IndexOf(action);
            if (mb != null && mb.isActiveAndEnabled && (actionIndex > 9 || actionActive[actionIndex])) {
				percentages.Add(action.GetPercentage());
			}
		}
		// Find maximum value
		float result = 0;
		foreach (float p in percentages) {
			result = Mathf.Max(result, p);
		}
		// Set value for this tile
		SetTargetPercentage(result);
	}

	// Returns distances between a point and this tile for each axis (local to this tile)
	// e.g.: z-axis is the pure distance in front of the wall
	//		 z = 1 means the shortest distance to the wall surface is 1
	public Vector3 GetDistanceFromTile(Vector3 point) {
		// Transform point to local space so we can easily read distances for each axis
		Vector3 p = transform.InverseTransformPoint(point);
		// Undo transformation by tile scale
		p.Scale(transform.localScale);
		//if (x == 0 && y == 0)
		//	Debug.Log($"Distance from wall is {p.z} | x offset is {p.x} | y offset is {p.y}");
		return p;
	}

	// Set percentage by which to protract the extendable arm
	// Resulting amounts are limited to reflect physical constraints
	private void SetTargetPercentage(float percentage) {
        percentage = Mathf.Clamp(percentage, 0, 1);
		TargetPercentage = percentage;
    }

	// Smoothly drives the extendable arm to current target percentage
	private void DriveExtendableArm() {
		float newPercentage = TargetPercentage;

		// Limit movement speed and acceleration to reflect physical constraints
		if (useMotorPhysics) {
			float maxDistThisFrame = maxSpeed * Time.deltaTime;
			float diff = newPercentage - lastFramePercentage;
			diff = Mathf.Clamp(diff, -maxDistThisFrame, maxDistThisFrame);
			newPercentage = lastFramePercentage + diff;
			lastFramePercentage = newPercentage;
		}
		// Move the arm between -0.5 and 0.5 on the z-Axis
		extendableArm.transform.localPosition = Vector3.forward * (newPercentage - 0.5f);
	}
}
