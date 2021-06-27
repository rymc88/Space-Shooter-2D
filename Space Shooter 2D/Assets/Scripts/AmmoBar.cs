using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private Image _fill;
    [SerializeField] private Text _ammoCount;
    private int _currentValue;
    private int _maxValue;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
       
        _player = GameObject.Find("Player").GetComponent<Player>();
        _fill.fillAmount = 1;//normalized value 0 - 1

        _currentValue = _player._maxAmmo;
        _maxValue = _player._maxAmmo;

        _ammoCount.text = $"{_currentValue}/{_maxValue}"; 
    }

    public void UseAmmo(int amount)
    {
        _currentValue -= amount;

        if(_currentValue <= 0)
        {
            _currentValue = 0;
        }

        _fill.fillAmount = (float)_currentValue / _maxValue;
        _ammoCount.text = $"{_currentValue}/{_maxValue}";

    }

    public void ReloadAmmo()
    {
        _currentValue = _maxValue;
        _fill.fillAmount = (float)_currentValue / _maxValue;
        _ammoCount.text = $"{_currentValue}/{_maxValue}";
    }

}
