namespace Application.Contracts.Response
{
    public class BaseResponse
    {
        public string Description { get; set; } = string.Empty;

        public StatusCode StatusCode { get; set; }
    }
}
