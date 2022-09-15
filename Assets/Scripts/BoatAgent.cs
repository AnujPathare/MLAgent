using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class BoatAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private GameObject boat;
    //[SerializeField] private GameObject episode;
    private int epNo = 0;
    private float accuracy = 0;
    [SerializeField]float moveSpeed = 5f;
    private float prevDis;
    public override void OnEpisodeBegin()
    {
        epNo += 1;
        Debug.Log(epNo + " accuracy: " + accuracy / epNo * 100 + "%");
        //episode.GetComponent<Text>().text = "Episode: " + epNo;
        pointB.transform.localPosition = new Vector3(Random.Range(-1.6f, 1.3f), 0.41f, Random.Range(2.5f, 4f));
        transform.parent.transform.localPosition = new Vector3(-3f, 0.7f, -3f);
        
        //transform.position = pointA.transform.position + new Vector3(0.5f, 0, 0.5f);

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        //sensor.AddObservation(pointA.transform.localPosition);
        sensor.AddObservation(pointB.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float MoveX = actions.ContinuousActions[0];
        float MoveZ = actions.ContinuousActions[1];
       

        transform.parent.transform.localPosition += new Vector3(MoveX, 0, MoveZ) * Time.deltaTime * moveSpeed;
        
        Vector3 movementDir = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
        if (movementDir != Vector3.zero)
        {
            //transform.parent.transform.forward = movementDir;
        }
    }
    //for testing purpose only
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void Update()
    {
        float prevDis = 999f;
        float currDis = Vector3.Distance(boat.transform.localPosition, pointB.transform.localPosition);
        if (currDis < prevDis)
        {
            SetReward(+0.1f);
        }
        else
        {
            SetReward(-0.1f);
        }
        prevDis = currDis;
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1.0f);
            boat.GetComponent<Renderer>().material.color = Color.red;
            //EndEpisode();
        }
        if (other.TryGetComponent<Whale>(out Whale whale))
        {
            SetReward(-1.0f);
            boat.GetComponent<Renderer>().material.color = Color.magenta;
            //EndEpisode();
        }
        if (other.TryGetComponent<Checkpoint>(out Checkpoint cp))
        {
            SetReward(+1.0f);
            boat.GetComponent<Renderer>().material.color = Color.yellow;
            
        }
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1.0f);
            boat.GetComponent<Renderer>().material.color = Color.green;
            EndEpisode();
            accuracy += 1;

        }

 

    }
}
