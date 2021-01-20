using UnityEngine;

public class FoodArea : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab = null;

    private GameObject food;

    private void Awake()
    {
        food = Instantiate(foodPrefab, transform);
    }

    public void ResetFoodPosition()
    {
        food.transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f));
        food.transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);
    }
}