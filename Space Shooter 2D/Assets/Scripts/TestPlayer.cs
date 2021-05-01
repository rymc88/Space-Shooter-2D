using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour
{
    public ThrusterBar thrusterBar;

    // Start is called before the first frame update
    private void Start()
    {
        thrusterBar = GameObject.Find("ThrusterBar").GetComponent<ThrusterBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            thrusterBar.UseThruster(1);
        }
    }
}
