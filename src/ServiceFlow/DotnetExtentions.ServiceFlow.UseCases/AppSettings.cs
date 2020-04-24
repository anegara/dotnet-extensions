namespace DotnetExtentions.ServiceFlow.UseCases
{
    public class AppSettings
    {
        public int CounterStartsAt { get; set; }

        public static string CounterKey => "counter";
    }
}
