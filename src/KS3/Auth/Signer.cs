namespace KS3.Auth
{
    public interface ISigner<T>
    {
        void Sign(IRequest<T> request, IKS3Credentials credentials);
    }
}
