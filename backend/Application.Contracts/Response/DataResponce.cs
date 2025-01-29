namespace Application.Contracts.Response
{
    public class DataResponse<T> : BaseResponse
    {
        public T Data { get; set; }
    }
}
