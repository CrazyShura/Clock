using System;
using System.Globalization;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton
/// </summary>
public class Clock : MonoBehaviour
{
	#region Fields
	static Clock instance;

	[SerializeField]
	Transform uiParent, analogParent;
	[SerializeField, Range(1, 10)]
	float timeSpeed = 1f;

	float currentTime, alarmTime = -1f;
	int lastHour, lastMinute, lastSecond, lastTime;
	const int totalTimeInDay = 60 * 60 * 24;
	bool timeOverwriten = false;
	UnityEvent timeChanged = new UnityEvent();
	UnityEvent alarm = new UnityEvent();
	UnityEvent<float> alarmSetUp = new UnityEvent<float>();

	#endregion

	#region Properties
	public static Clock Instance { get => instance; }
	public UnityEvent TimeChanged { get => timeChanged; }
	public UnityEvent Alarm { get => alarm; }
	public UnityEvent<float> AlarmSetUp { get => alarmSetUp; }
	public float CurrentTime { get => currentTime; }
	/// <summary>
	/// Alarm is set if this values is above or equal 0;
	/// </summary>
	public float AlarmTime { get => alarmTime; }
	public int LastHour { get => lastHour; }
	public int LastMinute { get => lastMinute; }
	public int LastSecond { get => lastSecond; }
	public static int TotalTimeInDay => totalTimeInDay;
	public bool TimeOverwriten { get => timeOverwriten; set => timeOverwriten = value; }
	#endregion

	#region Methods
	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
	}
	private void Start()
	{
		ReadWebTime();
	}
	private void FixedUpdate()
	{
		currentTime += Time.fixedDeltaTime * timeSpeed;
		CheckTime();
	}
	void CheckTime()
	{
		if ((int)currentTime != lastTime)
		{
			lastTime = (int)currentTime;
			lastSecond++;
			if (lastSecond == 60)
			{
				lastMinute++;
				lastSecond = 0;
				if (lastMinute == 60)
				{
					lastHour++;
					lastMinute = 0;
					if (lastHour == 24)
					{
						lastHour = 0;
						currentTime = 0;
					}
					if (!timeOverwriten)
					{
						ReadWebTime();
					}
				}
			}
			if (alarmTime >= 0 && (int)alarmTime == lastTime + 1)
			{
				alarm.Invoke();
			}
			timeChanged.Invoke();
		}
	}
	void ReadWebTime()
	{
		DateTime _dateTime = GetWebTime();
		lastHour = _dateTime.Hour;
		lastMinute = _dateTime.Minute;
		lastSecond = _dateTime.Second;
		currentTime = lastHour * 60 * 60 + lastMinute * 60 + lastSecond;
	}
	DateTime GetWebTime()
	{
		WebResponse _response = null;
		DateTime _res;
		try
		{
			_response = WebRequest.Create("http://www.google.com").GetResponse();
		}
		catch (WebException)
		{
			Debug.Log("Shit gone wrong yo");
			try
			{
				_response = WebRequest.Create("http://www.yahoo.com").GetResponse();
			}
			catch (WebException)
			{
				Debug.Log("Shit gone super wrong yo");
				_response = null;
			}
		}
		if (_response != null)
		{
			_res = DateTime.ParseExact(_response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
		}
		else
		{
			_res = DateTime.Now;
		}
		_response.Close();
		return _res;
	}
	public void OverwriteTime(float time)
	{
		if (time >= 0 && time < totalTimeInDay)
		{
			currentTime = time;
			lastHour = (int)currentTime / (60 * 60);
			lastMinute = ((int)currentTime % (60 * 60))/60;
			lastSecond = 0;
			timeOverwriten = true;
		}
		else
		{
			Debug.LogError("Trying to overwrite time wiht incorrect value. Passed vlaue = " + time);
		}
	}
	public void ClearTimeOverwrite()
	{
		timeOverwriten = false;
		ReadWebTime();
	}
	public void SetAlarm(float time)
	{
		if (time >= 0 && time < totalTimeInDay)
		{
			alarmTime = time;
			alarmSetUp.Invoke(alarmTime);
		}
		else
		{
			Debug.LogError("Trying to set up an alarm wiht incorrect value. Passed vlaue = " + time);
		}
	}
	public void ResetAnAlarm()
	{
		alarmTime = -1f;
		alarmSetUp.Invoke(alarmTime);
	}
	#endregion
}
