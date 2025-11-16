using System.Collections;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public VirtualJoystick steering;

    [SerializeField] private float _fireRate = 0.2f;

    private ShipWeapons _currentWeapons;
    private bool isFiring = false;

    public void SetWeapons(ShipWeapons weapons)
    {
        _currentWeapons = weapons;
    }

    public void RemoveWeapons(ShipWeapons weapons)
    {
        if (_currentWeapons == weapons)
            _currentWeapons = null;
    }

    public void StartFiring()
    {
        if (!isFiring)
            StartCoroutine(FireWeapons());
    }

    private IEnumerator FireWeapons()
    {
        isFiring = true;
        while (isFiring)
        {
            if (_currentWeapons != null)
                _currentWeapons.Fire();

            yield return new WaitForSeconds(_fireRate);
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
