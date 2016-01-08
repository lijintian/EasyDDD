using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyDDD.Infrastructure.Crosscutting.Helpers
{
    /// <summary>
    /// String extesion methods
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Formats a string. Similar to String.Format()
        /// </summary>
        /// <param name="target">string to format</param>
        /// <param name="args">Arguments to replace in the string.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatWith(this string target, params object[] args)
        {
            Check.Argument.IsNotNullOrEmpty(target, "target");
            return String.Format(target, args);
        }
    }
}
