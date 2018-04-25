using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Tile
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Common.AñadeALog("Comienza ejecución...", "Tile.exe");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new VTile());
                Common.AñadeALog("Finalizado correctamente.", "Tile.exe");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Common.AñadeALog($"Finalizado con error: {e.Message}.", "Tile.exe");
            }
        }
    }
}
