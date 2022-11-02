using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Extensions.Sensors;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class PoliceCarAgent : Agent
{
    public int idNumber;

    public Rigidbody rigidbody;
    public GameObject realCar;
    public float limitVelocity = 0.1f;

    private int raycastQuantity = 12;
    private RaycastHit[] hits;
    private float maxDistance = 15f;

    private bool isCollisionWithWall;
    private bool isCollisionWIthPoliceCarAgent;
    private bool isCollisionWithPlayerCar;
    
    
    
    //OnEpisodeBegin은 에피소드가 시작될 때 생성된다.
    public override void OnEpisodeBegin()
    {
        isCollisionWithWall = false;
        isCollisionWIthPoliceCarAgent = false;
        isCollisionWithPlayerCar = false;
    }
    
    
    //여러가지 요소들을 관찰하는 메소드
    public override void CollectObservations(VectorSensor sensor)
    {
        for (int i = 0; i < raycastQuantity; i++)
        {
            Debug.DrawRay(transform.position,new Vector3(Mathf.Sin(30 * i), 0f, Mathf.Cos(30 * i)), Color.blue, 0.3f);
            hits = Physics.RaycastAll(transform.position, transform.forward, maxDistance);

            for (int j = 0; j < hits.Length; j++)
            {
                float distance = Mathf.Sqrt((hits[j].transform.position.x - this.transform.position.x) *
                                              (hits[j].transform.position.x - this.transform.position.x) +
                                              (hits[j].transform.position.y - this.transform.position.y) *
                                              (hits[j].transform.position.y - this.transform.position.y));
                

                if (hits[j].transform.CompareTag("Wall"))
                {
                    sensor.AddObservation(new Vector2(distance, 1));        //벽을 만났을 땐 1을 함께 대입
                }
                else if (hits[j].transform.CompareTag("PlayerCar"))
                {
                    sensor.AddObservation(new Vector2(distance, 0));        //플레이어 차량을 만났을 땐 0 대입
                }
                else
                {
                    sensor.AddObservation(new Vector2(distance, -1));        //기타 -1 대입
                }
            }
        }
        
        
    }

    public void Update()
    {
        float speed = Mathf.Sqrt(rigidbody.velocity.x * rigidbody.velocity.x + 
                                 rigidbody.velocity.y * rigidbody.velocity.y +
                                 rigidbody.velocity.z * rigidbody.velocity.z);
        
        if (speed > 0f)
        {
            float rotationY;
            if (rigidbody.velocity.z > 0f)
            {
                rotationY = 180f + Mathf.Asin(rigidbody.velocity.x / speed) * 180 / Mathf.PI;
            }
            else if(rigidbody.velocity.x > 0f)
            {
                rotationY = 180f + 90f - Mathf.Asin(rigidbody.velocity.z / speed) * 180 / Mathf.PI;
            }
            else
            {
                rotationY = -Mathf.Asin(rigidbody.velocity.x / speed) * 180 / Mathf.PI;
            }
            realCar.transform.rotation = Quaternion.Euler(new Vector3(0f, rotationY, 0f));
            Debug.Log(rotationY);
        }
        

        
        if (speed > limitVelocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x / speed * limitVelocity,
                                            rigidbody.velocity.y / speed * limitVelocity,
                                            rigidbody.velocity.z / speed * limitVelocity);
        }
    }

    //OnActionReceived는 액션과 보상에 대한 코드가 작성됨
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rigidbody.AddForce(controlSignal);
        
        if (isCollisionWithWall == true || isCollisionWIthPoliceCarAgent)
        {
            SetReward(-10.0f);
            EndEpisode();
        }
        else if (isCollisionWithPlayerCar == true)
        {
            SetReward(30f);
            EndEpisode();
        }
    }
    
    //키보드 입력을 받아 테스트하는 메소드
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
        
        
        //sphereRigidbody.AddForce(new Vector3(continuousActionsOut[0], 0f, continuousActionsOut[1]), ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollisionWithWall = true;
        }
        else if (collision.gameObject.CompareTag("PoliceCarAgent"))
        {
            isCollisionWIthPoliceCarAgent = true;
        }
        else if (collision.gameObject.CompareTag("PlayerCar"))
        {
            isCollisionWithPlayerCar = true;
        }
        
    }
}