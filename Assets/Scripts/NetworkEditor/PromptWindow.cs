using UnityEngine;
using TMPro;

public class PromptWindow : MonoBehaviour
{
	public TextMeshProUGUI titleText, mainText, secondaryText;
	bool init = false;
	public bool answered { get; private set; } = false;
	public bool result { get; private set; }

	public void Initialize(string title, string main, string secondary)
	{
		if (init) return;
		titleText.text = title;
		mainText.text = main;
		secondaryText.text = secondary;
		init = true;
	}

	public void Agree()
	{
		if (!init) return;
		answered = true;
		result = true;
	}
	public void Disagree()
	{
		if (!init) return;
		answered = true;
		result = false;
	}
}