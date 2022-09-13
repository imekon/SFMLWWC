using TinyMessenger;

namespace WWC
{
    internal class StatusMessage : TinyMessageBase
    {
        public string Status { get; }

        public StatusMessage(object sender, string message) : base(sender)
        {
            Status = message;
        }
    }
}
