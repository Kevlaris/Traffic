using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour
{
	public static Network instance; //singleton

	public List<Node> nodes;
	public List<Segment> segments;
	public GameObject nodePrefab, segmentPrefab;

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
		/*
		for (int i = 0; i < 5; i++)
		{
			nodes.Add(Instantiate(nodePrefab, transform).GetComponent<Node>());
			nodes[i].transform.position = new Vector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5));
			nodes[i].id = nodeId + 1;
			nodes[i].gameObject.name = "Node #" + nodes[i].id;
			nodeId++;
		}
		*/
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
}
