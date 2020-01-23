using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    Cursor cursor;
    NavMeshAgent navMeshAgent;
    Shot shot;
    public Transform gunBarrel;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cursor = FindObjectOfType<Cursor>();
        shot = FindObjectOfType<Shot>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
            dir.z = -1.0f;
        if (Input.GetKey(KeyCode.RightArrow))
            dir.z = 1.0f;
        if (Input.GetKey(KeyCode.UpArrow))
            dir.x = -1.0f;
        if (Input.GetKey(KeyCode.DownArrow))
            dir.x = 1.0f;
        navMeshAgent.velocity = dir.normalized * moveSpeed;

        Vector3 forward = cursor.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(forward.x, 0, forward.z));

       if (Input.GetMouseButtonDown(0)) {
            var from = gunBarrel.position;
            var target = cursor.transform.position;
            var to = new Vector3(target.x, from.y, target.z);

            var direction = (to - from).normalized;

            RaycastHit hit;
            if (Physics.Raycast(from, to - from, out hit, 100)) {
                to = new Vector3(hit.point.x, from.y, hit.point.z);
                if (hit.transform != null) {
                    var zombie = hit.transform.GetComponent<Zombie>();
                    if (zombie != null)
                        zombie.Kill();
                }
            }
            else
                to = from + direction * 100;

            shot.Show(from, to);
        }
    }
}
