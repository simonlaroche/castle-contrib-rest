using System.Collections;
using System.Collections.Generic;

namespace Castle.MonoRail.Rest
{
	// For selecting relevant Http verbs
	public static class MethodSelector
	{
		public static string GetMethodName(string action)
		{
			switch(action.ToUpper())
			{
				case "INDEX":
				case "SHOW":
					return "GET";
				case "CREATE":
					return "POST";
				case "UPDATE":
					return "PUT";
				case "DESTROY":
					return "DELETE";
				default:
					return string.Empty;
			}
		}

		// Associates known available actions with relevant Http verbs 
		public static string GetAllowedMethods(IDictionary actions)
		{
			var allowedMethods = new List<string> { "OPTIONS" };
			
			foreach (DictionaryEntry entry in actions)
			{
				var methodName = GetMethodName(entry.Key.ToString());
				
				if (!string.IsNullOrEmpty(methodName) && !allowedMethods.Contains(methodName))
					allowedMethods.Add(methodName);
			}

			return string.Join(",", allowedMethods.ToArray());
		}
	}
}
