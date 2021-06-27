using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{
    [SerializeField] private int _maxValue;
    public Image fill;
    [SerializeField] private int _currentValue;
    private Player _player;
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine _regen;
    [SerializeField] private Text _thrusterCount;

    
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _currentValue = _player._currentThrusters;
        _maxValue = _player._maxThrusters;
      
        fill.fillAmount = 1; //normalized value 0 - 1

        _thrusterCount.text = _currentValue + "%";
    }

    public void UseThruster (int amount)
    {
        if(_currentValue - amount >= 0)
        {
            _currentValue -= amount;

            fill.fillAmount = (float)_currentValue / _maxValue;
            _player.ActivateThruster();
            _thrusterCount.text = _currentValue + "%";

            if (_regen != null)
            {
                StopCoroutine(_regen);
            }

            _regen = StartCoroutine(RegenThruster());
        }
        else
        {
            _player.DeactivateThruster();
        }
        
    }

    IEnumerator RegenThruster()
    {
        yield return new WaitForSeconds(2.0f);
        _player.ActivateThruster();

        while(_currentValue < _maxValue)
        {
            _currentValue += _maxValue / 100;
            fill.fillAmount = (float)_currentValue / _maxValue;
            _thrusterCount.text = _currentValue + "%";
            yield return regenTick;
        }

        _regen = null;
    }
}

