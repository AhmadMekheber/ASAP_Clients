namespace ASAP_Clients.Exceptions
{
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException() : base("Email already exists")
        {
        }
    }
}