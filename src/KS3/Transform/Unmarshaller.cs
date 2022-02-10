namespace KS3.Transform
{
    public interface IUnmarshaller<X, Y>
    {
        X Unmarshall(Y input);
    }
}
