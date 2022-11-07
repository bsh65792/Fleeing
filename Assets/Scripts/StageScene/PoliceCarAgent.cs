using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PoliceCarAgent : Agent
{
    public int agentID;
    
    public bool useVectorObs;
    Rigidbody rigidbody;

    float agentSpeed = 0.7f;
    StatsRecorder m_statsRecorder;

    private float deltaReward;

    public float limitVelocity;
    

    public override void Initialize()
    {
        limitVelocity = 0.1f;
        
        deltaReward = -1f / 21f / MaxStep;
        rigidbody = GetComponent<Rigidbody>();
        m_statsRecorder = Academy.Instance.StatsRecorder;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            sensor.AddObservation(StepCount / (float)MaxStep);
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        
        //가감속
        /*switch (action)
        {
            case 0:
                //AddReward(1f / MaxStep);
                dirToGo = transform.forward * 0f;
                break;
            default:
                dirToGo = transform.forward * 1f;
                break;
        }*/
        
        //dirToGo = transform.forward * 1f;

        //회전
        action = act[0];
        switch (action)
        {
            case 0:
                //직진
                break;
            case 1:
                rotateDir = transform.up * 1f;
                break;
            case 2:
                rotateDir = transform.up * 2f;
                break;
            case 3:
                rotateDir = transform.up * 3f;
                break;
            case 4:
                rotateDir = transform.up * 4f;
                break;
            case 5:
                rotateDir = transform.up * -1f;
                break;
            case 6:
                rotateDir = transform.up * -2f;
                break;
            case 7:
                rotateDir = transform.up * -3f;
                break;
            case 8:
                rotateDir = transform.up * -4f;
                break;
            default:
                break;
        }
        
        transform.Rotate(rotateDir, Time.deltaTime * 500f);
        
        //rigidbody.AddForce(dirToGo * agentSpeed, ForceMode.VelocityChange);
        /*if (Mathf.Sqrt(rigidbody.velocity.x * rigidbody.velocity.x + rigidbody.velocity.z + rigidbody.velocity.z) <=
            limitVelocity)
        {
            rigidbody.AddForce(dirToGo * agentSpeed, ForceMode.VelocityChange);
        }*/
        
    }

    private void FixedUpdate()
    {
        /*rigidbody.AddForce(transform.forward * agentSpeed / 60f, ForceMode.VelocityChange);
        if()*/
        rigidbody.velocity = transform.forward * agentSpeed;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        AddReward(-1f / MaxStep);
        MoveAgent(actionBuffers.DiscreteActions);
        //Debug.Log("step : " + StepCount);
    }

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("tag name : " + col.gameObject.tag);
        if (col.gameObject.CompareTag("PlayerCar"))
        {
            SetReward(10f);
            //Debug.Log("PlayerCar와 충돌함.");
            m_statsRecorder.Add("Collision/Player", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }
        else if (col.gameObject.CompareTag("Wall"))
        {
            SetReward(-1f);
            m_statsRecorder.Add("Collision/Wall", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }
        /*else if (col.gameObject.CompareTag("PoliceCarAgent"))
        {
            SetReward(-1f);
            m_statsRecorder.Add("Collision/PoliceCarAgent", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }*/
        
        
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            discreteActionsOut[1] = 1;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            discreteActionsOut[1] = 3;
        }
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            discreteActionsOut[0] = 0;
        }
    }

    public override void OnEpisodeBegin()
    {
        StageManager.instance.PrepareNextEpisode();
        
        m_statsRecorder.Add("Collision/Player", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Collision/PoliceCarAgent", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Collision/Wall", 0, StatAggregationMethod.Sum);
    }
    
}
