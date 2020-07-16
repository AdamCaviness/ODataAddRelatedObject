using System;
using System.Collections.Generic;

namespace TestOData
{
	[Serializable]
	public class PathSegment
	{
		public Dictionary<string, Guid> Keys { get; set; }
		public string EntitySetName { get; set; }
		public string NavigationPropertyName { get; set; }
		public bool IsCollection { get; set; }
		public bool Contained { get; set; }
		public bool Instance { get; set; }
	}
}