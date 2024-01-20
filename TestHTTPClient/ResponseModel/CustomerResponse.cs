namespace TestHTTPClient.ResponseModel
{
    public class CustomerResponse<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }
}
