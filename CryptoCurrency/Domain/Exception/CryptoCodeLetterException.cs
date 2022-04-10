namespace Domain.Exception
{
    public class CryptoCodeLetterException : BusinessException
    {
        public CryptoCodeLetterException() : base(1003)
        {
        }

        public override string GetTranslate()
        {
            return ExceptionResource.Exception_1003;
        }
    }
}
