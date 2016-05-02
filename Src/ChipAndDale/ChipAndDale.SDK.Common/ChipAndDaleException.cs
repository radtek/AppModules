using System;

namespace ChipAndDale.SDK
{
    public class ChipAndDaleException: Exception
    {
        public ChipAndDaleException()
            : base()
        { }

        public ChipAndDaleException(string message)
            : base(message)
        { }

        public ChipAndDaleException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ChipAndDaleException(Exception innerException)
            : base("Помилка в програмі Чип та Дейл.", innerException)
        { }
    }
}
