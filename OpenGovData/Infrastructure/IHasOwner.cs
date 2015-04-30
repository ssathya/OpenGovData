namespace Infrastructure
{
    public interface IHasOwner<T> where T : class
    {
        T Owner { get; set; }
    }
}