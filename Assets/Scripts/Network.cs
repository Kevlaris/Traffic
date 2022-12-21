using UnityEngine;

[ExecuteInEditMode]
public class Network : MonoBehaviour
{
	public Node[] nodes;
	public GameObject nodePrefab;

	private void Reset()
	{
		for (int i = 0; i < nodes.Length; i++)
		{
			nodes[i] = Instantiate(nodePrefab).GetComponent<Node>();
			InitializeNode(nodes[i]);
		}
	}

	void InitializeNode(Node node)
	{
		node.gameObject.name = "Node " + node.id;
	}
}
