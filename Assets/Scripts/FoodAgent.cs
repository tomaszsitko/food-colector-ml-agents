using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class FoodAgent : Agent
{
    private const float MoveSpeed = 5f;
    private const float RotationSpeed = 80f;

    [SerializeField] private TextMeshPro text = null;

    private FoodArea foodArea;
    private Rigidbody rb;

    public override void Initialize()
    {
        base.Initialize();

        rb = GetComponent<Rigidbody>();
        foodArea = GetComponentInParent<FoodArea>();

        RestartEnvironment();
    }

    private void RestartEnvironment()
    {
        foodArea.ResetFoodPosition();
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);
        sensor.AddObservation(transform.forward);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float forwardAmount = vectorAction[0];

        float turnAmount = 0f;

        if (vectorAction[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            turnAmount = 1f;
        }

        rb.MovePosition(transform.position + transform.forward * forwardAmount * MoveSpeed * Time.deltaTime);
        transform.Rotate(transform.up * turnAmount * RotationSpeed * Time.deltaTime);

        if (MaxStep > 0) AddReward(-1f / MaxStep);

        text.text = GetCumulativeReward().ToString("F2");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Food"))
        {
            AddReward(1f);
            foodArea.ResetFoodPosition();
        }
        else if (collision.transform.CompareTag("Wall"))
        {
            AddReward(-.01f);
        }
    }
}