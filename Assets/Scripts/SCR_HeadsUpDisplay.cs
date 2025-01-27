using TMPro;
using UnityEngine;

public class SCR_HeadsUpDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] public GameObject pointReticle;
    [SerializeField] public GameObject shotgunReticle;


    public static SCR_HeadsUpDisplay Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmmoCount(int currentAmmo, int maxAmmo) {
        ammoCount.text = $"Ammo: {maxAmmo - currentAmmo} / {maxAmmo}"; 
    }
}
