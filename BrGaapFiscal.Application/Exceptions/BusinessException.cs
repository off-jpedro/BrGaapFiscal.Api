namespace BrGaapFiscal.Application.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string businnesException) : base(businnesException)
        {
            
        }
    }
}
