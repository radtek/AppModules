using System;

namespace Core.SDK.Db
{
    public class DbBaseException : Exception
    {
        public DbBaseException() : base() { }
        public DbBaseException(string message) : base(message) { }
        public DbBaseException(string message, Exception inner) : base(message, inner) { }
    }



    public class NoDataFoundDbException : DbBaseException
    {
        public NoDataFoundDbException() : base() { }
        public NoDataFoundDbException(string message) : base(message) { }
        public NoDataFoundDbException(string message, Exception inner) : base(message, inner) { }
    }

    public class TooManyRowsDbException : DbBaseException
    {
        public TooManyRowsDbException() : base() { }
        public TooManyRowsDbException(string message) : base(message) { }
        public TooManyRowsDbException(string message, Exception inner) : base(message, inner) { }
    }

    public class NotLogedOnDbException : DbBaseException
    {
        public NotLogedOnDbException() : base() { }
        public NotLogedOnDbException(string message) : base(message) { }
        public NotLogedOnDbException(string message, Exception inner) : base(message, inner) { }
    }

    public class UniqueConstraintDbException : DbBaseException
    {
        public UniqueConstraintDbException() : base() { }
        public UniqueConstraintDbException(string message) : base(message) { }
        public UniqueConstraintDbException(string message, Exception inner) : base(message, inner) { }
    }

    public class ConnectDbException : DbBaseException
    {
        public ConnectDbException() : base() { }
        public ConnectDbException(string message) : base(message) { }
        public ConnectDbException(string message, Exception inner) : base(message, inner) { }
    }

    public class TransactionDbException : DbBaseException
    {
        public TransactionDbException() : base() { }
        public TransactionDbException(string message) : base(message) { }
        public TransactionDbException(string message, Exception inner) : base(message, inner) { }
    }

    public class UnexpectedDbException : DbBaseException
    {
        public UnexpectedDbException() : base() { }
        public UnexpectedDbException(string message) : base(message) { }
        public UnexpectedDbException(string message, Exception inner) : base(message, inner) { }
    }
}
