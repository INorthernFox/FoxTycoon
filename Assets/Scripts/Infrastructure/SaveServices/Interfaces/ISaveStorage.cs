namespace Infrastructure.SaveServices.Interfaces
{
    public interface ISaveStorage
    {
        public void Write(string data);
        public string Read();
    }

}