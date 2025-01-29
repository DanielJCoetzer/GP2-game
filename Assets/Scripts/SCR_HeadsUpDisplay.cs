using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_HeadsUpDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] public Image  staminaBar;
    [SerializeField] public GameObject pointReticle;
    [SerializeField] public GameObject shotgunReticle;
    [SerializeField] public GameObject scopeReticle;


    public static SCR_HeadsUpDisplay Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        //SCR_GameController.Instance.OnEquipRam += EnableReticle;
        //SCR_GameController.Instance.OnUnEquipRam += EnableReticle;
    }

    void EnableReticle(int id)
    {
        if (id != 1) return;

        pointReticle.gameObject.SetActive(true);
    }

    void Start()
    {
        UpdateStaminaBar(1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmmoCount(int currentAmmo, int maxAmmo) {
        ammoCount.text = $"Ammo: {maxAmmo - currentAmmo} / {maxAmmo}"; 
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if(!health) return;
        health.text = $"Health: {currentHealth} / {maxHealth}";
    }

    public void UpdateStaminaBar(float staminaFraction)
    {
        if(!staminaBar) return;

        staminaFraction = Mathf.Clamp01(staminaFraction);
        staminaBar.fillAmount = staminaFraction;
    }
}
