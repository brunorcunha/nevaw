using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public class IStoppableCaracIdSelectorUtils
	{
		public static IStoppableCaracIdSelector FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject val = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			JToken val2 = default(JToken);
			if (!val.TryGetValue("type", ref val2))
			{
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class IStoppableCaracIdSelector");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			if (text == "StoppableCaracIdDirectSelector")
			{
				IStoppableCaracIdSelector stoppableCaracIdSelector = new StoppableCaracIdDirectSelector();
				stoppableCaracIdSelector.PopulateFromJson(val);
				return stoppableCaracIdSelector;
			}
			Debug.LogWarning((object)("Unknown type: " + text));
			return null;
		}

		public static IStoppableCaracIdSelector FromJsonProperty(JObject jsonObject, string propertyName, IStoppableCaracIdSelector defaultValue = null)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			JProperty val = jsonObject.Property(propertyName);
			if (val == null || (int)val.get_Value().get_Type() == 10)
			{
				return defaultValue;
			}
			return FromJsonToken(val.get_Value());
		}
	}
}
