using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CircleAreaDefinition : IEditableContent, IAreaDefinition
	{
		private int m_radius;

		public int radius => m_radius;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static CircleAreaDefinition FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject jsonObject = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			CircleAreaDefinition circleAreaDefinition = new CircleAreaDefinition();
			circleAreaDefinition.PopulateFromJson(jsonObject);
			return circleAreaDefinition;
		}

		public static CircleAreaDefinition FromJsonProperty(JObject jsonObject, string propertyName, CircleAreaDefinition defaultValue = null)
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

		public void PopulateFromJson(JObject jsonObject)
		{
			m_radius = Serialization.JsonTokenValue<int>(jsonObject, "radius", 1);
		}

		public Area ToArea(Vector2Int position)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (m_radius == 0)
			{
				return new PointArea(position);
			}
			return new CircleArea(position, m_radius);
		}
	}
}
