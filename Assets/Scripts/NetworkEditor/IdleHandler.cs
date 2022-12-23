using UnityEngine;

public class IdleHandler : MonoBehaviour
{
    public void AddNode()
	{
		Node node = Network.instance.AddNode(new Vector2(0, 0));
		NetworkEditorHandler.instance.Select(node);
	}
}
