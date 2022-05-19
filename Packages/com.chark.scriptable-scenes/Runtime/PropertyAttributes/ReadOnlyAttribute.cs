using UnityEngine;

namespace CHARK.ScriptableScenes.PropertyAttributes
{
    /// <summary>
    /// Marks an Editor field as read only.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    internal sealed class ReadOnlyAttribute : PropertyAttribute
    {
    }
}
