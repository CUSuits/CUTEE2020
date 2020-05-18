using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadialGauge : MonoBehaviour
{
	public Image LoadingBar;
	public float currentValue;
	public TextMeshProUGUI percentage;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		LoadingBar.fillAmount = currentValue / 200;
		if (currentValue<=50f && currentValue>25f)
		{
			LoadingBar.color = Color.yellow;
		}
		else if (currentValue<=25f)
		{
			LoadingBar.color = Color.red;
		}
		else
		{
			LoadingBar.color = Color.green;
		}
		percentage.text = (int) currentValue + "%";
	}
}