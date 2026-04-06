using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float maxX = 3f;
    public int points;
    [SerializeField] private UIManager uiManager;

    private void Update()
    {
        if (points >= 1000)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
    public void MoveLeft()
    { 
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + 3, -maxX, maxX), transform.position.y, transform.position.z);
        
    }

    public void MoveRight()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x - 3, -maxX, maxX), transform.position.y, transform.position.z);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene("LoseScreen");
        }
        if (other.gameObject.CompareTag("Item"))
        {
            uiManager.IncreaseCollects();
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                points += item.points;
                uiManager.SetPoints(points);
            }

            Destroy(other.gameObject);
        }
    }
}
