namespace Vasileuski.UI.Services
{
    /// <summary>
    /// Общий класс для ответов сервисов
    /// </summary>
    /// <typeparam name="T">Тип данных</typeparam>
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static ServiceResponse<T> Ok(T data, string? message = null)
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ServiceResponse<T> Error(string errorMessage)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}