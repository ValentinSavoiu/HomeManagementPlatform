﻿using System.ComponentModel.DataAnnotations;
using System;
using System.Reflection;

namespace mss_project.Helpers
{
	public static class EnumHelpers
	{
		public static string DisplayName(this Enum value)
		{
			Type enumType = value.GetType();
			string enumValue = Enum.GetName(enumType, value);
			MemberInfo member = enumType.GetMember(enumValue)[0];

			object[] attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
			string outString = ((DisplayAttribute)attrs[0]).Name;

			if (((DisplayAttribute)attrs[0]).ResourceType != null)
			{
				outString = ((DisplayAttribute)attrs[0]).GetName();
			}

			return outString;
		}
	}
}

