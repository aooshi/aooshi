using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi
{
    /// <summary>
    /// “Ï≥£
    /// </summary>
    [Serializable]
    public class AooshiException:System.Exception
    {
        /// <summary>
        /// initialize exception
        /// </summary>
        /// <param name="message">exception message</param>
        public AooshiException(string message) : base(message) { }
        /// <summary>
        /// initialize exception
        /// </summary>
        /// <param name="message">exception message</param>
        /// <param name="exception">procreant exception</param>
        public AooshiException(string message,Exception exception) : base(message,exception) { }
    }
}
