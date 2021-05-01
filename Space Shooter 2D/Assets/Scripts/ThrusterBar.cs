using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{
    public Slider thrusterBar;
    private int _maxThruster = 100;
    private int _currentThruster;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);

    private Coroutine _regen;
    private Player _player;

    public Gradient gradient;
    public Image fill;

    // Start is called before the first frame update
    void Start()
    {
        _currentThruster = _maxThruster;
        thrusterBar.maxValue = _maxThruster;
        thrusterBar.value = _maxThruster;
        _player = GameObject.Find("Player").GetComponent<Player>();

        fill.color = gradient.Evaluate(1f);
    }

    public void UseThruster(int amount)
    {
        if(_currentThruster - amount >= 0)
        {
            _currentThruster -= amount;
            thrusterBar.value = _currentThruster;
            fill.color = gradient.Evaluate(thrusterBar.normalizedValue);
            _player.ActivateThruster();

            if(_regen != null)
            {
                StopCoroutine(_regen);
            }

            _regen = StartCoroutine(RegenThrusters());

        }
        else
        {
            _player.DeactivateThruster();
        }
    }

    private IEnumerator RegenThrusters()
    {
        yield return new WaitForSeconds(2.0f);
        _player.ActivateThruster();

        while (_currentThruster < _maxThruster)
        {
            _currentThruster += _maxThruster / 100;
            thrusterBar.value = _currentThruster;
            fill.color = gradient.Evaluate(thrusterBar.normalizedValue);
            yield return regenTick;
        }

        _regen = null;
    }

}
