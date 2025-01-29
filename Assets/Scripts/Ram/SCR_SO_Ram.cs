using UnityEngine;

public enum FloppyType {ESSENTIAL, MOD}

[CreateAssetMenu(fileName = "Ram", menuName = "Create Ram")]
public class SCR_SO_Ram : ScriptableObject
{
    public int Id, cost;
    public FloppyType floppyType;
    public string displayName;
    public Sprite Icon;

    [TextArea(5, 20)]
    public string Description = "";

    public void EquipRam()
    {
        SCR_GameController.Instance.EquipRam(Id);
    }

    public void UnEquipRam()
    {
        SCR_GameController.Instance.UnEquipRam(Id);
    }
}
