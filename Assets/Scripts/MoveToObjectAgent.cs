using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToObjectAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Renderer decor;
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(0, 4.5f), 13.2f, Random.Range(11f, 18f));
        targetTransform.localPosition = new Vector3(Random.Range(-2.8f, 0), 13.2f, Random.Range(11f, 18f));

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float MoveX = actions.ContinuousActions[0];
        float MoveZ = actions.ContinuousActions[1];
        float moveSpeed = 5f;
        transform.position += new Vector3(MoveX, 0, MoveZ) * Time.deltaTime * moveSpeed;
    }

    //for testing purpose only
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.name);
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1.0f);
            decor.material.color = Color.green;
            EndEpisode();

        }
        if (other.TryGetComponent<Wall>(out Wall wall)) 
        {
            SetReward(-1.0f);
            decor.material.color = Color.red;
            EndEpisode();
        }
    }
}
