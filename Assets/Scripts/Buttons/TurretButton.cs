using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretButton : MonoBehaviour
{
    public GameObject building;
    public Sprite panelBG;
    public Font font;
    public Sprite powerIconSprite;
    public Sprite cubitsIconSprite;

    private BuildingSystem buildSystem;
    private PlaceableObject placeableObject;
    private GameObject infoPanel;
    private TurretScript turret;


    void Awake()
    {
        buildSystem = BuildingSystem.current;
    }

    public void InitializeBuilding()
    {
        buildSystem.DestroyObjectToPlace();
        buildSystem.InitializeWithObject(building);
    }

    public void CreateButton()
    {
        placeableObject = building.GetComponent<PlaceableObject>();
        turret = building.GetComponent<TurretScript>();

        //Create Icon
        GameObject icon = new GameObject();
        icon.name = "icon";
        icon.transform.parent = gameObject.transform;
        CanvasGroup iconCanv = icon.AddComponent<CanvasGroup>();
        iconCanv.blocksRaycasts = false;
        Image iconImg = icon.AddComponent<Image>();
        iconImg.sprite = placeableObject.icon;
        RectTransform iconImgTransform = icon.GetComponent<RectTransform>();
        iconImgTransform.localPosition = new Vector3(0f, 0f, 0f);
        iconImgTransform.sizeDelta = new Vector2(90f, 90f);

        CreateInfoPanel();
    }

    private void CreateInfoPanel()
    {
        //Create Background
        infoPanel = new GameObject();
        infoPanel.name = "Info Panel";
        infoPanel.transform.parent = transform;
        Image panel = infoPanel.AddComponent<Image>();
        RectTransform panelTransform = infoPanel.GetComponent<RectTransform>();
        panel.sprite = panelBG;

        //Size and translate backgorund
        panelTransform.localPosition = new Vector3(110f, 415f, 0f);
        panelTransform.sizeDelta = new Vector2(250f, 350f);
        infoPanel.SetActive(false);

        //Set turret info
        float health = placeableObject.health;
        float damage = turret.damage;
        float range = turret.range;
        float fireRate = turret.fireRate;
        bool canAttackGround = turret.canAttackGround;
        bool canAttackAir = turret.canAttackAir;
        Sprite iconSprite = placeableObject.icon;

        //Create title Text
        GameObject titleTxtObj = new GameObject();
        titleTxtObj.name = "Title";
        titleTxtObj.transform.parent = infoPanel.transform;
        Text titleTxt = titleTxtObj.AddComponent<Text>();
        RectTransform titleTxtTransform = titleTxtObj.GetComponent<RectTransform>();
        titleTxtTransform.localPosition = new Vector3(57f, 65f, 0f);

        titleTxt.font = font;
        titleTxt.fontSize = 28;
        titleTxt.alignment = TextAnchor.UpperCenter;
        titleTxt.text = building.name;
        titleTxtTransform.localPosition = new Vector3(0f, 100f, 0f);
        titleTxtTransform.sizeDelta = new Vector2(250f, 100f);

        //Create Icon
        GameObject icon = new GameObject();
        icon.name = "icon";
        icon.transform.parent = infoPanel.transform;
        Image iconImg = icon.AddComponent<Image>();
        iconImg.sprite = iconSprite;
        RectTransform iconImgTransform = icon.GetComponent<RectTransform>();
        iconImgTransform.localPosition = new Vector3(0f, 65f, 0f);
        iconImgTransform.sizeDelta = new Vector2(110f, 110f);

        //Create info text
        GameObject infoTxtObj = new GameObject();
        infoTxtObj.name = "Info Text";
        infoTxtObj.transform.parent = infoPanel.transform;
        Text infoTxt = infoTxtObj.AddComponent<Text>();
        RectTransform infoTxtTransform = infoTxtObj.GetComponent<RectTransform>();
        infoTxtTransform.anchorMin = new Vector2(0, 1);
        infoTxtTransform.anchorMax = new Vector2(0, 1);
        infoTxtTransform.sizeDelta = new Vector2(200f, 200f);
        infoTxtTransform.localPosition = new Vector3(0f, -65f, 0f);

        infoTxt.fontSize = 15;
        infoTxt.font = font;
        if (fireRate > 0)
        {
            fireRate = 1 / fireRate;
        }
        infoTxt.text = "Attacks:\n" +
            "   Ground:     " + canAttackGround + "\n" +
            "   Air:              " + canAttackAir + "\n" + 
            "Damage: " + damage + "\n" +
            "Fire Rate: " + fireRate + "/s\n" +
            "Health: " + health + "\n" +
            "Range: " + range + "m";

        //Show price
        //Create Power Icon
        GameObject powerIcon = new GameObject();
        powerIcon.name = "Power Icon";
        powerIcon.transform.parent = infoPanel.transform;
        Image powerIconImg = powerIcon.AddComponent<Image>();
        powerIconImg.sprite = powerIconSprite;
        RectTransform powerIconImgTransform = powerIcon.GetComponent<RectTransform>();
        powerIconImgTransform.localPosition = new Vector3(-85f, -130f, 0f);
        powerIconImgTransform.sizeDelta = new Vector2(40f, 40f);
        //Create Cubits Icon
        GameObject cubitsIcon = new GameObject();
        cubitsIcon.name = "Cubits Icon";
        cubitsIcon.transform.parent = infoPanel.transform;
        Image cubitsIconImg = cubitsIcon.AddComponent<Image>();
        cubitsIconImg.sprite = cubitsIconSprite;
        RectTransform cubitsIconImgTransform = cubitsIcon.GetComponent<RectTransform>();
        cubitsIconImgTransform.localPosition = new Vector3(5f, -130f, 0f);
        cubitsIconImgTransform.sizeDelta = new Vector2(40f, 40f);
        //Create power price text
        GameObject powerPriceTxtObj = new GameObject();
        powerPriceTxtObj.name = "powerPrice Text";
        powerPriceTxtObj.transform.parent = infoPanel.transform;
        Text powerPriceTxt = powerPriceTxtObj.AddComponent<Text>();
        RectTransform powerPriceTxtTransform = powerPriceTxtObj.GetComponent<RectTransform>();
        powerPriceTxtTransform.anchorMin = new Vector2(0, 1);
        powerPriceTxtTransform.anchorMax = new Vector2(0, 1);
        powerPriceTxtTransform.sizeDelta = new Vector2(50f, 50f);
        powerPriceTxtTransform.localPosition = new Vector3(-35f, -140f, 0f);
        powerPriceTxt.fontSize = 20;
        powerPriceTxt.font = font;
        powerPriceTxt.text = placeableObject.powerCost.ToString();
        //Create power price text
        GameObject cubitsPriceTxtObj = new GameObject();
        cubitsPriceTxtObj.name = "cubitsPrice Text";
        cubitsPriceTxtObj.transform.parent = infoPanel.transform;
        Text cubitsPriceTxt = cubitsPriceTxtObj.AddComponent<Text>();
        RectTransform cubitsPriceTxtTransform = cubitsPriceTxtObj.GetComponent<RectTransform>();
        cubitsPriceTxtTransform.anchorMin = new Vector2(0, 1);
        cubitsPriceTxtTransform.anchorMax = new Vector2(0, 1);
        cubitsPriceTxtTransform.sizeDelta = new Vector2(50f, 50f);
        cubitsPriceTxtTransform.localPosition = new Vector3(55f, -140f, 0f);
        cubitsPriceTxt.fontSize = 20;
        cubitsPriceTxt.font = font;
        cubitsPriceTxt.text = placeableObject.cubitsCost.ToString();
    }

    public void DisableInfo()
    {
        infoPanel.SetActive(false);
    }

    public void EnableInfo()
    {
        infoPanel.SetActive(true);
    }
}
