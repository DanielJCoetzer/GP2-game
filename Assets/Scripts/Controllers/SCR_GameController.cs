using System;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GameController : MonoBehaviour
{
    public static SCR_GameController Instance;

    public GameObject PlayerPrefab, CurrentPlayer;

    public event Action<int> OnEquipRam, OnUnEquipRam;

    public List<SCR_SO_Ram> CurrentEquippedRam, BackPackRam, AllRamNotEquipped;

    private int currentFloor = 0;

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
        SetupGame();

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
            ProgressLevel();
    }

    public void SetupGame()
    {
        SCR_LevelController.Instance.CreateFloor();
        PlayerSetup();
    }

    public void ProgressLevel()
    {
        SCR_LevelController.Instance.DestroyCurrentLevel();
        Destroy(CurrentPlayer);
        CurrentPlayer = null;
        SetupGame();
        currentFloor++;
    }

    public void PlayerSetup()
    {
        SpawnPlayerInRoom(/*Room number*/);
        AddAllEquippedRamToPlayer();
        AddAllBuffsToPlayer();
        AddAllUpgradesToPlayer();

    }

    public void OpenRamHud()
    {
        //Pause Game
        //BackPackMenu.Instance.gameObject.setActive(false);
        //UpgradesMenu.Instance.gameObject.setActive(true);
    }

    public void OpenBackpackHud()
    {
        //Pause Game
        //BackPackMenu.Instance.gameObject.setActive(true);
        //UpgradesMenu.Instance.gameObject.setActive(false);
    }

    /// <summary>
    /// Provides the caller with a ram upgrade from the not equipped ram list
    /// </summary>
    /// <returns>A random ram upgrade</returns>
    public SCR_SO_Ram GetRandomNotEquippedRam()
    {
        return AllRamNotEquipped[UnityEngine.Random.Range(0, AllRamNotEquipped.Count - 1)];
    }

    private void AddAllEquippedRamToPlayer()
    {
        foreach (var ram in CurrentEquippedRam)
        {
            ram.EquipRam();
        }
    }

    private void AddAllUpgradesToPlayer()
    {
        return;
    }

    private void AddAllBuffsToPlayer()
    {
        return;
    }

    /// <summary>
    /// Adds a ram from the allRamNotEquipped list to the players backpack
    /// </summary>
    /// <param name="upgrade"></param>
    public void AddRamToBackpack(SCR_SO_Ram upgrade)
    {
        if(!AllRamNotEquipped.Contains(upgrade)) return;

        AllRamNotEquipped.Remove(upgrade);
        BackPackRam.Add(upgrade);
    }

    /// <summary>
    /// Equip a ram from the allRamNotEquipped list
    /// </summary>
    /// <param name="upgrade"></param>
    public void EquipNonPlayerRam(SCR_SO_Ram upgrade)
    {
        if (!AllRamNotEquipped.Contains(upgrade)) return;

        AllRamNotEquipped.Remove(upgrade);
        CurrentEquippedRam.Add(upgrade);
    }
    /// <summary>
    /// Takes an upgrade that the player has equipped and places it in their backpack
    /// </summary>
    /// <param name="upgrade"></param>
    public void UnequipEquippedRam(SCR_SO_Ram upgrade)
    {
        if (!CurrentEquippedRam.Contains(upgrade)) return;

        CurrentEquippedRam.Remove(upgrade);
        BackPackRam.Add(upgrade);
    }


    /// <summary>
    /// Spawns the player in the selected room from the map
    /// </summary>
    /// <param name="roomNumber">The room number that you want the player prefab to spawn in</param>
    public void SpawnPlayerInRoom(int roomNumber = 0)
    {
        //CurrentPlayer = Instantiate(PlayerPrefab, SCR_LevelController.Instance.PossiblePlayerSpawns[roomNumber], Quaternion.identity);
    }

    public void EquipRam(int ramId)
    {
        OnEquipRam?.Invoke(ramId);
    }

    public void UnEquipRam(int ramId)
    {
        OnUnEquipRam?.Invoke(ramId);
    }
}
