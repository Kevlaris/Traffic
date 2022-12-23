using UnityEngine;

public class Segment : MonoBehaviour
{
	public int id { get; private set; }
	public int node1id, node2id;
	public Direction direction;
	public int lanes, speedLimit;

	LineRenderer lineRenderer;
	bool init = false;

	public enum Direction
	{
		TwoWay,
		OneToTwo,
		TwoToOne
	}

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	public void Initialize(Node node1, Node node2, int id)
	{
		this.id = id;
		node1id = node1.id;
		node2id = node2.id;
		direction = Direction.TwoWay;
		lanes = 2;
		speedLimit = 50;
		init = true;
		UpdateLine();
	}

	public void UpdateLine()
	{
		if (!init) return;
		lineRenderer.SetPosition(0, Network.instance.GetNode(node1id).transform.position);
		lineRenderer.SetPosition(1, Network.instance.GetNode(node2id).transform.position);
	}

	public void SetLanes(int lanes)
	{
		this.lanes = lanes;
		//logic
	}
	public void SetSpeedLimit(int limit)
	{
		speedLimit = limit;
		//logic
	}
	public void SetDirection(Direction direction)
	{
		this.direction = direction;
		//logic
	}
}
