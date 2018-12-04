using System.Windows;
using System.Windows.Documents;

namespace Nuntium
{
    public static class ButtonStateHelper
    {

        /// <summary>
        /// Checkes property at given position, 
        /// returnes true if <paramref name="testedProperty"/> if equal to <paramref name="expectedValue"/>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="testedProperty"></param>
        /// <param name="expectedValue"></param>
        /// <returns></returns>
        public static bool CheckDependencyPropertyState(TextPointer position, DependencyProperty testedProperty, object expectedValue)
        {
            object currentValue = position.Parent.GetValue(testedProperty);

            if (currentValue == null || currentValue == DependencyProperty.UnsetValue)
                return false;

            return currentValue.Equals(expectedValue);
        }


       
    }
}
