namespace Tasky.Shared;

public static class Messages
{
    public static class English
    {
        public const string Add =
            "add a new task/step informing the text and board. " +
            "If it is step, add the --step-of parameter informing the task id";

        public const string Begin = "Marks a task/step already created as begin informing the id and board";
        public const string Reset = "Marks a task/step already created as todo informing the id and board";
        public const string Done = "Marks a task/step already created as done informing the id and board";
        public const string List = "list all tasks";

        public const string TaskIdNotEmpty = "The task id cannot be empty.";
        public const string TaskIdEqualZero = "The task id cannot be '0'.";
        public const string TaskNotEmpty = "The task/step description cannot be empty.";
        public const string BoardNotEmpty = "The board name must be provided via the -b parameter";
    }
}