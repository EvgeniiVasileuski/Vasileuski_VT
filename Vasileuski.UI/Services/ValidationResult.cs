namespace Vasileuski.UI.Services
{
    /// <summary>
    /// Результат валидации
    /// </summary>
    public class ValidationResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsValid => !Errors.Any();

        public void AddError(string error) => Errors.Add(error);
        public void AddErrors(IEnumerable<string> errors) => Errors.AddRange(errors);
    }
}