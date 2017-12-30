﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Exceptions
{
    /// <summary>
    /// This interface represents an exception handler
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// Attempts to perform an exception handling
        /// </summary>
        /// <param name="ex"></param>
        Task HandleAsync<TException>(TException ex)
            where TException : Exception;
    }
}
