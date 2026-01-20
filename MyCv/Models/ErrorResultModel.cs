using MyCv.Models;

namespace MyCv.ViewModels
{
    public class ErrorResultModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
