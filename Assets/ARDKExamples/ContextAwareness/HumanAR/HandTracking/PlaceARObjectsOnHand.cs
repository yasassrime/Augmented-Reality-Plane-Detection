using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceARObjectsOnHand : MonoBehaviour
{

    [SerializeField] private HandPositionSolver handPositionSolver;
    [SerializeField] private GameObject ARObject;
    [SerializeField] private float objectMovingSpeed = 0.5f;
    [SerializeField] private float objectRotationSpeed = 25.0f;

    private float minimumDistanceToObject = 0.05f;
    private float minimumAngleToObject = 2.0f;
    private bool isRequiredAngleChange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlaceObjectsOnHand(handPositionSolver.HandPosition);
    }

    private void PlaceObjectsOnHand(Vector3 handPosition)
    {
        float currentDistanceToObject = Vector3.Distance(handPosition, ARObject.transform.position);
        ARObject.transform.position = Vector3.MoveTowards(ARObject.transform.position, handPosition, objectMovingSpeed * Time.deltaTime);

        if(currentDistanceToObject >= minimumDistanceToObject)
        {
            ARObject.transform.LookAt(handPosition);
            isRequiredAngleChange = true;
        }

        if (isRequiredAngleChange)
        {
            ARObject.transform.rotation = Quaternion.Slerp(ARObject.transform.rotation, Quaternion.identity, 2 * Time.deltaTime);
            Vector3 angles = ARObject.transform.rotation.eulerAngles;
            isRequiredAngleChange = angles.magnitude >= minimumAngleToObject;
        } else
        {
            ARObject.transform.Rotate(Vector3.up * objectMovingSpeed * Time.deltaTime);
        }
        }
}
