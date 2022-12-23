using UnityEngine;

public class Node : MonoBehaviour
{
	public int id;
	public NodeType nodeType { get; private set; } = NodeType.Junction; //default to Junction type

	public enum NodeType
	{
		Junction,
		Middle,
		Source,
		Destination
	}

	public void SetPosition(Vector2 pos)
	{
		transform.position = pos;
		UpdateConnectedSegments();
	}
	public void SetX(float x)
	{
		transform.position = new Vector2(x, transform.position.y);
		UpdateConnectedSegments();
	}
	public void SetY(float y)
	{
		transform.position = new Vector2(transform.position.x, y);
		UpdateConnectedSegments();
	}

	public void SetType(NodeType newType)
	{
		this.nodeType = newType;
		//add logic
	}

	void UpdateConnectedSegments()
	{
		Segment[] segments = Network.instance.GetSegments(this);

		if (segments.Length < 1) return;
		foreach (Segment segment in segments)
		{
			segment.UpdateLine();   // update all connected segments to new position
		}
	}
}
