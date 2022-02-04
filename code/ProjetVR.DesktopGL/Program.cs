using System;

namespace ProjetVR.DesktopGL
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
