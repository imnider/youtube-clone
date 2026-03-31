namespace YoutubeClone.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        // NotFoundExeption() { }
        public NotFoundException(string message) : base(message) { }
    }
}
