using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Transform[] routes;
    private int routeToGo;
    private float tParam;
    private Vector3 objectPosition;
    private float speedModifier;
    private bool coroutineAllowed;

    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.1f;
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector3 p0 = routes[routeNum].GetChild(0).localPosition;
        Vector3 p1 = new Vector3(Random.Range(0f,4f),0.44f,Random.Range(-2.5f,0f));
        //Vector3 p1 = routes[routeNum].GetChild(1).position;
        Vector3 p2 = new Vector3(Random.Range(-3f, 0f), 0.44f, Random.Range(0f, 3f));
        //Vector3 p2 = routes[routeNum].GetChild(2).position;
        Vector3 p3 = routes[routeNum].GetChild(3).localPosition;

        while (tParam <= 1)
        {
            tParam += Time.deltaTime * speedModifier;
            //tParam += 0.005f;
            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
            
            transform.localPosition = new Vector3(objectPosition.x, 0.35f, objectPosition.z);
            

            
            yield return new WaitForEndOfFrame();
        }
        //transform.parent.transform.LookAt(position);
        tParam = 0f;

        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;

    }

}
