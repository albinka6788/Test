using System;

namespace BHIC.Common.XmlHelper
{
    public class XmlParserException : Exception
    {
        public XmlParserException() { }

        public XmlParserException(string message) : base(message) { }

        public XmlParserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
