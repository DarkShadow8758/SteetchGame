using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private Vector3[] locations;
    [Header("Items")]
    [SerializeField] private GameObject[] possibleItems;
    [Header("Obstacles")]
    [SerializeField] private GameObject[] possibleObstacles;

    public void SpawnItem()
    {
        if (possibleItems.Length > 0)
        {
            int rIndexItem = Random.Range(0, possibleItems.Length);
            int rIndexLocation = Random.Range(0, locations.Length);
            Instantiate(possibleItems[rIndexItem], locations[rIndexLocation], Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Nenhum item possível definido no array possibleItems.");
        }
    }

    public void SpawnObstacle()
    {
        
        if (possibleObstacles.Length > 0)
        {
            int rIndexW = Random.Range(0, possibleObstacles.Length);
            int rIndexLocation = Random.Range(0, locations.Length);
            Instantiate(possibleObstacles[rIndexW], locations[rIndexLocation] + new Vector3(0, 0, -5), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Nenhuma parede possível definida no array possibleObstacles.");
        }
    }
}
