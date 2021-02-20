using System;
using System.Runtime.Serialization;
using Common.Core.Extensions;
using Common.Model.Enums;

namespace Common.Core.Exceptions
{
    public class LogicException : Exception
    {

        public LogicException()
        {
        }

        public LogicException(
            ExceptionMessage message)
            : base(message.ToDescription())
        {
            MessageType = message;
        }

        public LogicException(
            string message)
            : base(message)
        {
        }

        protected LogicException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public ExceptionMessage? MessageType { get; }
    }
}