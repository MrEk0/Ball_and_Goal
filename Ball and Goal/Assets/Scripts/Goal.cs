using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] Color32 _winColor;
    [SerializeField] GameObject _victoryPanel;
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<Ball>())
        {
            GetComponent<MeshRenderer>().material.color = _winColor;
            collider.GetComponent<Ball>().CanFire = false;
            _victoryPanel.SetActive(true);
        }
    }
}
