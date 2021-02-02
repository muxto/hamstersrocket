namespace PriceTargets.Core.Domain
{
    public interface IOutput
    {
        void Publish();
        void Publish(string message);
        void PublishAtTheLine(string message);
    }
}
