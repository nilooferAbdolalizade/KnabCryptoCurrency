namespace Domain.Entity
{
    public class ApiResponse
    {
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
    }
}
