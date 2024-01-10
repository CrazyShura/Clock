using UnityEngine;
using UnityEngine.Events;

public class AnalogHandle : MonoBehaviour
{
	#region Fields
	bool follow = false;
	Camera mainCamera;
	UnityEvent<float> valueChanged = new UnityEvent<float>();
	#endregion

	#region Properties
	public float Value
	{
		get
		{
			return 1 - transform.localRotation.eulerAngles.z / 360;
		}
	}
	public UnityEvent<float> ValueChanged { get => valueChanged; }
	#endregion

	#region Methods
	private void Start()
	{
		mainCamera = Camera.main;
	}
	private void Update()
	{
		if (follow)
		{
			Vector3 _targ = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			_targ.z = 0f;
			_targ.x = _targ.x - transform.position.x;
			_targ.y = _targ.y - transform.position.y;

			float angle = Mathf.Atan2(_targ.y, _targ.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
			float _res = 1 - transform.localRotation.eulerAngles.z / 360;
			valueChanged.Invoke(_res);
		}
	}
	private void OnMouseDown()
	{
		follow = true;
	}
	private void OnMouseUp()
	{
		if (follow)
		{
			follow = false;
			float _res = 1 - transform.localRotation.eulerAngles.z / 360;
			valueChanged.Invoke(_res);
		}
	}
	#endregion
}
