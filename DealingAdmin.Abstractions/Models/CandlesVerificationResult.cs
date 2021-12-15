namespace DealingAdmin.Abstractions.Models
{
    public class CandlesVerificationResult
    {
        public int TotalCount { get; set; }

        public int ActualCount { get; set; }

        public int ScannedCount { get; set; }

        public int CorrespondingCount { get; set; }

        public int DifferingCount { get; set; }

        public int MissedCount { get; set; }
    }
}