namespace SimpleInjection
{
	public class DependencyNotResolvedException : SimpleInjectionException
	{
		public DependencyNotResolvedException() : base() { }
		public DependencyNotResolvedException(string message) : base(message) { }
	}
}