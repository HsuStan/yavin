using System;

namespace Yavin.Core.Infrastructure
{
	public class AttributeInfo<T>
	{
		public T Attribute { get; set; }

		public Type DecoratedType { get; set; }
	}
}
