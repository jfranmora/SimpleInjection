using UnityEngine;

namespace SimpleInjection
{
	[DefaultExecutionOrder(-31000)]
	public class MonoInjector : MonoBehaviour
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