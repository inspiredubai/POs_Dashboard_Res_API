public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Status { get; set; } = "Failed";
    public string Message { get; set; } = string.Empty;
    public T? ReturnObject { get; set; }
}
