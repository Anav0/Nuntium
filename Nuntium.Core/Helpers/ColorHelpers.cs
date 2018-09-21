
using System;

namespace Nuntium.Core
{
    public static class ColorHelpers
    {
        private static Random mRnd = new Random();

        public static string GenerateRandomColor()
        {
            return String.Format("#{0:X6}", mRnd.Next(0x1000000));
        }
    }
}
