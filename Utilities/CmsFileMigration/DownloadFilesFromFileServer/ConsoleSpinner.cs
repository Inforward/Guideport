using System;

namespace CmsFileMigration.DownloadFilesFromFileServer
{
    public class ConsoleSpinner
    {
        int _counter;

        public ConsoleSpinner()
        {
            _counter = 0;
        }

        public void Turn()
        {
            _counter++;

            Console.CursorVisible = false;

            switch (_counter % 4)
            {
                case 0: Console.Write("/"); break;
                case 1: Console.Write("-"); break;
                case 2: Console.Write("\\"); break;
                case 3: Console.Write("-"); break;
            }

            try
            {
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
            catch { }
        }
    } 
}
