using System;
using ChipAndDale.SDK;

namespace ChipAndDale.Main
{
    internal class AuthorizateExceptions : ChipAndDaleException
    {
        public AuthorizateExceptions(Exception innerException)
            : base("Помилка при авторизації.", innerException)
        { }
    }

    internal class NsiRefreshExceptions : ChipAndDaleException
    {
        public NsiRefreshExceptions(Exception innerException)
            : base("Помилка при оновленні довідників НСІ.", innerException)
        { }
    }
}
