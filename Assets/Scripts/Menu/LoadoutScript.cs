using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutScript : MonoBehaviour
{
    public GameObject spawn;
    public GameObject turret;
    public GameObject utility;

    public Dropdown spawnDropdown;
    public Dropdown turretDropdown;
    public Dropdown utilityDropdown;

    public GameObject[] spawnButtons;
    public GameObject[] turretButtons;
    public GameObject[] utilityButtons;

    public GameObject buildingPoint;

    public Sprite active;
    public Sprite inactive;

    public GameObject[] spawnBuildings;
    public GameObject[] turretBuildings;
    public GameObject[] utilityBuildings;

    public Text titleText;
    public Image troopIcon;
    public Text infoText;
    public Text energyText;
    public Text cubitsText;


    private enum State { Spawn, Turrets, Utility};
    private State _state;

    private GameObject currentPane;
    private GameObject currentButton;
    private GameObject currentBuilding;
    
    // Start is called before the first frame update
    void Start()
    {
        _state = State.Spawn;
        currentPane = spawn;
        InitializeButtons();
        currentButton = spawnButtons[0];
        UpdateInfo(spawnBuildings[0]);
    }


    void OnDisable()
    {
        if(currentBuilding != null)
        {
            currentBuilding.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (currentBuilding != null)
        {
            currentBuilding.SetActive(true);
        }
    }

    private void InitializeButtons()
    {
        for (int i = 0; i < spawnButtons.Length; i++)
        {
            spawnButtons[i].GetComponent<MenuBuildingButton>().building = PlayerPrefs.GetInt("SpawnBuilding" + i);
            spawnButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = spawnBuildings[PlayerPrefs.GetInt("SpawnBuilding" + i)].GetComponent<PlaceableObject>().icon;
        }
        for (int i = 0; i < turretButtons.Length; i++)
        {
            turretButtons[i].GetComponent<MenuBuildingButton>().building = PlayerPrefs.GetInt("TurretBuilding" + i);
            turretButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = turretBuildings[PlayerPrefs.GetInt("TurretBuilding" + i)].GetComponent<PlaceableObject>().icon;
        }
        for (int i = 0; i < utilityButtons.Length; i++)
        {
            utilityButtons[i].GetComponent<MenuBuildingButton>().building = PlayerPrefs.GetInt("UtilityBuilding" + i);
            utilityButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = utilityBuildings[PlayerPrefs.GetInt("UtilityBuilding" + i)].GetComponent<PlaceableObject>().icon;
        }
    }

    public void SetButton(GameObject button)
    {
        currentButton.GetComponent<Image>().sprite = inactive;
        currentButton = button;
        currentButton.GetComponent<Image>().sprite = active;
        switch (_state)
        {
            case State.Spawn:
                UpdateInfo(spawnBuildings[currentButton.GetComponent<MenuBuildingButton>().building]);
                spawnDropdown.value = currentButton.GetComponent<MenuBuildingButton>().building;
                break;
            case State.Turrets:
                UpdateInfo(turretBuildings[currentButton.GetComponent<MenuBuildingButton>().building]);
                turretDropdown.value = currentButton.GetComponent<MenuBuildingButton>().building;
                break;
            default:
                UpdateInfo(utilityBuildings[currentButton.GetComponent<MenuBuildingButton>().building]);
                utilityDropdown.value = currentButton.GetComponent<MenuBuildingButton>().building;
                break;
        }
    }

    public void SetSpawn()
    {
        _state = State.Spawn;
        currentPane.SetActive(false);
        spawn.SetActive(true);
        currentPane = spawn;
        SetButton(spawnButtons[0]);
        UpdateInfo(spawnBuildings[spawnButtons[0].GetComponent<MenuBuildingButton>().building]);
    }

    public void SetTurret()
    {
        _state = State.Turrets;
        currentPane.SetActive(false);
        turret.SetActive(true);
        currentPane = turret;
        SetButton(turretButtons[0]);
        UpdateInfo(turretBuildings[turretButtons[0].GetComponent<MenuBuildingButton>().building]);
    }
    public void SetUtility()
    {
        _state = State.Utility;
        currentPane.SetActive(false);
        utility.SetActive(true);
        currentPane = utility;
        SetButton(utilityButtons[0]);
        UpdateInfo(utilityBuildings[utilityButtons[0].GetComponent<MenuBuildingButton>().building]);
    }

    public void UpdateBuilding()
    {
        GameObject selectedBuilding;
        
        switch (_state)
        {
            case State.Spawn:
                selectedBuilding = spawnBuildings[spawnDropdown.value];
                currentButton.GetComponent<MenuBuildingButton>().building = spawnDropdown.value;
                UpdateSpawns();
                break;
            case State.Turrets:
                selectedBuilding = turretBuildings[turretDropdown.value];
                currentButton.GetComponent<MenuBuildingButton>().building = turretDropdown.value;
                UpdateTurrets();
                break;
            default:
                selectedBuilding = utilityBuildings[utilityDropdown.value];
                currentButton.GetComponent<MenuBuildingButton>().building = utilityDropdown.value;
                UpdateUtilities();
                break;
        }

        currentButton.transform.GetChild(0).GetComponent<Image>().sprite = selectedBuilding.GetComponent<PlaceableObject>().icon;

        UpdateInfo(selectedBuilding);
    }

    public void UpdateSpawns()
    {
        for(int i = 0;  i < spawnButtons.Length; i++) {
            PlayerPrefs.SetInt("SpawnBuilding" + i, spawnButtons[i].GetComponent<MenuBuildingButton>().building);
        }
    }

    public void UpdateTurrets()
    {
        for (int i = 0; i < turretButtons.Length; i++)
        {
            PlayerPrefs.SetInt("TurretBuilding" + i, turretButtons[i].GetComponent<MenuBuildingButton>().building);
        }
    }

    public void UpdateUtilities()
    {
        for (int i = 0; i < utilityButtons.Length; i++)
        {
            PlayerPrefs.SetInt("UtilityBuilding" + i, utilityButtons[i].GetComponent<MenuBuildingButton>().building);
        }
    }

    public void UpdateInfo(GameObject building)
    {
        PlaceableObject placeableObject = building.GetComponent<PlaceableObject>();

        int damage;
        float speed;
        float range;
        float health;
        float fireRate;
        bool canAttackGround;
        bool canAttackAir;
        bool canAttackBuildings;

        switch (_state)
        {
            case State.Spawn:
                ISpawnable troopInfo = building.GetComponent<SpawnBuildingScript>().troop.GetComponent<ISpawnable>();
                troopIcon.color = Color.white;
                troopIcon.sprite = troopInfo.GetIcon();

                titleText.text = building.GetComponent<SpawnBuildingScript>().troop.name;

                //Set troop info
                bool isFlying = troopInfo.GetIsFlying();
                damage = troopInfo.GetDamage();
                speed = troopInfo.GetSpeed();
                range = troopInfo.GetRange();
                health = troopInfo.GetHealth();
                fireRate = troopInfo.GetFireRate();
                canAttackGround = troopInfo.GetCanAttackGround();
                canAttackAir = troopInfo.GetCanAttackAir();
                canAttackBuildings = troopInfo.GetCanAttackBuilding();
                if (fireRate > 0)
                {
                    fireRate = 1 / fireRate;
                }

                string type;
                if (isFlying)
                {
                    type = "Flying";
                } else
                {
                    type = "Ground";
                }

                infoText.text = "Type: " + type + "\n" + 
                    "Attacks:\n" +
                    "   Ground:   " + canAttackGround + "\n" +
                    "   Air:           " + canAttackAir + "\n" +
                    "   Buildings: " + canAttackBuildings + "\n" +
                    "Damage: " + damage + "\n" +
                    "Fire Rate: " + fireRate + "/s\n" +
                    "Health: " + health + "\n" +
                    "Range: " + range + "m\n" +
                    "Speed: " + speed + "\n" +
                    placeableObject.info;
                break;
            case State.Turrets:
                troopIcon.color = Color.clear;

                TurretScript turret = building.GetComponent<TurretScript>();

                titleText.text = building.name;

                //Set turret info
                health = placeableObject.health;
                damage = turret.damage;
                range = turret.range;
                fireRate = turret.fireRate;
                canAttackGround = turret.canAttackGround;
                canAttackAir = turret.canAttackAir;
                if (fireRate > 0)
                {
                    fireRate = 1 / fireRate;
                }

                infoText.text = "Attacks:\n" +
                    "   Ground:     " + canAttackGround + "\n" +
                    "   Air:             " + canAttackAir + "\n" +
                    "Damage: " + damage + "\n" +
                    "Fire Rate: " + fireRate + "/s\n" +
                    "Health: " + health + "\n" +
                    "Range: " + range + "m\n\n" +
                    placeableObject.info;
                break;
            default:
                troopIcon.color = Color.clear;

                titleText.text = building.name;

                infoText.text = placeableObject.info;
                break;
        }

        energyText.text = placeableObject.powerCost.ToString();
        cubitsText.text = placeableObject.cubitsCost.ToString();

        SetBuilding(building);
    }

    private void SetBuilding(GameObject building)
    {
        if(currentBuilding != null)
        {
            Destroy(currentBuilding);
        }
        currentBuilding = Instantiate(building, buildingPoint.transform);
        if(currentBuilding.TryGetComponent(out PlaceableObject po))
        {
            po.enabled = false;
        }
        if (currentBuilding.TryGetComponent(out TurretScript ts))
        {
            ts.enabled = false;
        }
        if (currentBuilding.TryGetComponent(out UtilityInterface ui))
        {
            ui.DisableUtility();
        }
    }
}
