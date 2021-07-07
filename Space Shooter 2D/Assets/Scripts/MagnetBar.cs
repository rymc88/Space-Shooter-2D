using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnetBar : MonoBehaviour
{
    [SerializeField] private int _maxValue;
    public Image fill;
    [SerializeField] private int _currentValue;
    private Player _player;
    [SerializeField] private Text _magnetCount;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _currentValue = _player._maxMagnet;
        _maxValue = _player._maxMagnet;

        fill.fillAmount = 1; //normalized value 0 - 1

        _magnetCount.text = _currentValue + "%";
    }

    public void UseMagnet(int amount)
    {
        if (_currentValue - amount >= 0)
        {
            _currentValue -= amount;
            _player.ActivateMagnet();
            fill.fillAmount = (float)_currentValue / _maxValue;
            _magnetCount.text = _currentValue + "%";
        }
        else
        {
            _player.DeactivateMagnet();
        }

    }

    public void ReloadMagnet()
    {
        _currentValue = _maxValue;
        fill.fillAmount = (float)_currentValue / _maxValue;
        _magnetCount.text = _currentValue + "%";
    }
}
