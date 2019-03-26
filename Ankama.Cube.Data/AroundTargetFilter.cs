using Ankama.Cube.Fight.Entities;
using Ankama.Utilities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class AroundTargetFilter : IEditableContent, ICoordOrEntityFilter, ICoordFilter, ITargetFilter, IEntityFilter
	{
		private ITargetSelector m_targetsToCompare;

		private ValueFilter m_distance;

		public ITargetSelector targetsToCompare => m_targetsToCompare;

		public ValueFilter distance => m_distance;

		public override string ToString()
		{
			return $"distance to {m_targetsToCompare} {distance}";
		}

		public static AroundTargetFilter FromJsonToken(JToken token)
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
			AroundTargetFilter aroundTargetFilter = new AroundTargetFilter();
			aroundTargetFilter.PopulateFromJson(jsonObject);
			return aroundTargetFilter;
		}

		public static AroundTargetFilter FromJsonProperty(JObject jsonObject, string propertyName, AroundTargetFilter defaultValue = null)
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
			m_targetsToCompare = ITargetSelectorUtils.FromJsonProperty(jsonObject, "targetsToCompare");
			m_distance = ValueFilter.FromJsonProperty(jsonObject, "distance");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			List<Area> areas = ListPool<Area>.Get();
			areas.AddRange(ZoneAreaFilterUtils.TargetsToCompareAreaList(targetsToCompare, context));
			int areaCount = areas.Count;
			foreach (IEntity entity in entities)
			{
				IEntityWithBoardPresence entityWithBoardPresence = entity as IEntityWithBoardPresence;
				if (entityWithBoardPresence != null)
				{
					for (int i = 0; i < areaCount; i++)
					{
						if (distance.Matches(entityWithBoardPresence.area.MinDistanceWith(areas[i]), context))
						{
							yield return entity;
							break;
						}
					}
				}
			}
			ListPool<Area>.Release(areas);
		}

		public IEnumerable<Coord> Filter(IEnumerable<Coord> coords, DynamicValueContext context)
		{
			List<Area> areas = ListPool<Area>.Get();
			areas.AddRange(ZoneAreaFilterUtils.TargetsToCompareAreaList(targetsToCompare, context));
			int areaCount = areas.Count;
			foreach (Coord coord in coords)
			{
				Vector2Int other = default(Vector2Int);
				other._002Ector(coord.x, coord.y);
				for (int i = 0; i < areaCount; i++)
				{
					if (distance.Matches(areas[i].MinDistanceWith(other), context))
					{
						yield return coord;
						break;
					}
				}
			}
			ListPool<Area>.Release(areas);
		}
	}
}
