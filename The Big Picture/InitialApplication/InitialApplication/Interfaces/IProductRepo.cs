namespace InitialApplication.Interfaces
{
    public interface IProductRepo<T,K>
    {
        public T Add(T item);
        public Task<T> Update(T item);
        public Task<T> Delete(K key);
        public Task<T> Get(K key);
        public ICollection<T> GetAll();
    }
}
