using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int maxValue;
    public Image fill;
    private int _currentValue;

    // Start is called before the first frame update
    void Start()
    {
        _currentValue = maxValue;
        fill.fillAmount = 1; //normalized value 0 - 1
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddHealth(10);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SubtractHealth(5);
        }

    }

    public void AddHealth (int health)
    {
        _currentValue += health;

        if(_currentValue > maxValue)
        {
            _currentValue = maxValue;
        }

        fill.fillAmount = (float)_currentValue / maxValue; //fill needs to be between 0-1. It expects a float. 
    }

    public void SubtractHealth(int health)
    {
        _currentValue -= health;

        if (_currentValue < 0)
        {
            _currentValue = 0;
        }

        fill.fillAmount = (float)_currentValue / maxValue;
    }

}

