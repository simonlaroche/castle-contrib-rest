using System.Collections;
using Castle.MonoRail.Framework;
using System.Linq;

namespace Castle.MonoRail.Rest
{
	public static class CrossOriginHeaders
	{
		public static void WriteToResponse(IRequest request, IResponse response, IDictionary actions)
		{
			try
			{
				response.AppendHeader("Access-Control-Allow-Methods", MethodSelector.GetAllowedMethods(actions));

				var configSection = MonoRailRestConfigSection.Instance;

				WriteAllowOrigin(request, response, configSection);
				WriteAllowHeaders(request, response, configSection);
				WriteAllowCredentials(response, configSection);
			}
			catch
			{
			}
		}

		private static void WriteAllowOrigin(IRequest request, IResponse response, MonoRailRestConfigSection configSection)
		{
			foreach (AllowOriginElement allowOrigin in configSection.AllowOriginList)
			{
				if (string.Compare(allowOrigin.Domain, request.Headers["Origin"], true) == 0)
				{
					response.AppendHeader("Access-Control-Allow-Origin", allowOrigin.Domain);
					break;
				}
			}
		}

		private static void WriteAllowHeaders(IRequest request, IResponse response, MonoRailRestConfigSection configSection)
		{
			var accessControlRequestHeaders = request.Headers.Get("Access-Control-Request-Headers");
			if (string.IsNullOrEmpty(accessControlRequestHeaders)) return;

			var allowedHeaders = (from AllowHeaderElement allowHeader in configSection.AllowHeaderList select allowHeader.Name).ToList();
			var matchedHeaders = string.Join(",", accessControlRequestHeaders.Split(',').Where(allowedHeaders.Contains).ToArray());

			response.AppendHeader("Access-Control-Allow-Headers", matchedHeaders);
		}

		private static void WriteAllowCredentials(IResponse response, MonoRailRestConfigSection configSection)
		{
			bool allowCredentials;

			if (!string.IsNullOrEmpty(configSection.AllowCredentials.Allow) && bool.TryParse(configSection.AllowCredentials.Allow, out allowCredentials))
				response.AppendHeader("Access-Control-Allow-Credentials", configSection.AllowCredentials.Allow);
		}
	}
}
