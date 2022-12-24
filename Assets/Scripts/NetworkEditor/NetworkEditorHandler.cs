using System.Collections;
using UnityEngine;

public class NetworkEditorHandler : MonoBehaviour
{
	public static NetworkEditorHandler instance;

	public GameObject editPanel, idle, nodeEdit, segmentEdit, nodeSelectText;

	bool isEditing = false;
	public GameObject Editing { get; private set; }

	[Header("Segment Creation")]
	public bool isConnecting = false;
	Node node1;

	[Header("Prompt Window")]
	public GameObject promptWindowPrefab;
	PromptWindow prompt;

	private void Awake()
	{
		instance = this;
	}

	void Update()
	{
		if (!isEditing) return;

		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, LayerMask.GetMask("RoadNetwork", "UI"));
			if (hit.collider != null)
			{
				Debug.Log(hit.transform.gameObject.name);
				if (isConnecting)
				{
					if (hit.collider.GetComponent<Node>())
					{
						EndConnection(hit.collider.GetComponent<Node>());
					}
				}
				else
				{
					if (hit.collider.GetComponent<Node>())
					{
						Select(hit.collider.GetComponent<Node>());
					}
					else if (hit.collider.GetComponent<Segment>())
					{
						Select(hit.collider.GetComponent<Segment>());
					}
				}
			}
		}
	}

	public void ToggleEditor()
	{
		editPanel.SetActive(!isEditing);
		isEditing = !isEditing;
	}

	public void Select(Node node)
	{
		Editing = node.gameObject;
		nodeEdit.SetActive(true);
		idle.SetActive(false);
		segmentEdit.SetActive(false);

		nodeEdit.GetComponent<NodeHandler>().LoadNode(node);
	}
	public void Select(Segment segment)
	{
		Editing = segment.gameObject;
		nodeEdit.SetActive(false);
		idle.SetActive(false);
		segmentEdit.SetActive(true);

		segmentEdit.GetComponent<SegmentHandler>().LoadSegment(segment);
	}

	public void Deselect()
	{
		nodeEdit.SetActive(false);
		idle.SetActive(true);
		segmentEdit.SetActive(false);
		Editing = null;
	}

	public void StartConnection(Node node1)
	{
		this.node1 = node1;
		isConnecting = true;
		nodeSelectText.SetActive(true);
	}
	public void EndConnection(Node node2)
	{
		isConnecting = false;
		nodeSelectText.SetActive(false);
		Select(Network.instance.CreateSegment(node1, node2));
		node1 = null;
	}

	public void Remove(Node node)
	{
		isEditing = false;

		string title = "WARNING";
		string main = "You're about to delete Node #" + node.id;
		string secondary = "";
		Segment[] segments = Network.instance.GetSegments(node);
		if (segments.Length == 1)
		{
			secondary = "This will affect Segment #" + segments[0].id;
		}
		else if (segments.Length > 1)
		{
			secondary = "This will affect " + segments.Length + " Segments";
		}

		prompt = Instantiate(promptWindowPrefab, FindObjectOfType<Canvas>().transform).GetComponent<PromptWindow>();
		prompt.Initialize(title, main, secondary);
		StartCoroutine(Prompt(node));
	}
	IEnumerator Prompt(Node node)
	{
		yield return new WaitUntil(() => prompt.answered);
		if (prompt.result)
		{
			Deselect();
			Network.instance.RemoveNode(node.id);
		}
		Destroy(prompt.gameObject);
		isEditing = true;
	}

	public void Remove(Segment segment)
	{
		isEditing = false;

		string title = "WARNING";
		string main = "You're about to delete Segment #" + segment.id;

		prompt = Instantiate(promptWindowPrefab, FindObjectOfType<Canvas>().transform).GetComponent<PromptWindow>();
		prompt.Initialize(title, main, "");
		StartCoroutine(Prompt(segment));
	}
	IEnumerator Prompt(Segment segment)
	{
		yield return new WaitUntil(() => prompt.answered);
		if (prompt.result)
		{
			Deselect();
			Network.instance.RemoveSegment(segment.id);
		}
		Destroy(prompt.gameObject);
		isEditing = true;
	}
}