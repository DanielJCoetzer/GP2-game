using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class SCR_Equipped_Slot : MonoBehaviour
{
    public Button button;
    private SCR_SO_Ram mod;
    public SCR_Equipped_Slot(SCR_SO_Ram mod, VisualTreeAsset template) {

        TemplateContainer modButtonContainer = template.Instantiate();

        button = modButtonContainer.Q<Button>();
        this.mod = mod;

        button.text = mod.displayName + " " + mod.cost;
        
        button.RegisterCallback<ClickEvent>(OnClick);
    }

    public void OnClick(ClickEvent evt) {
        Debug.Log("Mod slot with name "+ mod.displayName + " has been clicked");
    }
}
