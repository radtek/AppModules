using System;
using Core.SDK.Dom;
using ChipAndDale.SDK.Nsi;

namespace ChipAndDale.SDK.Common
{
    public class MessageEntity : EntityBase<MessageEntity>
    {
        public MessageEntity()
            : base()
        { }


        #region fields

        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private DateTime _CreateDate;
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }

        private UserEntity _Sender;
        public UserEntity Sender
        {
            get { return _Sender; }
            set { _Sender = value; }
        }

        private UserEntity _Receiver;
        public UserEntity Receiver
        {
            get { return _Receiver; }
            set { _Receiver = value; }
        }

        private string _Subject;
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        private string _Body;
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }


        private MessageChannelType _channel;
        public MessageChannelType Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        private MessageState _State;
        public MessageState State
        {
            get { return _State; }
            set { _State = value; }
        }


        private long _AttemptCount;
        public long AttemptCount
        {
            get { return _AttemptCount; }
            set { _AttemptCount = value; }
        }

        #endregion fields


        #region helper property

        public int ChannelId
        {
            get { return (int)Channel; }
        }

        #endregion helper property


        #region Object override

        public override MessageEntity Clone()
        {
            MessageEntity result = new MessageEntity();
            result.Id = Id;
            result.Key = Key;
            result.CreateDate = CreateDate;
            result.Sender = Sender.Clone();
            result.Receiver = Receiver.Clone();
            result.Subject = Subject;
            result.Body = Body;
            result.Channel = Channel;
            result.State = State;
            result.AttemptCount = AttemptCount;
            return result;
        }


        public override bool Equals(MessageEntity other)
        {
            if (other == null) return false;

            if (object.ReferenceEquals(this, other)) return true;

            return (string.Equals(Id, other.Id) &&
                     string.Equals(Key, other.Key) &&
                     Sender.Equals(other.Sender) &&
                     Receiver.Equals(other.Receiver) &&
                     string.Equals(Subject, other.Subject) &&
                     string.Equals(Body, other.Body)) &&
                     Channel == other.Channel &&
                     State == other.State &&
                     CreateDate == other.CreateDate;
        }

        public override bool Equals(object obj)
        {
            MessageEntity message = obj as MessageEntity;
            if (message == null) return false;
            else return Equals(message);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToInternalString()
        {
            return string.Format("Message ({0} - {1}) from {2} to {3}", Id, CreateDate.ToString("dd.MM.yyyy HH:mm:ss"), Sender.Name, Receiver.Name);
        }

        public override string ToString()
        {
            return Subject;
        }
        #endregion


        public static MessageEntity Create()
        {
            return new MessageEntity();
        }
    }
}
