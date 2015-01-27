namespace Portal.Web.Models
{
    public class JsonResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
    }
}