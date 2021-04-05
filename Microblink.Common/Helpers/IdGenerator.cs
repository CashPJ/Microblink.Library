using System;

namespace Microblink.Common.Common.Helpers
{
    public static class IdGenerator
    {
        /// <summary>
        /// Generates unique random integer.
        /// <para>Value can be positive or negative number.</para>
        /// </summary>
        /// <returns>Unique random integer</returns>
        public static int GetId()
        {
            return Guid.NewGuid().ToString().GetHashCode();
        }
    }
}
