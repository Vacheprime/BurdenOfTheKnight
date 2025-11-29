using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject SwordWeapon;
    public GameObject MagicWeapon;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwordWeapon.SetActive(true);
            MagicWeapon.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwordWeapon.SetActive(false);
            MagicWeapon.SetActive(true);
        }
        
    }
}
