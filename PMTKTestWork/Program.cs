// See https://aka.ms/new-console-template for more information
using PTMKTestWork;


if (args.Length < 0)
{
  Console.WriteLine("world!");
  return;
}



int task = 0;
int.TryParse(args[0], out task);
ITaskRunner taskRunner = new TaskRunner();

TaskRunner.RunTask(taskRunner, task, args);

