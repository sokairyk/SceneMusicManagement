using System;

namespace SokairykFramework.Logger
{
    public interface ILogger
    {
        void LogError(string message, Exception ex);
        void LogWarning(string message);
        void LogInfo(string message);
    }
}
