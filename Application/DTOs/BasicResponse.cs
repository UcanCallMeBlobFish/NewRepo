namespace Application.DTOs
{
    public class BasicResponse
    {
        public bool Success;
        public string Message;
        public BasicResponse(bool suc, string Mes)
        {
            Success = suc;
            Message = Mes;
        }
    }
}
