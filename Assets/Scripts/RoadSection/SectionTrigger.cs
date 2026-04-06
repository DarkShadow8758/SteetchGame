using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public float moveStep;
    public GameObject roadSection;
    private SpawnController spawnController;

    private void Start()
    {
        spawnController =  FindAnyObjectByType<SpawnController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
             Instantiate(roadSection, new Vector3(0, 0, moveStep), Quaternion.identity);

             spawnController.SpawnItem();
             spawnController.SpawnObstacle();
        }
    }
}
