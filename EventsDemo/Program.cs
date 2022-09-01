namespace EventsDemo
{
    /// <summary>
    /// This class subscribes to the publisher's event. When the publisher raises its event OnFinishedCounting,
    /// this subscriber gets notified and calls the HandleEvent method.
    /// </summary>
    internal class Subscriber
    {
        static void Main(string[] args)
        {
            Publisher.OnFinishedCounting += HandleFinishedCounting; // Subscribe to the event = "Do this when the event is raised!"
            Publisher.CountToN(3, 1);

            PublisherWithEventArguments.OnFinishedPickingRandomNumber += HandleFinishedPickingRandomNumber;
            PublisherWithEventArguments.PickRandomNumer(2);
        }

        static void HandleFinishedCounting(object? sender, EventArgs e) // The method that is called when the event is raised
        {
            Thread.Sleep(1000);
            Console.WriteLine("Subscriber >> I was notified that Publisher raised the event!");
        }
        static void HandleFinishedPickingRandomNumber(object? sender, CustomArgs e) // The method that is called when the event is raised
        {
            Thread.Sleep(1000);
            Console.WriteLine($"Subscriber >> I was notified that PublisherWithEventArguments finished picking a random number and chose {e.ChosenNumber}!");
        }
    }


    internal static class Publisher
    {
        public static event EventHandler? OnFinishedCounting; // Declare the event (aside note: EventHandler is a generic delegate)
        // We declare the event as nullable (?) because it might be that nobody subscribed to the event

        public static void CountToN(int n, int waitTime)
        {
            for (int i = 0; i < n; i++)
            {
                Thread.Sleep(1000 * waitTime);
                Console.WriteLine(i + 1);
            }

            Thread.Sleep(1000 * waitTime);
            Console.WriteLine("Publisher >> I have finished counting and now I am raising the event OnFinishedCounting!");
            OnFinishedCounting?.Invoke(null, EventArgs.Empty); // Raise the event = Invoke
            // We only want to raise the event if there are subscribors
            // => The ? operator only calls .Invoke(...) if OnFinishedCounting is not null
        }
    }

    internal static class PublisherWithEventArguments
    {
        public static event EventHandler<CustomArgs>? OnFinishedPickingRandomNumber; // Declare the event with the generic EventHandler<T> type
        // We declare the event as nullable (?) because it might be that nobody subscribed to the event

        public static void PickRandomNumer(int waitTime)
        {
            Thread.Sleep(1000 * waitTime);
            Console.Write("I am... ");

            Thread.Sleep(1000 * waitTime);
            Console.Write("choosing... ");

            Thread.Sleep(1000 * waitTime);
            Console.Write("a number... ");

            Random random = new();
            CustomArgs args = new(random.Next(10) + 1);

            Thread.Sleep(1000 * waitTime);
            Console.WriteLine("\nPublisherWithEventArguments >> I have picked a random number!");

            OnFinishedPickingRandomNumber?.Invoke(null, args); // Raise the event
        }
    }

    internal class CustomArgs : EventArgs // Declare a class for custom event arguments
    {
        public int ChosenNumber { get; set; }

        public CustomArgs(int chosenNumber)
        {
            ChosenNumber = chosenNumber;
        }
    }
}