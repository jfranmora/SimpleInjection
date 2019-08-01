using UnityEngine;

namespace SimpleInjection
{
	[DefaultExecutionOrder(-31000)]
	public class AutoInjector : MonoBehaviour
	{
		private void Awake()
		{
			foreach (var component in GetComponents<MonoBehaviour>())
			{
				SI.Inject(component);
			}
		}
	}
}