namespace Opus.Contracts
{
    public interface IJobItem
    {
        string Type { get; }
    }

    public class TyreReplacement : IJobItem
    {
        public string Type { get => Constants.JobItems.TyreReplacementTypeName; }
        public WheelPosition Position { get; set; }
        public int Size { get; set; }
    }

    public class BrakeDiscReplacement : IJobItem
    {
        public string Type { get => Constants.JobItems.BrakeDiscReplacementTypeName; }
        public WheelPosition Position { get; set; }
    }

    public class BrakePadReplacement : IJobItem
    {
        public string Type { get => Constants.JobItems.BrakePadReplacementTypeName; }
        public WheelPosition Position { get; set; }
    }

    public class ExhaustReplacement : IJobItem
    {
        public string Type { get => Constants.JobItems.ExhaustTypeName; }
    }

    public class OilChange : IJobItem
    {
        public string Type { get => Constants.JobItems.OilChangeTypeName; }
    }
}