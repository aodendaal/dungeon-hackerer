using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandFight : MonoBehaviour {

    [Header("Player")]
    [SerializeField]
    private GameObject player;

    [Header("Icons")]
    [SerializeField]
    private Sprite emptyHandSprite;
    [SerializeField]
    private Sprite swishSprite;
    [SerializeField]
    private Sprite shieldSprite;

    [Header("Slot")]
    [SerializeField]
    private GameObject handSlot;

    float disableTime;
    float disableRate = 1.0f;

    private Image image;
    private Button button;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        var slot = handSlot.GetComponent<DragSlot>();

        slot.OnItemDrop += HandFight_OnItemDrop;
        slot.OnItemRemoved += HandFight_OnItemRemoved;
        
	}

    private void HandFight_OnItemRemoved(object sender, System.EventArgs e)
    {
        image.sprite = emptyHandSprite;
    }

    private void HandFight_OnItemDrop(object sender, System.EventArgs e)
    {
        var item = handSlot.transform.GetChild(0).gameObject.GetComponent<ItemDetails>();

        if (item.HandHeldType == HandHeldTypes.Slashing)
        {
            image.sprite = swishSprite;
        }
        else if (item.HandHeldType == HandHeldTypes.Shield)
        {
            image.sprite = shieldSprite;
        }
    }
    

    // Update is called once per frame
    void Update () {
        if (!button.interactable)
        {
            if (Time.time >= disableTime)
            {
                button.interactable = true;
            }
        }
	}

    public void Button_Click()
    {
        var ray = new Ray(player.transform.position, player.transform.forward);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 5f, 1 << 8))
        {
            if (hitInfo.transform.tag == "Monster")
            {
                AttackMonster(hitInfo.transform.gameObject);
            }
        }

        button.interactable = false;
        disableTime = Time.time + disableRate;
    }

    private void AttackMonster(GameObject monster)
    {
        var mon = monster.GetComponent<Monster>();
        mon.GetHit();
    }
}
