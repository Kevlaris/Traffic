using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VehicleSpawner : MonoBehaviour
{
	Network network;
	NetworkEditorHandler handler;

	[SerializeField] TextMeshProUGUI sourceText, destinationText;
	[SerializeField] Button spawnButton;
	Node source, destination;

	private void OnEnable()
	{
		ResetFields();
		network = Network.instance;
		handler = NetworkEditorHandler.instance;
	}

	private void Update()
	{
		if (!source || !destination)
		{
			spawnButton.interactable = false;
		}
		else
		{
			spawnButton.interactable = true;
		}

		if (Input.GetButtonDown("Cancel"))
		{
			if (handler.isSelecting)
			{
				handler.CancelSelection();
			}
		}
	}

	public void SpawnVehicle()
	{
		Vehicle vehicle = network.SpawnVehicle(source);
		vehicle.PlotRoute(destination.id);
		ResetFields();
	}

	void ResetFields()
	{
		sourceText.text = "Select Node";
		destinationText.text = "Select Node";

		source = null;
		destination = null;
	}

	public void SelectSource()
	{
		handler.StartSelection(true);
	}
	public void SelectDestination()
	{
		handler.StartSelection(false);
	}
	public void SetSource(Node node)
	{
		source = node;
		sourceText.text = "Node #" + node.id;
	}
	public void SetDestination(Node node)
	{
		destination = node;
		destinationText.text = "Node #" + node.id;
	}
}