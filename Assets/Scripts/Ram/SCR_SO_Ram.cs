using UnityEngine;

public enum FloppyType {ESSENTIAL, MOD}

[CreateAssetMenu(fileName = "Ram", menuName = "Create Ram")]
public class SCR_SO_Ram : ScriptableObject
{
    public int Id;
    public FloppyType floppyType;

    public void EquipRam()
    {
        SCR_GameController.Instance.EquipRam(Id);
    }

    public void UnEquipRam()
    {
        SCR_GameController.Instance.UnEquipRam(Id);
    }
}
