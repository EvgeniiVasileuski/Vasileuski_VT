namespace Vasileuski.UI.Services
{
    /// <summary>
    /// Устаревший класс, используйте ServiceResponse<T>
    /// </summary>
    public class ResponseData<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
    }
}