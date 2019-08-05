using NUnit.Framework;

namespace SimpleInjection.Tests
{
    public class InjectorTests
    {
		[Test]
		public void InjectorTestDependencyNotResolved()
		{
			Assert.Throws(typeof(DependencyNotResolvedException), () => SI.Inject(new ClassA()));
		}

		[Test]
		public void InjectorTestOptionalDependencyNotResolved()
		{
			SI.Inject(new ClassB());
		}

		private class ClassA
		{
			[Inject] ISomeInterface someValue;
		}

		private class ClassB
		{
			[Inject(optional = true)] ISomeInterface someValue;
		}

		private interface ISomeInterface
		{

		}
    }
}
