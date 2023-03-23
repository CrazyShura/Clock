using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
	#region Fields
	[SerializeField]
	GameObject alarmSetUpMenue, analogSetup, clockMenue, analogClock, alarmWindow;
	[SerializeField]
	AnalogHandle alarmHourSetupHand, alarmMinuteSetupHand;
	[SerializeField]
	TMP_InputField hourInputField, minuteInputField;

	int hourInput, minuteInput;
	#endregion

	#region Methods
	private void Start()
	{
		alarmHourSetupHand.ValueChanged.AddListener(AnalogInputUpdate);
		alarmMinuteSetupHand.ValueChanged.AddListener(AnalogInputUpdate);
		Clock.Instance.Alarm.AddListener(ShowAlarmWindow);
	}
	public void SwitchMenue()
	{
		if (alarmSetUpMenue.activeSelf)
		{
			alarmSetUpMenue.SetActive(false);
			analogSetup.SetActive(false);
			clockMenue.SetActive(true);
			analogClock.SetActive(true);
		}
		else
		{
			alarmSetUpMenue.SetActive(true);
			analogSetup.SetActive(true);
			clockMenue.SetActive(false);
			analogClock.SetActive(false);
		}
	}
	public void DigitalInputUpdate()
	{
		hourInput = Mathf.Clamp(int.Parse(hourInputField.text), 0, 23);
		minuteInput = Mathf.Clamp(int.Parse(minuteInputField.text), 0, 59);
		float _currentTime = (float)(hourInput * 60 * 60 + minuteInput * 60) / (float)Clock.TotalTimeInDay;
		alarmHourSetupHand.transform.localRotation = Quaternion.Euler(0, 0, -_currentTime * 360);
		alarmMinuteSetupHand.transform.localRotation = Quaternion.Euler(0, 0, -_currentTime * 360 * 24);
	}
	public void AnalogInputUpdate(float unused)
	{
		hourInput = (int)Mathf.Clamp(alarmHourSetupHand.Value * 24f, 0, 24);
		if (hourInput == 24)
		{
			hourInput = 0;
		}
		minuteInput = (int)Mathf.Clamp(alarmMinuteSetupHand.Value * 60f, 0, 60);
		if (minuteInput == 60)
		{
			minuteInput = 0;
		}
		hourInputField.text = hourInput.ToString("00");
		minuteInputField.text = minuteInput.ToString("00");
	}
	public void SetAlarmAndSwitchMenue()
	{
		hourInput = int.Parse(hourInputField.text);
		minuteInput = int.Parse(minuteInputField.text);
		float _currentTime = hourInput * 60 * 60 + minuteInput * 60;
		Clock.Instance.SetAlarm(_currentTime);
	}
	public void SnoozAlarm()
	{
		Clock.Instance.SetAlarm(Clock.Instance.CurrentTime + 60 * 5);
	}
	public void ClearAlarm()
	{
		Clock.Instance.ResetAnAlarm();
	}
	public void ShowAlarmWindow()
	{
		alarmWindow.SetActive(true);
	}
	#endregion
}
