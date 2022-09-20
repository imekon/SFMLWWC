namespace WWC
{
    internal interface IGameContainer<T>
    {
        string Title { get; }
        List<T> Items { get; }
    }
}
