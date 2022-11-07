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

    float agentSpeed = 0.3f;
    StatsRecorder m_statsRecorder;

    private float deltaReward;

    public override void Initialize()
    {
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
        switch (action)
        {
            case 0:
                //AddReward(1f / MaxStep);
                dirToGo = transform.forward * 0f;
                break;
            default:
                dirToGo = transform.forward * 1f;
                break;
        }

        //회전
        action = act[1];
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
                rotateDir = transform.up * -1f;
                break;
            case 4:
                rotateDir = transform.up * -2f;
                break;
            default:
                break;
        }
        
        transform.Rotate(rotateDir, Time.deltaTime * 150f);
        rigidbody.AddForce(dirToGo * agentSpeed, ForceMode.VelocityChange);
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
            SetReward(1f);
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
        ResetPosition();
        
        m_statsRecorder.Add("Collision/Player", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Collision/PoliceCarAgent", 0, StatAggregationMethod.Sum);
        m_statsRecorder.Add("Collision/Wall", 0, StatAggregationMethod.Sum);
    }

    void ResetPosition()
    {
        transform.position = StageManager.instance.firstPoliceCarPos[agentID].transform.position;
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        rigidbody.velocity *= 0f;
    }
}
