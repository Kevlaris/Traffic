using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SegmentHandler : MonoBehaviour
{
	public TextMeshProUGUI segmentNameText, node1text, node2text;
	public TMP_InputField lanes, speedLimit;
	public Image directionButton;
	[SerializeField] Sprite[] directionSprites = new Sprite[3];
	Segment segment;
	Segment.Direction direction;

	private void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			NetworkEditorHandler.instance.Deselect();
		}
	}

	public void LoadSegment(Segment segment)
	{
		this.segment = segment;
		segmentNameText.text = "Segment #" + segment.id;
		node1text.text = "Node #" + segment.node1id;
		node2text.text = "Node #" + segment.node2id;
		direction = segment.direction;
		directionButton.sprite = directionSprites[(int)direction];

		lanes.text = segment.lanes.ToString();
		speedLimit.text = segment.speedLimit.ToString();
	}

	public void Close()
	{
		segment = null;
		NetworkEditorHandler.instance.Deselect();
	}

	public void SetLanes()
	{
		if (string.IsNullOrWhiteSpace(lanes.text)) return;
		segment.SetLanes(int.Parse(lanes.text));
	}
	public void SetSpeedLimit()
	{
		if (string.IsNullOrWhiteSpace(speedLimit.text)) return;
		segment.SetSpeedLimit(int.Parse(speedLimit.text));
	}
	public void CycleDirection()
	{
		segment.SetDirection(direction);
		int l = System.Enum.GetValues(typeof(Segment.Direction)).Length;

		if ((int)direction + 1 >= l)
		{
			direction = 0;
			segment.SetDirection(direction);
		}
		else
		{
			direction++;
			segment.SetDirection(direction);
		}

		directionButton.sprite = directionSprites[(int)direction];
	}
	public void DeleteSegment()
	{
		NetworkEditorHandler.instance.Remove(segment);
	}
}