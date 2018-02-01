using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Collections.Generic;

public class AgentCreator : MonoBehaviour {

    [SerializeField]
    GameObject _prefab;

    [SerializeField]
    int _amount;

    [SerializeField]
    Transform _agentPosition;

    [SerializeField]
    Transform _targetPosition;

    RVO2Simulator simulator;
    Seeker agentSeeker;

    List<Vector3> pathNodes = null;

    // Use this for initialization
    IEnumerator Start () {

        simulator = GameObject.FindGameObjectWithTag("RVOSim").GetComponent<RVO2Simulator>();
        agentSeeker = gameObject.GetComponent<Seeker>();
        pathNodes = new List<Vector3>();

        yield return StartCoroutine(StartPaths(_prefab));
        StartCoroutine(GenerateAgent());
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    IEnumerator StartPaths(GameObject go)
    {
        agentSeeker = gameObject.GetComponent<Seeker>();
        var path = agentSeeker.StartPath(go.transform.position, _targetPosition.position, OnPathComplete);
        yield return StartCoroutine(path.WaitForPath());

    }

    public void OnPathComplete(Path p)
    {
        if (p.error) {
            Debug.Log("" + this.gameObject.name + " ---- -" + p.errorLog);
        }
        else {
            pathNodes = p.vectorPath;
        }
    }

    public List<Vector3> getPathNodes()
    {
        return pathNodes;
    }

    IEnumerator GenerateAgent()
    {
        int count = 0;
        while (count != _amount)
        {
            GameObject agentGameObject = (GameObject)Instantiate(_prefab, transform.position, Quaternion.Euler(0,270,0));
            agentGameObject.name = _prefab.name + "C" + count;
            yield return new WaitForSeconds(1);
            count++;
        }
    }
}