using TMPro;
using UnityEngine;

public class ClockGraphics : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Transform hourHand, minuteHand , alarmHand;
	[SerializeField]
	TMP_Text digitalDisplay;
	#endregion

	#region Methods
	private void Start()
	{
		Clock.Instance.TimeChanged.AddListener(UpdateClock);
		Clock.Instance.TimeChanged.AddListener(UpdateDisplay);
		Clock.Instance.AlarmSetUp.AddListener(UpdateAlarmHand);
	}
	void UpdateDisplay()
	{
		string _newDesplay = $"{Clock.Instance.LastHour:00}:{Clock.Instance.LastMinute:00}:{Clock.Instance.LastSecond:00}";
		digitalDisplay.text = _newDesplay;
	}
	void UpdateClock()
	{
		float _currentTime = Clock.Instance.CurrentTime / Clock.TotalTimeInDay;
		hourHand.transform.localRotation = Quaternion.Euler(0, 0,  - _currentTime * 360 * 2);
		minuteHand.transform.localRotation = Quaternion.Euler(0, 0,  - _currentTime * 360 * 24);
	}
	void UpdateAlarmHand(float time)
	{
		if(time>0)
		{
			alarmHand.gameObject.SetActive(true);
			float _currentTime = time / Clock.TotalTimeInDay;
			alarmHand.transform.localRotation = Quaternion.Euler(0, 0, -_currentTime * 360 * 2);
		}
		else
		{
			alarmHand.gameObject.SetActive(false);
		}
	}
	#endregion
}
