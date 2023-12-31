namespace DirectShared;

public class MessageQueueEventArg<TData> : EventArgs
{
    public TData Data { get; set; }

    public MessageQueueEventArg(TData data)
    {
        Data = data;
    }
}