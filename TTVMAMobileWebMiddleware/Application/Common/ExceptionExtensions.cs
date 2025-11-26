using System;

namespace TTVMAMobileWebMiddleware.Application.Common;

public static class ExceptionExtensions
{
    public static TException WithHelpLink<TException>(this TException exception, string helpLink)
        where TException : Exception
    {
        exception.HelpLink = helpLink;
        return exception;
    }
}

