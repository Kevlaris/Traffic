using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	// The current node that the vehicle is at
	public Node currentNode;
	public Segment currentSegment;
	// A list of nodes that the vehicle needs to visit
	public List<int> destinationNodes;
	public float maxSpeed = 120f;
	float speed = 0;
	float desiredSpeed;
	public float acceleration = 10f;

	Collider2D c;
	Rigidbody2D rb;

	Network network;

	private void Awake()
	{
		// Find the Network script in the scene and store a reference to it
		network = Network.instance;
		c = GetComponent<Collider2D>();
		rb = GetComponent<Rigidbody2D>();
		transform.rotation = Quaternion.Euler(0, 0, 0);
		transform.forward = transform.right;
	}

	private void FixedUpdate()
	{
		if (destinationNodes.Count < 1)
		{
			Destroy(gameObject);    // Delete vehicle when it has reached its destination
		}

		// If the vehicle has reached the next node, remove from list
		if (Vector2.Distance(transform.position, currentNode.transform.position) < 0.1f)
		{
			destinationNodes.RemoveAt(0);
		}
		// If the vehicle has more nodes to visit, move to the next one
		if (destinationNodes.Count > 0)
		{
			Node nextNode = network.GetNode(destinationNodes[0]);
			// Find the segment connecting the current node and the next node
			currentSegment = network.GetSegment(currentNode.id, nextNode.id);
			/*
			Collider2D[] colliders = new Collider2D[1];
			c.OverlapCollider(new ContactFilter2D { layerMask = LayerMask.GetMask("Segment"), useLayerMask = true }, colliders);
			segment = colliders[0]?.transform.GetComponent<Segment>();
			*/

			desiredSpeed = maxSpeed;
			if (currentSegment != null)
			{
				desiredSpeed = Mathf.Min(maxSpeed, currentSegment.speedLimit);
			}

			// Accelerate / decelerate towards desired speed
			if (speed < desiredSpeed)
			{
				if (speed + 1f < desiredSpeed)
				{
					speed += acceleration * Time.fixedDeltaTime;
				}
			}
			else if (speed > desiredSpeed)
			{
				if (speed - 1f > desiredSpeed)
				{
					speed -= acceleration * Time.fixedDeltaTime;
				}
			}

			Vector3 nodePos = nextNode.transform.position;
			nodePos.z = 0f;

			// Calculate the direction to the next node
			//Vector2 direction = nodePos - transform.position;
			// Move the vehicle in the direction of the next node
			//transform.position += transform.forward * speed * 0.01f * Time.fixedDeltaTime;

			transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);

			// Face next node
			Vector3 objectPos = transform.position;
			nodePos.x -= objectPos.x;
			nodePos.y -= objectPos.y;
			float angle = Mathf.Atan2(nodePos.y, nodePos.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle);
			rb.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed * Time.fixedDeltaTime;	// Split angle into X and Y

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
		destinationNodes = network.FindShortestRoute(currentNode.id, destinationId);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.forward * speed);
	}
}
