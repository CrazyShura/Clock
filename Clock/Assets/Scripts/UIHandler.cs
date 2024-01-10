using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
	#region Fields
	[SerializeField]
	GameObject alarmSetUpMenue, analogInput, digitalInput, clockMenue, analogClock, alarmWindow, timeOverwriteWindow;
	[SerializeField]
	AnalogHandle hourHand, minuteHand;
	[SerializeField]
	TMP_InputField hourInputField, minuteInputField;

	int hourInput, minuteInput;
	#endregion

	#region Methods
	private void Start()
	{
		hourHand.ValueChanged.AddListener(AnalogInputUpdate);
		minuteHand.ValueChanged.AddListener(AnalogInputUpdate);
		Clock.Instance.Alarm.AddListener(ShowAlarmWindow);
	}
	public void SwitchToDefaultView()
	{
		ResetInput();
		alarmSetUpMenue.SetActive(false);
		timeOverwriteWindow.SetActive(false);
		analogInput.SetActive(false);
		digitalInput.SetActive(false);
		clockMenue.SetActive(true);
		analogClock.SetActive(true);
	}
	public void SwitchToOverwrite()
	{
		ResetInput();
		alarmSetUpMenue.SetActive(false);
		timeOverwriteWindow.SetActive(true);
		analogInput.SetActive(true);
		digitalInput.SetActive(true);
		clockMenue.SetActive(false);
		analogClock.SetActive(false);
	}
	public void SwitchToAlarmSetUp()
	{
		ResetInput();
		alarmSetUpMenue.SetActive(true);
		timeOverwriteWindow.SetActive(false);
		analogInput.SetActive(true);
		digitalInput.SetActive(true);
		clockMenue.SetActive(false);
		analogClock.SetActive(false);
	}
	void ResetInput()
	{
		hourHand.transform.localRotation = Quaternion.Euler(0, 0, 0);
		minuteHand.transform.localRotation = Quaternion.Euler(0, 0, 0);
		hourInputField.text = "00";
		minuteInputField.text = "00";
	}

	public void DigitalInputUpdate()
	{
		try
		{
			hourInput = Mathf.Clamp(int.Parse(hourInputField.text), 0, 23);
		}
		catch
		{
			hourInput = 0;
		}
		try
		{
			minuteInput = Mathf.Clamp(int.Parse(minuteInputField.text), 0, 59);
		}
		catch
		{
			minuteInput = 0;
		}
		hourInputField.text = hourInput.ToString("00");
		minuteInputField.text = minuteInput.ToString("00");
		float _currentTime = (float)(hourInput * 60 * 60 + minuteInput * 60) / (float)Clock.TotalTimeInDay;
		hourHand.transform.localRotation = Quaternion.Euler(0, 0, -_currentTime * 360);
		minuteHand.transform.localRotation = Quaternion.Euler(0, 0, -_currentTime * 360 * 24);
	}
	public void AnalogInputUpdate(float unused)
	{
		hourInput = (int)Mathf.Clamp(hourHand.Value * 24f, 0, 24);
		if (hourInput == 24)
		{
			hourInput = 0;
		}
		minuteInput = (int)Mathf.Clamp(minuteHand.Value * 60f, 0, 60);
		if (minuteInput == 60)
		{
			minuteInput = 0;
		}
		hourInputField.text = hourInput.ToString("00");
		minuteInputField.text = minuteInput.ToString("00");
	}
	public void SetAlarm()
	{
		hourInputField.text = hourInput.ToString("00");
		minuteInputField.text = minuteInput.ToString("00");
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
	public void SetTimeOverwrite()
	{
		hourInputField.text = hourInput.ToString("00");
		minuteInputField.text = minuteInput.ToString("00");
		hourInput = int.Parse(hourInputField.text);
		minuteInput = int.Parse(minuteInputField.text);
		float _currentTime = hourInput * 60 * 60 + minuteInput * 60;
		Clock.Instance.OverwriteTime(_currentTime);
	}
	public void ClearOverwrite()
	{
		Clock.Instance.ClearTimeOverwrite();
	}
	#endregion
}
