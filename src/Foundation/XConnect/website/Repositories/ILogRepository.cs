using System;

namespace Helixbase.Foundation.XConnect.Repositories
{
    public interface ILogRepository
    {
        void Debug(string message, Exception t);
        void Debug(string message);
        void Debug(string message, params object[] args);

        void Error(string message, Exception t);
        void Error(string message);

        void Info(string message, Exception t);
        void Info(string message);
        void Info(string message, params object[] args);

        void Warn(string message, Exception t);
        void Warn(string message);
        void Fatal(string message, Exception t);
    }
}
