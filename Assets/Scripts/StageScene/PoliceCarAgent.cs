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
    private float limitVelocity = 1.5f;

    private int raycastQuantity = 24 * 3;
    private RaycastHit hit;
    private float maxDistance = 200f;

    private bool isCollisionWithWall;
    private bool isCollisionWIthPoliceCarAgent;
    private bool isCollisionWithPlayerCar;

    private float speed;
    private float multiplePower = 3f;

    private float beforeForceX;
    private float beforeForceZ;
    
    
    
    //OnEpisodeBegin은 에피소드가 시작될 때 생성된다.
    public override void OnEpisodeBegin()
    {
        isCollisionWithWall = false;
        isCollisionWIthPoliceCarAgent = false;
        isCollisionWithPlayerCar = false;
        beforeForceX = 0f;
        beforeForceZ = 0f;

        transform.position = StageManager.instance.firstPoliceCarPos[idNumber].transform.position;
    }
    
    
    //여러가지 요소들을 관찰하는 메소드
    public override void CollectObservations(VectorSensor sensor)
    {
        for (int i = 0; i < raycastQuantity; i++)
        {

            Vector3 raycastDirection = new Vector3(Mathf.Sin(5 * i),
                0f,
                Mathf.Cos(5 * i));
            
            /*Vector3 raycastDirection = new Vector3(
                rigidbody.velocity.x / speed * Mathf.Cos(5 * i)
                - rigidbody.velocity.z / speed * Mathf.Sin(5 * i),
                0f,
                rigidbody.velocity.x / speed * Mathf.Sin(5 * i)
                + rigidbody.velocity.z / speed * Mathf.Cos(5 * i));*/
            
            //Debug.Log(raycastDirection);
            //Debug.DrawRay(transform.position, raycastDirection, Color.blue, 0.2f);

            if (Physics.Raycast(transform.position, raycastDirection, out hit, maxDistance))
            {
                float distance = Mathf.Sqrt((hit.transform.position.x - this.transform.position.x) *
                                              (hit.transform.position.x - this.transform.position.x) +
                                              (hit.transform.position.z - this.transform.position.z) *
                                              (hit.transform.position.z - this.transform.position.z));
            
                if (hit.transform.CompareTag("Wall"))
                {
                    sensor.AddObservation(new Vector2(distance, 1));        //벽을 만났을 땐 1을 함께 대입
                }
                else if (hit.transform.CompareTag("PlayerCar"))
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
        speed = Mathf.Sqrt(rigidbody.velocity.x * rigidbody.velocity.x + 
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
            //Debug.Log(rotationY);
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
        controlSignal.x = actionBuffers.ContinuousActions[0] * multiplePower;
        controlSignal.z = actionBuffers.ContinuousActions[1] * multiplePower;
        rigidbody.AddForce(controlSignal);


        float a = Mathf.Sqrt(controlSignal.x * controlSignal.x + controlSignal.z * controlSignal.z);
        float b = Mathf.Sqrt((controlSignal.x - beforeForceX) * (controlSignal.x - beforeForceX) +
                             (controlSignal.z - beforeForceZ) * (controlSignal.z - beforeForceZ));
        float c = Mathf.Sqrt(beforeForceX * beforeForceX + beforeForceZ + beforeForceZ);

        float angle = Mathf.Acos((a * a + b * b - c * c) / 2f * a * b) * 180 / Mathf.PI;

        if (Mathf.Abs(angle) >= 10f)
        {
            SetReward(-(int)(angle / 10f));
        }
        
        
        Debug.Log("angle : " + angle);

        beforeForceX = controlSignal.x;
        beforeForceZ = controlSignal.z;
        
        
        
        if (isCollisionWithWall == true || isCollisionWIthPoliceCarAgent)
        {
            SetReward(-1000000000);
            EndEpisode();
        }
        else if (isCollisionWithPlayerCar == true)
        {
            SetReward(5000000000);
            EndEpisode();
        }
    }
    
    //키보드 입력을 받아 테스트하는 메소드
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal") * multiplePower;
        continuousActionsOut[1] = Input.GetAxis("Vertical") * multiplePower;
        
        
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