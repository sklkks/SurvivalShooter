using System.Runtime.Serialization;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField]
    private bool isLookAt = true;

    private void Update()
    {
        if(isLookAt == false)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);

        if(plane.Raycast(ray, out float distance))
        {
            Vector3 direction = ray.GetPoint(distance) - transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
    }
    
}
