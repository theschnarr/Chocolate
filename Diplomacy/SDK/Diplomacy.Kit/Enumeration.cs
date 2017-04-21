using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit
{
    /// <summary>
    /// Base abstract class for creating 'enum' style class with more functionality and ease of use.
    /// Taken from: https://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/
    /// </summary>
    public abstract class Enumeration : IComparable
    {
        /// <summary>
        /// Type: <see cref="Int32"/><para>The internal numeric value of the current enum.</para>
        /// </summary>
        private readonly int _value;
        /// <summary>
        /// Type: <see cref="String"/><para>The user friendly string containing a display name associated with the current value.</para>
        /// </summary>
        private readonly string _displayName;
        /// <summary>
        /// Default constructor.
        /// </summary>
        protected Enumeration()
        {
        }
        /// <summary>
        /// Constructor with the <paramref name="value"/> and <paramref name="displayName"/>.
        /// </summary>
        /// <param name="value">Type: <see cref="Int32"/><para>The numeric value to use for this enum.</para></param>
        /// <param name="displayName">Type: <see cref="String"/><para>The user friendly display string to use for this enum.</para></param>
        protected Enumeration(int value, string displayName)
        {
            _value = value;
            _displayName = displayName;
        }
        /// <summary>
        /// Type: <see cref="Int32"/><para>The numeric value associated with this enum.</para>
        /// </summary>
        public int Value
        {
            get { return _value; }
        }
        /// <summary>
        /// Type: <see cref="String"/><para>The user friendly display string associated with the <see cref="Enumeration.Value"/> of this enum.</para>
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
        }
        /// <summary>
        /// Converts the enum to a <see cref="String"/> representation.
        /// </summary>
        /// <returns>Type: <see cref="String"/><para>The display name of the enum.</para></returns>
        public override string ToString()
        {
            return DisplayName;
        }
        /// <summary>
        /// Gets all of the static "constant" enums that have been declared in this or child classes.
        /// </summary>
        /// <typeparam name="T">Any class that inherits <see cref="Enumeration"/>.</typeparam>
        /// <returns>Type: <see cref="IEnumerable{T}"/><para>The collection of all the "constant" enums that have been declared.</para></returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }
        /// <summary>
        /// Determines if the current instance is equal to the <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">Type: <see cref="object"/><para>The object to be compared with the current instance.</para></param>
        /// <returns>Type: <see cref="Boolean"/><para>Indicates if the comparison was a match or not.</para></returns>
        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }
        /// <summary>
        /// Gets the hash code for the current instance.
        /// </summary>
        /// <returns>Type: <see cref="Int32"/><para>The hash code associated with the current instance.</para></returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        /// <summary>
        /// Calculates the absolute difference between two <see cref="Enumeration"/> instances.
        /// </summary>
        /// <param name="firstValue">Type: <see cref="Enumeration"/><para>The first value in the difference comparison.</para></param>
        /// <param name="secondValue">Type: <see cref="Enumeration"/><para>The second value in the difference comparison.</para></param>
        /// <returns>Type: <see cref="Int32"/><para>The absolute difference between the two instances.</para></returns>
        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }
        /// <summary>
        /// Gets the <typeparamref name="T"/> instance associated with the <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">Any class which inherits from <see cref="Enumeration"/>.</typeparam>
        /// <param name="value">Type: <see cref="Int32"/><para>The numeric value to find the <typeparamref name="T"/> instance of.</para></param>
        /// <returns>Type: <typeparamref name="T"/><para>The instance associated with the <paramref name="value"/>.</para></returns>
        public static T FromValue<T>(int value) where T : Enumeration, new()
        {
            var matchingItem = parse<T, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }
        /// <summary>
        /// Gets the <typeparamref name="T"/> instance associated with the <paramref name="displayName"/>.
        /// </summary>
        /// <typeparam name="T">Any class which inherits from <see cref="Enumeration"/>.</typeparam>
        /// <param name="displayName">Type: <see cref="String"/><para>The user friendly display name to find the matching <typeparamref name="T"/> instance of.</para></param>
        /// <returns>Type: <typeparamref name="T"/><para>The instance associated with the <paramref name="displayName"/>.</para></returns>
        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }
        /// <summary>
        /// Parses the <typeparamref name="T"/> type for the first item that matches the <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">Any class inheriting from <see cref="Enumeration"/>.</typeparam>
        /// <typeparam name="K">The type of value to parse for.</typeparam>
        /// <param name="value">Type: <typeparamref name="K"/><para>The value to parse for.</para></param>
        /// <param name="description">Type: <see cref="String"/><para>The user friendly description of the type value being parsed for.</para></param>
        /// <param name="predicate">Type: <see cref="Func{T, bool}"/><para>The comparison method to use while parsing for a matching item.</para></param>
        /// <returns>Type: <typeparamref name="T"/><para>The matching item.</para></returns>
        private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }
        /// <summary>
        /// Compares this instance to the <paramref name="other"/> object.
        /// </summary>
        /// <param name="other">Type: <see cref="object"/><para>The value to compare against.</para></param>
        /// <returns>Type: <see cref="Int32"/><para>Indicates if this instance is less than (returns less than 0), equal to (returns 0), or greater than (returns greater than 0) the <paramref name="other"/>.</para></returns>
        public int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration)other).Value);
        }
    }
}
