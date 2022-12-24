using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class NodeHandler : MonoBehaviour
{
    public TextMeshProUGUI nodeNameText;
    public TMP_InputField x, y;
    public TMP_Dropdown typeDropdown;

	Node node;

	public GameObject moveNodeText;
	bool isMoving;
	Vector2 originalPos;
	Vector2 originalMousePos;

	private void Update()
	{
		if (Input.GetButtonDown("Move") && !isMoving)
		{
			isMoving = true;
			originalPos = node.transform.position;
			originalMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;
		}
		if (Input.GetButtonDown("Cancel"))
		{
			if (isMoving)
			{
				isMoving = false;
				node.transform.position = originalPos;
				originalPos = Vector2.zero;
				originalMousePos = Vector2.zero;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			else if (NetworkEditorHandler.instance.isConnecting)
			{
				NetworkEditorHandler.instance.CancelConnection();
			}
			else
			{
				NetworkEditorHandler.instance.Deselect();
			}
		}

		moveNodeText.SetActive(isMoving);

		if (isMoving)
		{
			Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 delta = currentMousePos - originalMousePos;
			Vector2 newPos = (Vector2)node.transform.position + (delta * 2.5f);
			//snap when SHIFT is down
			node.SetPosition(newPos);
			originalMousePos = currentMousePos;
			if (Input.GetMouseButtonDown(0))
			{
				isMoving = false;
				originalPos = Vector2.zero;
				originalMousePos = Vector2.zero;
				x.text = node.transform.position.x.ToString();
				y.text = node.transform.position.y.ToString();
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}

	public void LoadNode(Node node)
	{
		this.node = node;
		nodeNameText.text = "Node #" + node.id;

		List<string> options = System.Enum.GetNames(typeof(Node.NodeType)).ToList();

		typeDropdown.ClearOptions();
		typeDropdown.AddOptions(options);

		x.text = node.transform.position.x.ToString();
		y.text = node.transform.position.y.ToString();
		typeDropdown.value = (int)node.nodeType;
	}

	public void Close()
	{
		node = null;
		NetworkEditorHandler.instance.Deselect();
	}

	public void SetX()
	{
		if (string.IsNullOrWhiteSpace(x.text)) return;
		node.SetX(float.Parse(x.text));
	}
	public void SetY()
	{
		if (string.IsNullOrWhiteSpace(y.text)) return;
		node.SetY(float.Parse(y.text));
	}
	public void SetType()
	{
		node.SetType((Node.NodeType)typeDropdown.value);
	}
	public void DeleteNode()
	{
		NetworkEditorHandler.instance.Remove(node);
	}
	public void StartConnection()
	{
		NetworkEditorHandler.instance.StartConnection(node);
	}
}