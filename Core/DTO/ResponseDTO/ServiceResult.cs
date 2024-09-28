using System.Net;


namespace Core.DTO.ResponseDTO;
public class ServiceResult<T>
{
    public T Data { get; private set; }
    public List<T> DataList { get; private set; }
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public HttpStatusCode StatusCode { get; private set; }
    public int TotalRecords { get; private set; }

    // Constructor for single item
    public ServiceResult(T data, bool success, string message, HttpStatusCode statusCode)
    { 
        Data = data;
        Success = success;
        Message = message;
        StatusCode = statusCode;
 
    }

    // Constructor for list of items
    public ServiceResult(List<T> dataList, bool success, string message, HttpStatusCode statusCode,int totalRecords)
    { 
        DataList = dataList;
        Success = success;
        Message = message;
        StatusCode = statusCode;
        TotalRecords = totalRecords;
    }

    // Constructor for general success/failure without data
    public ServiceResult(bool success, string message, HttpStatusCode statusCode)
    {
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }
}