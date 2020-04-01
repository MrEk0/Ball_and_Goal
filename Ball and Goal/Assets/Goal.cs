using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] Color32 winColor;
    [SerializeField] GameObject victoryPanel;
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<Ball>())
        {
            GetComponent<MeshRenderer>().material.color = winColor;
            collider.GetComponent<Ball>().CanFire = false;
            victoryPanel.SetActive(true);
        }
    }
}
