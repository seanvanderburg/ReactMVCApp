namespace Paginator{
    public class Page<T>{
        public int Index {get;set;}
        public T[] Items {get;set;}
        public int TotalPages {get;set;}
    }
}