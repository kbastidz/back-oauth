using db.Enum;

namespace db.Response
{
    public class RsTrxService
    {
        public StatusCode Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string DatoAdicional { get; set; }

        public RsTrxService() { }
        public RsTrxService(StatusCode status, int code, string message)
        {
            Status = status;
            Code = code;
            Message = message;
           
        }

        public RsTrxService(StatusCode status, int code, string message, string datoAdicional)
        {
            Status = status;
            Code = code;
            Message = message;
            DatoAdicional = datoAdicional;

        }

    }
}
