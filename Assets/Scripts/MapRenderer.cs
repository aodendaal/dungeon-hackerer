using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRenderer : MonoBehaviour
{

    [SerializeField]
    private GameObject mapPanel;
    [SerializeField]
    private GameObject mapContainer;
    [SerializeField]
    private GameObject mapImage;
    [SerializeField]
    private GameObject playerImage;

    #region Sprites

    [Header("1-Exit Building Blocks")]
    [SerializeField]
    private Sprite upSprite;

    [SerializeField]
    private Sprite downSprite;

    [SerializeField]
    private Sprite leftSprite;

    [SerializeField]
    private Sprite rightSprite;

    [SerializeField]
    private Sprite entranceUpSprite;

    [SerializeField]
    private Sprite entranceDownSprite;

    [SerializeField]
    private Sprite entranceLeftSprite;

    [SerializeField]
    private Sprite entranceRightSprite;

    [SerializeField]
    private Sprite exitUpSprite;

    [SerializeField]
    private Sprite exitDownSprite;

    [SerializeField]
    private Sprite exitLeftSprite;

    [SerializeField]
    private Sprite exitRightSprite;

    [Header("2-Building Blocks")]
    [SerializeField]
    private Sprite upLeftSprite;

    [SerializeField]
    private Sprite upRightSprite;

    [SerializeField]
    private Sprite upDownSprite;

    [SerializeField]
    private Sprite downLeftSprite;

    [SerializeField]
    private Sprite downRightSprite;

    [SerializeField]
    private Sprite leftRightSprite;

    [Header("3-Exit Building Blocks")]
    [SerializeField]
    private Sprite upDownRightSprite;

    [SerializeField]
    private Sprite upDownLeftSprite;

    [SerializeField]
    private Sprite upLeftRightSprite;

    [SerializeField]
    private Sprite downLeftRightSprite;

    [Header("4-Exit Building Blocks")]
    [SerializeField]
    private Sprite upDownLeftRightSprite;

    #endregion

    // Use this for initialization
    void Start()
    {
        mapPanel.SetActive(false);
        DungeonData.OnPlayerMoved += DungeonData_OnPlayerMoved;
    }

    private void DungeonData_OnPlayerMoved(object sender, System.EventArgs e)
    {
        GetPlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            mapPanel.SetActive(!mapPanel.activeSelf);
            GetPlayerPosition();
        }
    }

    public void Clear()
    {

        for (var i = 0; i < mapContainer.transform.childCount; i++)
        {
            Destroy(mapContainer.transform.GetChild(i).gameObject);
        }
    }

    public void UpdateMap(int level)
    {
        var x = 0;
        var y = 0;
        for (var tile = 0; tile < DungeonData.gridSize * DungeonData.gridSize; tile++)
        {
            var go = Instantiate(mapImage, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(mapContainer.transform);
            go.name = string.Format("MapImage ({0}, {1})", x, y);

            x++;
            if (x == DungeonData.gridSize)
            {
                x = 0;
                y++;
            }

            var image = go.GetComponent<Image>();

            var dataCell = DungeonData.DataMap[level][tile];

            if (dataCell.TileType == 1 && !dataCell.IsEntrance && !dataCell.IsExit) image.sprite = upSprite;
            else if (dataCell.TileType == 1 && dataCell.IsEntrance && !dataCell.IsExit) image.sprite = entranceUpSprite;
            else if (dataCell.TileType == 1 && !dataCell.IsEntrance && dataCell.IsExit) image.sprite = exitUpSprite;

            else if (dataCell.TileType == 2 && !dataCell.IsEntrance && !dataCell.IsExit) image.sprite = downSprite;
            else if (dataCell.TileType == 2 && dataCell.IsEntrance && !dataCell.IsExit) image.sprite = entranceDownSprite;
            else if (dataCell.TileType == 2 && !dataCell.IsEntrance && dataCell.IsExit) image.sprite = exitDownSprite;

            else if (dataCell.TileType == 4 && !dataCell.IsEntrance && !dataCell.IsExit) image.sprite = leftSprite;
            else if (dataCell.TileType == 4 && dataCell.IsEntrance && !dataCell.IsExit) image.sprite = entranceLeftSprite;
            else if (dataCell.TileType == 4 && !dataCell.IsEntrance && dataCell.IsExit) image.sprite = exitLeftSprite;

            else if (dataCell.TileType == 8 && !dataCell.IsEntrance && !dataCell.IsExit) image.sprite = rightSprite;
            else if (dataCell.TileType == 8 && dataCell.IsEntrance && !dataCell.IsExit) image.sprite = entranceRightSprite;
            else if (dataCell.TileType == 8 && !dataCell.IsEntrance && dataCell.IsExit) image.sprite = exitRightSprite;

            else if (dataCell.TileType == 5) image.sprite = upLeftSprite;
            else if (dataCell.TileType == 9) image.sprite = upRightSprite;
            else if (dataCell.TileType == 3) image.sprite = upDownSprite;
            else if (dataCell.TileType == 6) image.sprite = downLeftSprite;
            else if (dataCell.TileType == 10) image.sprite = downRightSprite;
            else if (dataCell.TileType == 12) image.sprite = leftRightSprite;

            else if (dataCell.TileType == 11) image.sprite = upDownRightSprite;
            else if (dataCell.TileType == 7) image.sprite = upDownLeftSprite;
            else if (dataCell.TileType == 13) image.sprite = upLeftRightSprite;
            else if (dataCell.TileType == 14) image.sprite = downLeftRightSprite;

            else if (dataCell.TileType == 15) image.sprite = upDownLeftRightSprite;

        }

        GetPlayerPosition();
    }

    private void GetPlayerPosition()
    {

        var rect = playerImage.GetComponent<RectTransform>();

        var playerPosition = DungeonData.PlayerPosition;
        var position = DungeonData.ConvertToMapCoord(playerPosition);

        //Debug.Log(string.Format("Player ({2}, {3}) to Map ({0}, {1})", position.x, position.y, playerPosition.x, playerPosition.z));

        position.x = 50.0f + (position.x - 4.0f) * 100.0f;
        position.y = 50.0f + (position.y - 4.0f) * 100.0f;

        rect.anchoredPosition = position;
    }
}
