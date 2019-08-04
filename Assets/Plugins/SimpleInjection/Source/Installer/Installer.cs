using UnityEngine;

namespace SimpleInjection
{
	[DefaultExecutionOrder(-32000)]
	public abstract class Installer : MonoBehaviour
	{
		private void Awake()
		{
			Install();
		}

		protected abstract void Install();
	}
}