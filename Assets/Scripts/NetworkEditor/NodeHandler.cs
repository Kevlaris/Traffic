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