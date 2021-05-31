using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour

{
    public int maxValue;
    public Image fill;
    private int _currentValue;
    private Player _player;
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine _regen;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _currentValue = maxValue;
        fill.fillAmount = 1; //normalized value 0 - 1
    }

    public void UseThruster (int amount)
    {
        if(_currentValue - amount >= 0)
        {
            _currentValue -= amount;

            /*if (_currentValue < 0)
            {
                _currentValue = 0;
            }*/
            fill.fillAmount = (float)_currentValue / maxValue;
            _player.ActivateThruster();

            if(_regen != null)
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

        while(_currentValue < maxValue)
        {
            _currentValue += maxValue / 100;
            fill.fillAmount = (float)_currentValue / maxValue;
            yield return regenTick;
        }

        _regen = null;
    }

    /*public void AddThruster (int thruster)
   {
       _currentValue += thruster;

       if(_currentValue > maxValue)
       {
           _currentValue = maxValue;
       }

       fill.fillAmount = (float)_currentValue / maxValue; //fill needs to be between 0-1. It expects a float. 
   }*/
}

