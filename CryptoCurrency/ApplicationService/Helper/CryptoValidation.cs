using Application.Dto;
using Domain.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationService.Helper
{
    internal class CryptoValidation
    {
        private static void ValidateCryptoCode(string code, List<string> message)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrWhiteSpace(code))
            {
                message.Add(new CryptoCodeLengthException().GetTranslate());
            }
            if (code.Trim().Length != 3)
            {
                message.Add(new CryptoCodeLengthException().GetTranslate());
            }
            if (!code.All(char.IsUpper))
            {
                message.Add(new CryptoCodeLetterException().GetTranslate());
            }
        }

        private static LatestQuoteDto GenerateInvalidResponse(string code, List<string> messages)
        {
            return new LatestQuoteDto()
            {
                Status = new Status()
                {
                    ErrorCode = 1,
                    ErrorMessage = messages
                },
                Data = new List<Data>() { new Data() { Symbol = code } }
            };
        }

        private static LatestQuoteDto GenerateValidResponse(List<Data> dataList)
        {
            return new LatestQuoteDto()
            {
                Data = dataList,
                Status = new Status()
                {
                    ErrorCode = 0,
                    ErrorMessage = null
                }
            };
        }

    }
}
