using Core.Interfaces;

namespace Infrastructure;

public class LoggerFake: ILog
{
    public void Information(string message)
    {
         
    }

    public void Warning(string message)
    {
      
    }

    public void Debug(string message)
    {
       
    }

    public void Error(string message)
    {
         
    }

    public void Error(Exception message)
    {
        
    }

    public void Error(Exception ex, string message, params object[] objs)
    {
       
    }
}