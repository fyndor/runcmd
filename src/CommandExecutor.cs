using System.Diagnostics;

namespace runcmd
{
    public static class CommandExecutor
    {
        public static void Execute(string[] args)
        {
            var argsSpan = new Span<string>(args, 1, args.Length - 1);
            var argStrings = GetArgStrings(argsSpan);
            var fileName = args[0];
            var commandArgs = argStrings.Length == 0 ? "" : $" {string.Join(" ", argStrings)}";
            Console.WriteLine($"Executing Command: {fileName}{commandArgs}");
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = commandArgs,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true,
            };
            try
            {
                proc.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string[] GetArgStrings(Span<string> args)
        {
            var argStrings = new string[args.Length];
            var index = 0;
            foreach (var arg in args)
            {
                argStrings[index] = arg.Contains(' ') ? $"\"{arg}\"" : arg;
                index++;
            }
            return argStrings;
        }
    }
}