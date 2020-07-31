using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class RoomSettingsController : MonoBehaviour
{
    public ConstructionManager constructionManager;
    public MovePlayArea playArea;
    [Space(5)]
    public GameObject panel;
    [Header("UI Elements")]
    public InputField roomDimensionX;
    public InputField roomDimensionY;
    public InputField roomDimensionZ;

    public InputField wallySizeX;
    public InputField wallySizeY;

    public Dropdown priority;

    public InputField amountCubesX;
    public InputField amountCubesY;

    public InputField tileSizeX;
    public InputField tileSizeY;
    public InputField tileSizeZ;

    public InputField spacing;

    public InputField areaRotationY;

    public InputField areaPositionX;
    public InputField areaPositionY;
    public InputField areaPositionZ;
    void ShowPanel(bool _enabled)
    {
        if (_enabled == false)
            SetRoom();
        else
            UpdateUI();

        panel.SetActive(_enabled);
    }

    void SetRoom()
    {
        UpdateRoomDimension();
        UpdateWallySize();
        UpdateAmountCubes();
        UpdateTileSize();
        UpdateSpacing();
        UpdatePriority();

        constructionManager.ConstructRoom();
    }

    #region UIInput

    public void UpdateRoomDimension()
    {
        constructionManager.roomDimensions = new Vector3(
            float.Parse(roomDimensionX.text, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(roomDimensionY.text, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(roomDimensionZ.text, CultureInfo.InvariantCulture.NumberFormat)
            );
    }

    public void UpdateWallySize()
    {
        constructionManager.wallySize = new Vector2(
            float.Parse(wallySizeX.text, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(wallySizeY.text, CultureInfo.InvariantCulture.NumberFormat)
            );
    }
    public void UpdatePriority()
    {
        constructionManager.priority =(ConstructionManager.PlacingPriority) priority.value;
    }

    public void UpdateAmountCubes()
    {
        constructionManager.amountCubes = new Vector2(
            float.Parse(amountCubesX.text, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(amountCubesY.text, CultureInfo.InvariantCulture.NumberFormat)
            );
    }

    public void UpdateTileSize()
    {
        constructionManager.tileSize = new Vector3(
            float.Parse(tileSizeX.text, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(tileSizeY.text, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(tileSizeZ.text, CultureInfo.InvariantCulture.NumberFormat)
            );
    }

    public void UpdateSpacing()
    {
        constructionManager.spacing = float.Parse(spacing.text, CultureInfo.InvariantCulture.NumberFormat);
    }


    public void UpdateRotation()
    {
        playArea.SetRotation(Quaternion.Euler(
            0,
            float.Parse(areaRotationY.text, CultureInfo.InvariantCulture.NumberFormat),
            0
            ));
    }

    public void UpdatePosition()
    {
        playArea.SetPosition(new Vector3(
            float.Parse(areaPositionX.text, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(areaPositionY.text, CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(areaPositionZ.text, CultureInfo.InvariantCulture.NumberFormat)
            ));
    }



    #endregion

    #region OutputUI

    void UpdateUI()
    {
      roomDimensionX.text = constructionManager.roomDimensions.x.ToString();
      roomDimensionY.text = constructionManager.roomDimensions.y.ToString();
      roomDimensionZ.text = constructionManager.roomDimensions.z.ToString(); 

      wallySizeX.text = constructionManager.wallySize.x.ToString();
      wallySizeY.text = constructionManager.wallySize.y.ToString();

        priority.value = (int)constructionManager.priority;

      amountCubesX.text = constructionManager.amountCubes.x.ToString();
      amountCubesY.text = constructionManager.amountCubes.y.ToString();

      tileSizeX.text = constructionManager.tileSize.x.ToString();
      tileSizeY.text = constructionManager.tileSize.y.ToString();
      tileSizeZ.text = constructionManager.tileSize.z.ToString();

      spacing.text = constructionManager.spacing.ToString();

        areaRotationY.text = playArea.transform.rotation.eulerAngles.y.ToString();

        areaPositionX.text = playArea.transform.position.x.ToString();
        areaPositionY.text = playArea.transform.position.y.ToString();
        areaPositionZ.text = playArea.transform.position.z.ToString();
}

    public void UpdatePlayArea()
    {


        areaRotationY.text = playArea.transform.rotation.eulerAngles.y.ToString();

        areaPositionX.text = playArea.transform.position.x.ToString();
        areaPositionY.text = playArea.transform.position.y.ToString();
        areaPositionZ.text = playArea.transform.position.z.ToString();
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ShowPanel(panel.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            ShowPanel(!panel.activeSelf);
    }
}
