using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button SpawnBuildingButton;
    public Button TurretBuildingButton;
    public Button UtilityBuildingButton;

    private BuildingSystem buildSystem;
    private List<Button> spawnButtons;
    private List<Button> turretButtons;
    private List<Button> utilityButtons;
    private bool spawnActive;
    private bool turretActive;
    private bool utilityActive;

    private void Awake()
    {
        //buildSystem = BuildingSystem.current;
    }


    // Start is called before the first frame update
    void Start()
    {
        buildSystem = BuildingSystem.current;
        spawnButtons = new List<Button>();
        turretButtons = new List<Button>();
        utilityButtons = new List<Button>();
        InitializeSpawnButtons();
        InitializeTurretButtons();
        InitializeUtilityButtons();
    }

    private void InitializeSpawnButtons()
    {
        GameObject[] buildings = buildSystem.spawnBuildings;
        for(int i = 0; i < 5; i++)
        {
            int idx = PlayerPrefs.GetInt("SpawnBuilding" + i);
            Button newButt = Instantiate(SpawnBuildingButton);
            spawnButtons.Add(newButt);
            newButt.GetComponent<RectTransform>().SetParent(transform, false);
            newButt.GetComponent<SpawnButton>().building = buildings[idx];
            newButt.transform.Translate(new Vector3(125f * i, 0, 0));
            newButt.GetComponent<SpawnButton>().CreateButton();
            newButt.gameObject.SetActive(false);
        }
    }
    private void InitializeTurretButtons()
    {
        GameObject[] buildings = buildSystem.turrets;
        for (int i = 0; i < 4; i++)
        {
            int idx = PlayerPrefs.GetInt("TurretBuilding" + i);
            Button newButt = Instantiate(TurretBuildingButton);
            turretButtons.Add(newButt);
            newButt.GetComponent<RectTransform>().SetParent(transform, false);
            newButt.GetComponent<TurretButton>().building = buildings[idx];
            newButt.transform.Translate(new Vector3(125f * i, 0, 0));
            newButt.GetComponent<TurretButton>().CreateButton();
            newButt.gameObject.SetActive(false);
        }
    }

    private void InitializeUtilityButtons()
    {
        GameObject[] buildings = buildSystem.utilities;
        for (int i = 0; i < 4; i++)
        {
            int idx = PlayerPrefs.GetInt("UtilityBuilding" + i);
            Button newButt = Instantiate(UtilityBuildingButton);
            utilityButtons.Add(newButt);
            newButt.GetComponent<RectTransform>().SetParent(transform, false);
            newButt.GetComponent<Utility>().building = buildings[idx];
            newButt.transform.Translate(new Vector3(125f * i, 0, 0));
            newButt.GetComponent<Utility>().CreateButton();
            newButt.gameObject.SetActive(false);
        }
    }

    public void ActivateSpawnButtons()
    {
        if (spawnActive)
        {
            DeactivateSpawnButtons();
            spawnActive = false;
            return;
        }
        if (turretActive)
        {
            DeactivateTurretButtons();
            turretActive = false;
        }
        if (utilityActive)
        {
            DeactivateUtilityButtons();
            utilityActive = false;
        }

        int i = 0;
        while (i < spawnButtons.Count)
        {
            spawnButtons[i++].gameObject.SetActive(true);
        }

        spawnActive = true;
    }

    public void ActivateTurretButtons()
    {
        if (turretActive)
        {
            DeactivateTurretButtons();
            turretActive = false;
            return;
        }
        if (spawnActive)
        {
            DeactivateSpawnButtons();
            spawnActive = false;
        }
        if (utilityActive)
        {
            DeactivateUtilityButtons();
            utilityActive = false;
        }

        int i = 0;
        while (i < turretButtons.Count)
        {
            turretButtons[i++].gameObject.SetActive(true);
        }

        turretActive = true;
    }

    public void ActivateUtilityButtons()
    {
        if (utilityActive)
        {
            DeactivateUtilityButtons();
            utilityActive = false;
            return;
        }
        if (spawnActive)
        {
            DeactivateSpawnButtons();
            spawnActive = false;
        }
        if (turretActive)
        {
            DeactivateTurretButtons();
            turretActive = false;
        }

        int i = 0;
        while (i < utilityButtons.Count)
        {
            utilityButtons[i++].gameObject.SetActive(true);
        }

        utilityActive = true;
    }

    private void DeactivateSpawnButtons()
    {
        int i = 0;
        while (i < spawnButtons.Count)
        {
            spawnButtons[i++].gameObject.SetActive(false);
        }
    }

    private void DeactivateTurretButtons()
    {
        int i = 0;
        while (i < turretButtons.Count)
        {
            turretButtons[i++].gameObject.SetActive(false);
        }
    }

    private void DeactivateUtilityButtons()
    {
        int i = 0;
        while (i < utilityButtons.Count)
        {
            utilityButtons[i++].gameObject.SetActive(false);
        }
    }
}
