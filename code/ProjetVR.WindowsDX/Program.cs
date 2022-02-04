using System;

namespace ProjetVR.WindowsDX
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ProjetVRGame())
                game.Run();
        }
    }
}
