using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Network : MonoBehaviour
{
	public static Network instance; //singleton

	public List<Node> nodes;
	public List<Segment> segments;
	public GameObject nodePrefab, segmentPrefab, vehiclePrefab;

	int nodeId = 0;
	int segmentId = 0;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		nodes = new List<Node>();
		segments = new List<Segment>();
	}

	public Node AddNode(Vector2 position)
	{
		Node node = Instantiate(nodePrefab, transform).GetComponent<Node>();
		nodes.Add(node);
		node.transform.position = position;
		node.id = nodeId + 1;
		node.gameObject.name = "Node #" + node.id;
		nodeId++;
		return node;
	}
	public void RemoveNode(int id)
	{
		Node node = nodes.Find(x => x.id == id);
		if (!node)
		{
			Debug.LogWarning("Node #" + id + " does not exist");
			return;
		}
		Segment[] connectedSegments = GetSegments(node);
		for (int i = 0; i < connectedSegments.Length; i++)
		{
			RemoveSegment(connectedSegments[i].id);
		}
		nodes.Remove(node);
		Destroy(node.gameObject);
	}
	public Node GetNode(int id)
	{
		Node node = nodes.Find(x => x.id == id);
		if (!node)
		{
			Debug.LogWarning("Node #" + id + " does not exist");
			return null;
		}
		return node;
	}

	public Segment[] GetSegments(Node node)
	{
		Segment[] connected = segments.FindAll(x => x.node2id == node.id || x.node1id == node.id).ToArray();
		if (connected.Length < 1)
		{
			Debug.LogWarning("Node #" + node.id + " has no connected segments");
		}
		return connected;
	}
	public int[] GetConnectedNodes(int nodeId)
	{
		Segment[] connectedSegments = GetSegments(GetNode(nodeId));
		int[] connected = new int[connectedSegments.Length];
		for (int i = 0; i < connected.Length; i++)
		{
			if (connectedSegments[i].node1id == nodeId)
			{
				connected[i] = connectedSegments[i].node2id;
			}
			else
			{
				connected[i] = connectedSegments[i].node1id;
			}
		}
		return connected.Distinct().ToArray();
	}
	public Segment GetSegment(int node1, int node2)
	{
		return segments.Where(segment => (segment.node1id == node1 && segment.node2id == node2) || (segment.node1id == node2 && segment.node2id == node1)).FirstOrDefault();
	}

	public Segment CreateSegment(Node node1, Node node2)
	{
		Segment segment = Instantiate(segmentPrefab, transform).GetComponent<Segment>();
		segments.Add(segment);
		segment.Initialize(node1, node2, segmentId + 1);
		segment.gameObject.name = "Segment #" + segment.id;
		segmentId++;
		return segment;
	}
	public void RemoveSegment(int id)
	{
		Segment segment = segments.Find(x => x.id == id);
		if (!segment)
		{
			Debug.LogWarning("Segment #" + id + " does not exist");
			return;
		}
		segments.Remove(segment);
		Destroy(segment.gameObject);
	}

	public Vehicle SpawnVehicle(Node node)
	{
		Vehicle vehicle = Instantiate(vehiclePrefab, transform).GetComponent<Vehicle>();
		vehicle.transform.position = node.transform.position;
		vehicle.currentNode = node;
		return vehicle;
	}


	public List<int> FindShortestRoute(int startNodeId, int endNodeId)
	{
		// Create a dictionary to store the distances from the start node to each node
		Dictionary<int, float> distances = new Dictionary<int, float>();
		// Initialize all distances to infinity
		foreach (Node node in nodes)
		{
			distances[node.id] = float.PositiveInfinity;
		}
		// Set the distance from the start node to itself to 0
		distances[startNodeId] = 0;

		// Create a priority queue to store the nodes that still need to be visited
		PriorityQueue<Node> queue = new PriorityQueue<Node>();
		// Add the start node to the queue
		queue.Enqueue(GetNode(startNodeId), 0);

		// Create a dictionary to store the previous node for each node
		Dictionary<int, int> previous = new Dictionary<int, int>();
		// Set the previous node for the start node to itself
		previous[startNodeId] = startNodeId;

		// While there are still nodes in the queue
		while (queue.Count > 0)
		{
			// Get the node with the smallest distance from the queue
			Node currentNode = queue.Dequeue();

			// If the current node is the end node, we have found the shortest route
			if (currentNode.id == endNodeId)
			{
				// Use the previous dictionary to reconstruct the shortest route
				List<int> route = new List<int>();
				int node = endNodeId;
				while (node != startNodeId)
				{
					route.Add(node);
					node = previous[node];
				}
				route.Add(startNodeId);
				// Reverse the route so it starts at the start node and ends at the end node
				route.Reverse();
				return route;
			}

			// Get the connected nodes to the current node
			int[] connectedNodes = GetConnectedNodes(currentNode.id);
			// For each connected node
			for (int i = 0; i < connectedNodes.Length; i++)
			{
				// Get the distance from the start node to the connected node through the current node (divide actual distance by speed limit, so the algorithm chooses the fastest route available)
				float distance = distances[currentNode.id] + (Vector2.Distance(currentNode.transform.position, GetNode(connectedNodes[i]).transform.position) / GetSegment(currentNode.id, connectedNodes[i]).speedLimit);
				// If the distance is smaller than the current distance from the start node to the connected node
				if (distance < distances[connectedNodes[i]])
				{
					// Update the distance and the previous node for the connected node
					distances[connectedNodes[i]] = distance;
					previous[connectedNodes[i]] = currentNode.id;
					// Add the connected node to the queue
					queue.Enqueue(GetNode(connectedNodes[i]), distance);
				}
			}
		}

		// If the end node was not reached, return an empty list
		return new List<int>();
	}

}