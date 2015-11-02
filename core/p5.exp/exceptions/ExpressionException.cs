/*
 * Phosphorus Five, copyright 2014 - 2015, Thomas Hansen, phosphorusfive@gmail.com
 * Phosphorus Five is licensed under the terms of the MIT license, see the enclosed LICENSE file for details
 */

using System;
using p5.core;

/// <summary>
///     Main namespace for exceptions within the phosphorus.expressions project.
/// 
///     Contains the exceptions that "phosphorus.expressions" might throw.
/// </summary>
namespace p5.exp.exceptions
{
    /// <summary>
    ///     Exception thrown when expressions contains syntax errors.
    /// 
    ///     Contains some helper logic to see Hyperlisp StackTrace, among other things, in addition to which expression
    ///     that fissled in your code, and why.
    /// </summary>
    public class ExpressionException : ArgumentException
    {
        private readonly string _expression;
        private readonly string _message;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpressionException" /> class.
        /// </summary>
        /// <param name="expression">Expression that caused exception.</param>
        public ExpressionException (string expression)
            : this (expression, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpressionException" /> class.
        /// </summary>
        /// <param name="expression">Expression that caused exception.</param>
        /// <param name="message">Message providing additional information.</param>
        /// <param name="node">Node where expression was found.</param>
        /// <param name="context">Application context. Necessary to perform conversion from p5.lambda to Hyperlisp to show Hyperlisp StackTrace.</param>
        public ExpressionException (string expression, string message)
        {
            _message = message;
            _expression = expression;
        }

        /*
         * overriding Message to provide expression that malfunctioned as an additional piece of contextual information
         */
        public override string Message
        {
            get
            {
                var retVal = string.Format ("Expression '{0}' is not a valid expression.", _expression);
                if (_message != null)
                    retVal = _message + "\r\n" + retVal;
                return retVal;
            }
        }
    }
}