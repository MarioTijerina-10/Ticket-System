using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace TicketsApp.Helper_Classes;

public class EnumHelper
{
    public static string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attribute = field?.GetCustomAttribute<DisplayAttribute>();

        return attribute?.Name ?? value.ToString();
    }
}
