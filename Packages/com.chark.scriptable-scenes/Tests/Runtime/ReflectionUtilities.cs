using System;
using System.Reflection;

namespace CHARK.ScriptableScenes.Tests
{
    internal static class ReflectionUtilities
    {
        #region Private Fields

        private const BindingFlags PrivateFieldBindingFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Set private field value with give name on this object.
        /// </summary>
        internal static void SetField(this object obj, string name, object value)
        {
            if (obj.TryGetField(name, out var field))
            {
                field.SetValue(obj, value);
            }
        }

        /// <returns>
        /// Value named <paramref name="name"/> of type <typeparamref name="T"/> on
        /// <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// If value could not be retrieved.
        /// </exception>
        internal static T GetValue<T>(this object obj, string name)
        {
            if (obj.TryGetValue<T>(name, out var value))
            {
                return value;
            }

            throw new InvalidOperationException(
                $"{obj} does not have a field of type {typeof(T)} named \"{name}\""
            );
        }

        /// <returns>
        /// <c>true</c> if a <paramref name="value"/> of type <typeparamref name="T"/> is retrieved
        /// from <paramref name="obj"/> or <c>false</c> otherwise.
        /// </returns>
        internal static bool TryGetValue<T>(this object obj, string name, out T value)
        {
            value = default;

            if (obj.TryGetField(name, out var rawField) == false)
            {
                return false;
            }

            var rawValue = rawField.GetValue(obj);
            if (rawValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            return false;
        }

        /// <returns>
        /// <c>true</c> if <paramref name="field"/> is retrieved from <paramref name="obj"/> or
        /// <c>false</c> otherwise.
        /// </returns>
        internal static bool TryGetField(this object obj, string name, out FieldInfo field)
        {
            var type = obj.GetType();

            FieldInfo info;
            do
            {
                info = type.GetField(name, PrivateFieldBindingFlags);
            } while (info == null && (type = type.BaseType) != null);

            field = info;
            return info != null;
        }

        #endregion
    }
}
