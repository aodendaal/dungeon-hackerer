using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GlobalManager : MonoBehaviour
{
    [Header("Generation Input")]
    [SerializeField]
    private InputField seedNumber;

    [Header("Panels")]
    [SerializeField]
    private GameObject seedPanel;
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject actionPanel;
    [SerializeField]
    private GameObject mapPanel;

    [Header("Player")]
    [SerializeField]
    private GameObject player;

    private DungeonGenerator generator;
    private MapRenderer mapRenderer;
    private Highscore highscoreManager;



    // Use this for initialization
    void Start()
    {
        generator = gameObject.GetComponent<DungeonGenerator>();
        mapRenderer = gameObject.GetComponent<MapRenderer>();
        highscoreManager = gameObject.GetComponent<Highscore>();


        seedPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        actionPanel.SetActive(false);
        mapPanel.SetActive(false);

        mapRenderer.enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void StartRun_Click()
    {
        generator.Generate(int.Parse(seedNumber.text));
        mapRenderer.UpdateMap(0);
        MovePlayerToStart();

        seedPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        actionPanel.SetActive(true);

        player.GetComponent<PlayerMovement>().enabled = true;
        mapRenderer.enabled = true;
    }

    public void ChangeLevel()
    {
        var level = ++DungeonData.currentLevel;

        Debug.Log("new level: " + level.ToString());

        generator.Clear();
        generator.InstantiateMap(level);

        MovePlayerToStart();

        mapRenderer.Clear();
        mapRenderer.UpdateMap(level);
        Debug.Log("Level Successfully changed");
    }

    private void MovePlayerToStart()
    {
        var position = DungeonData.GetEntrancePosition(DungeonData.currentLevel);
        var direction = DungeonData.GetEntranceDirection(DungeonData.currentLevel);
        player.GetComponent<PlayerMovement>().SetNewPosition(position + new Vector3(0f, 2.5f, 0f), direction);
        
    }

    public void Randomize_Click()
    {
        var number = Random.Range(111111, 1000000);
        seedNumber.text = number.ToString();
    }
}
