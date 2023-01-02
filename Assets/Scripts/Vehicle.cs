using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	// The current node that the vehicle is at
	public Node currentNode;
	// A list of nodes that the vehicle needs to visit
	public List<int> destinationNodes;

	Collider2D c;

	Network network;

	private void Awake()
	{
		// Find the Network script in the scene and store a reference to it
		network = Network.instance;
		c = GetComponent<Collider2D>();
		transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	private void FixedUpdate()
	{
		if (destinationNodes.Count < 1)
			return;

		// If the vehicle has reached its destination, remove the node from the list
		if (Vector2.Distance(transform.position, currentNode.transform.position) < 0.1f)
		{
			destinationNodes.RemoveAt(0);
		}
		// If the vehicle has more nodes to visit, move to the next one
		if (destinationNodes.Count > 0)
		{
			Node nextNode = network.GetNode(destinationNodes[0]);
			// Find the segment connecting the current node and the next node
			Segment segment;// = network.GetSegment(currentNode.id, nextNode.id);
			Collider2D[] colliders = new Collider2D[1];
			c.OverlapCollider(new ContactFilter2D { layerMask = LayerMask.GetMask("Segment"), useLayerMask = true }, colliders);
			segment = colliders[0]?.transform.GetComponent<Segment>();
			float speed = 70f;
			if (segment != null)
			{
				speed = segment.speedLimit;
			}
			// Calculate the direction to the next node
			Vector2 direction = nextNode.transform.position - transform.position;
			// Move the vehicle in the direction of the next node
			transform.position += (Vector3)direction.normalized * speed * 0.01f * Time.fixedDeltaTime;
			transform.LookAt(nextNode.transform.position);
			// Update the current node
			currentNode = nextNode;
		}
		else
		{
			currentNode = null;
		}
	}

	// A method to set the destination nodes for the vehicle
	public void PlotRoute(int destinationId)
	{
		Debug.Log(currentNode.id + " - " + destinationId);
		destinationNodes = network.FindShortestRoute(currentNode.id, destinationId);
	}
}
