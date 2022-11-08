using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class PoliceCarAgent : Agent
{
    public int agentID;
    
    public bool useVectorObs;
    Rigidbody rigidbody;

    float agentSpeed = 2f;
    StatsRecorder m_statsRecorder;

    private float deltaReward;

    public float limitVelocity;

    public override void Initialize()
    {

        rigidbody = GetComponent<Rigidbody>();
        m_statsRecorder = Academy.Instance.StatsRecorder;
        //StartCoroutine("Co_TurnToPlayerCar");
    }



    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            //sensor.AddObservation(StepCount / (float)MaxStep);
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];

        //회전
        action = act[0];
        switch (action)
        {
            case 0:
                //직진
                AddReward(1f / MaxStep);
                break;
            case 1:
                rotateDir = transform.up * 1f;
                break;
            case 2:
                rotateDir = transform.up * 2f;
                break;
            case 3:
                rotateDir = transform.up * 7f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                rotateDir = transform.up * -2f;
                break;
            case 6:
                rotateDir = transform.up * -7f;
                break;
            default:
                break;
        }
        
        transform.Rotate(rotateDir, Time.deltaTime * 1000f);
        
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = transform.forward * agentSpeed;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        MoveAgent(actionBuffers.DiscreteActions);

        //Debug.Log("step : " + StepCount);
    }

    void OnCollisionEnter(Collision col)
    {
        
        //Debug.Log("tag name : " + col.gameObject.tag);
        if (col.gameObject.CompareTag("Wall"))
        {
            /*if (StageManager.instance.isLearning == false)
            {
                Destroy(gameObject);
                return;
            }*/
            AddReward(-1f);
            m_statsRecorder.Add("Collision/Wall", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }
        else if (col.gameObject.CompareTag("PoliceCarAgent"))
        {
            /*if (StageManager.instance.isLearning == false)
            {
                Destroy(gameObject);
                return;
            }*/
            AddReward(-1f);
            m_statsRecorder.Add("Collision/PoliceCarAgent", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }

    }

    private void OnCollisionStay(Collision col)
    {
        //Debug.Log("tag name : " + col.gameObject.tag);
        if (col.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);
            m_statsRecorder.Add("Collision/Wall", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }
        else if (col.gameObject.CompareTag("PoliceCarAgent"))
        {
            AddReward(-1f);
            m_statsRecorder.Add("Collision/PoliceCarAgent", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            discreteActionsOut[0] = 2;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            discreteActionsOut[0] = 6;
        }
    }

    public override void OnEpisodeBegin()
    {
        //StageManager.instance.PrepareNextEpisode();

        if (GameSceneManager.instance != null)
        {
            GameSceneManager.instance.ResetPoliceCarPosition(gameObject);
        }
        else
        {
            StageManager.instance.ResetPoliceCarAgentPosition(gameObject);
        }
        
        
        //m_statsRecorder.Add("Collision/Player", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Collision/PoliceCarAgent", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Collision/Wall", 0, StatAggregationMethod.Sum);
    }
    
}
