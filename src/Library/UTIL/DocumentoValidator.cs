using System;
using System.Collections.Generic;
using System.Text;

namespace Library.UTIL
{
    public class DocumentoValidator
    {
        public static bool IsValid(string doc) => CnpjValidator.IsValid(doc) || CpfValidator.IsValid(doc);
    }
}
