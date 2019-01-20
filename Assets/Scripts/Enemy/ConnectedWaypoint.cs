using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedWaypoint : Waypoint {
    [SerializeField] protected float _connectivityRadius = 50f;
    List<ConnectedWaypoint> _connections;
    //public GameObject[] allWaypoints;
    // Use this for initialization
    void Start () {
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("wayPoint");
        _connections = new List<ConnectedWaypoint>();
        for (int i = 0; i < allWaypoints.Length; i++)
        {
            ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();

            //i.e. we found a waypoint.
            if (nextWaypoint != null)
            {
                if (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= _connectivityRadius && nextWaypoint != this)
                {
                    _connections.Add(nextWaypoint);
                }
            }
        }
    }
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _connectivityRadius);
    }
    // Update is called once per frame
    public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
    {
        if (_connections.Count == 0)
        {
            //No waypoints?  Return null and complain.
            //连接黄线以内的红点
            Debug.LogError("Insufficient waypoint count.");
            return null;
        }
        else if (_connections.Count == 1 && _connections.Contains(previousWaypoint))
        {
            //Only one waypoint and it's the previous one? Just use that.
            return previousWaypoint;
        }
        else //Otherwise, find a random one that isn't the previous one.
        {
            ConnectedWaypoint nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = Random.Range(0, _connections.Count);
                nextWaypoint = _connections[nextIndex];

            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}
