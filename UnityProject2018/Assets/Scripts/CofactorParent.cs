using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofactorParent : MonoBehaviour
{
    public EdgeDataDisplay edgeDataDisplay;
    public GameObject parentObject;
    public List<CofactorLabel> cofactorLabels;
    public string parentLocationName;
    public int childID;

    public bool isReactant = false;
    public bool secondDirection = false;

    private GameObject arrow;
    public Vector3 arrowPosition;

    private Vector3 directionToMesh;

    private bool arrowInitialized = false;

    private float distanceBetweenLabels = 3f;

    public CofactorParent()
    {
        cofactorLabels = new List<CofactorLabel>();
    }


    public Vector3 GetCofactorLabelLocalPosition(CofactorLabel label)
    {
        int childNumber = 0;
        for (int i = 0; i < cofactorLabels.Count; i++)
        {
            if (label == cofactorLabels[i])
            {
                childNumber = i;
                break;
            }
        }

        if (edgeDataDisplay.GetComponent<MeshCollider>())
        {
            Vector3 meshPosition = GetClosestPointToMesh(transform.position, edgeDataDisplay.GetComponent<MeshCollider>().gameObject);

            // Getting normalized direction between this and the closest point on the mesh

            Vector3 normalizedDirection = (meshPosition - transform.position).normalized;
            directionToMesh = normalizedDirection;

            return (normalizedDirection * (distanceBetweenLabels * (childNumber + 1))) * (-1f);

        }
        else
        {
            Debug.Log("No meshfilter found in parent for cofactorparent: " + parentLocationName);
            return Vector3.zero;
        }


    }

    public void InitializeArrow()
    {
        if (arrowInitialized)
            return;

        GameObject arrowPrefab = Resources.Load<GameObject>("Prefabs/CofactorArrow");
        GameObject instantiatedArrow = Instantiate(arrowPrefab);

        instantiatedArrow.transform.SetParent(parentObject.transform, false);
        CalculateArrowPosition();
        instantiatedArrow.transform.position = arrowPosition;

        arrowInitialized = true;

        arrow = instantiatedArrow;

        Vector3 meshPosition = GetClosestPointToMesh(transform.position, edgeDataDisplay.GetComponent<MeshCollider>().gameObject);
        directionToMesh = (meshPosition - transform.position).normalized;

        if (directionToMesh.y < -0.5f) // Check if arrow should point up
        {
            instantiatedArrow.transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(90, 90, 0));//.SetLocalPositionAndRotation(instantiatedArrow.transform.localPosition, (instantiatedArrow.transform.rotation + new Vector3(0, 90, 0)))
        }
        else if (directionToMesh.y > 0.5f) // Check if arrow should point down
        {
            instantiatedArrow.transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(90, 270, 0));
        }
        else if (directionToMesh.x > 0.5f) // Left or Right
        {
            instantiatedArrow.transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(90, 180, 0));
        }
        else
        {
            instantiatedArrow.transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        }




        //Debug.Log("Initializing Arrow");

    }

    public void CalculateArrowPosition()
    {
        if (edgeDataDisplay.GetComponent<MeshCollider>())
        {
            Vector3 meshPosition = GetClosestPointToMesh(transform.position, edgeDataDisplay.GetComponentInParent<MeshCollider>().gameObject);
            arrowPosition = (transform.position + meshPosition) / 2f;
        }
    }


    /// <summary>
    /// Grabbing the closest point to the edge mesh.
    /// This is necessary to place the arrow in the right spot
    /// </summary>
    /// <param name="point"></param>
    /// <param name="targetMeshObject"></param>
    /// <returns></returns>
    Vector3 GetClosestPointToMesh(Vector3 point, GameObject targetMeshObject)
    {
        MeshCollider meshCollider = targetMeshObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            Debug.LogError("Target GameObject does not have a MeshCollider component.");
            return Vector3.zero;
        }

        Vector3 closestPoint = meshCollider.ClosestPoint(point);
        return closestPoint;
    }

}


