namespace Core.Interfaces;

public interface ILog
{
    void Information(string message);
    void Warning(string message);
    void Debug(string message);
    void Error(string message);
    void Error(Exception message);
    void Error(Exception ex, string message, params object[] objs);
}