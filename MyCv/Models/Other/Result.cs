namespace MyCv.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public Error Error { get; set; }

        public T Data { get; set; }
    }

}
