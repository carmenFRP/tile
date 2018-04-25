using System.Drawing;
using System.Windows.Forms;

namespace Tile
{
    public partial class VTile : Form
    {
        private void InicializaVentana()
        {
            Parrilla layout = Parrilla.DeserializaXML();

            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(layout.AnchoVentanaPx, layout.AltoVentanaPx);
            this.Controls.Add(layout.CreaLayout());
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "VTile";
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public VTile()
        {
            this.SuspendLayout();
            InicializaVentana();
            this.ResumeLayout(false);
        }
    }
}
