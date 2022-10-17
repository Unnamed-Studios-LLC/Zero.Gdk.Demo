public interface IDataHandler<T> where T : unmanaged
{
    void Process(ref T data);
}