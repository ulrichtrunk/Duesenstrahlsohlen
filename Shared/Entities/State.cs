using NPoco;

namespace Shared.Entities
{
    [TableName("States")]
    public class State : BaseEntity<int>
    {
        public string Name { get; set; }

        public const string NameEnqueued = "Enqueued";
        public const string NameRunning = "Running";
        public const string NameCancelling = "Cancelling";
        public const string NameCancelled = "Cancelled";
        public const string NameError = "Error";
        public const string NameDone = "Done";
        public const string NameDeleting = "Deleting";

        public static readonly string[] ValidExecuteStates = new string[]
        {
            NameCancelled
        };

        public static readonly string[] ValidDeleteStates = new string[]
        {
            NameCancelled,
            NameError,
            NameDone
        };

        public static readonly string[] ValidCancelStates = new string[]
        {
            NameEnqueued,
            NameRunning
        };

        public static readonly string[] ValidCSVExportStates = new string[]
        {
            NameDone
        };

        public static readonly string[] ValidShowImageStates = new string[]
        {
            NameDone
        };
    }
}
