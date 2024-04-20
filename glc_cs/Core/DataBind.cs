using System;
using System.Linq;
using static glc_cs.Core.Property;

namespace glc_cs.Core
{
	internal class DataBind
	{
		public static string[] StatusDropDown()
		{
			string[] result = new string[Enum.GetValues(typeof(StatusType)).Cast<int>().Max() + 1];
			int i = 0;
			foreach (string name in Enum.GetNames(typeof(StatusType)))
			{
				result[i] = name;
				i++;
			}
			return result;
		}
	}
}
