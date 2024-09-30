namespace Core.DTO.ResponseDTO;

public class LoginResponse
{
    public string token { get; set; }
    public DateTime expiration { get; set; }
    public string authRole { get; set; }
    public string userName { get; set; }
}