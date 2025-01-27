using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SCR_GameController : MonoBehaviour
{
    public static SCR_GameController Instance;

    public GameObject PlayerPrefab;

    public event Action<int> OnEquipRam, OnUnEquipRam;

    public List<SCR_SO_Ram> CurrentEquippedRam, AvailableRam, AllRam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //Runs the setup from the level controller
        SCR_LevelController.Instance.CreateFloor();
        SpawnPlayerInRoom();

        EquipAllCurrentlyEquippableRam();

        //Opens the upgrade menu and the map
    }

    private void Update()
    {
        
    }

    public void EquipAllCurrentlyEquippableRam()
    {
        foreach (var scrSoRam in CurrentEquippedRam)
        {
            scrSoRam.EquipRam();
        }
    }

    public void EquipAllEssentialRam()
    {
        foreach (var scrSoRam in CurrentEquippedRam.Where(scrSoRam => scrSoRam.floppyType == FloppyType.ESSENTIAL))
        {
            scrSoRam.EquipRam();
        }
    }

    public void UnEquipAllRam()
    {
        foreach (var scrSoRam in CurrentEquippedRam)
        {
            scrSoRam.UnEquipRam();
        }
    }



    /// <summary>
    /// Spawns the player in the selected room from the map
    /// </summary>
    /// <param name="roomNumber">The room number that you want the player prefab to spawn in</param>
    public void SpawnPlayerInRoom(int roomNumber = 0)
    {
        Instantiate(PlayerPrefab, SCR_LevelController.Instance.PossiblePlayerSpawns[roomNumber], Quaternion.identity);
    }

    public void EquipRam(int ramId)
    {
        OnEquipRam?.Invoke(ramId);
    }

    public void UnEquipRam(int ramId)
    {
        OnUnEquipRam?.Invoke(ramId);
    }

    public void DisplayUpgrades()
    {

    }
}
