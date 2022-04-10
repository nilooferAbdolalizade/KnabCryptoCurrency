namespace Domain.Exception
{
    public class CryptoCodeLengthException : BusinessException
    {
        public CryptoCodeLengthException(): base(1002)
        {
        }

        public override string GetTranslate()
        {
            return ExceptionResource.Exception_1002;
        }
    }
}
