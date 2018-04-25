using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Tile
{
    public class Opcion
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string RutaAImagen { get; set; }
        public string Comando { get; set; }
        public bool EsperarHastaFin { get; set; }
        public string DirectorioDeEjecucion { get; set; }

        private Label CreaEtiquetaNombre(
                    string foreNameColorRGB,
                    string backNameColorRGB,
                    string font,
                    float size)
        {
            return new Label
            {
                BackColor = Common.FromRGBString(backNameColorRGB),
                ForeColor = Common.FromRGBString(foreNameColorRGB),
                Font = new Font(font, size, FontStyle.Regular, GraphicsUnit.Point),
                AutoSize = true,
                Dock = DockStyle.Fill,
                Name = $"label{Fila}{Columna}",
                TabStop = false,
                Text = Nombre,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private Button CreaBotonImagen(Label statusBar)
        {
            Button imageButton = new Button()
            {
                BackgroundImage = Image.FromFile(RutaAImagen),
                BackgroundImageLayout = ImageLayout.Stretch,
                Name = $"button{Fila}{Columna}",
                FlatStyle = FlatStyle.Flat,                
                Dock = DockStyle.Fill,
                TabStop = false
            };
            imageButton.Click += (s, e) =>
            {
                string fileName;
                string arguments;

                if (Common.IAmOnLinux())
                {
                    fileName = "/bin/bash";
                    arguments = "-c ";
                }
                else if (Common.IAmOnWindows())
                {
                    fileName = "cmd.exe";
                    arguments = "/c ";
                }
                else
                    throw new PlatformNotSupportedException("Plataforma de ejecución no soportada.");
                arguments += $"\"{Comando}\"";

                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        FileName = fileName,
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        WorkingDirectory = DirectorioDeEjecucion,
                        UseShellExecute = false
                    }
                };
                statusBar.Text = $"Ejecutando {Nombre}...";
                process.Start();
                Common.AñadeALog(process.StandardOutput.ReadToEnd(), Comando);
                if (EsperarHastaFin)
                {
                    process.WaitForExit();
                    statusBar.Text = "Ejecución finalizada.";
                }
                else
                    statusBar.Text = "Ejecución finalizada.";
            };

            imageButton.MouseEnter += (s, e) => statusBar.Text = Descripcion;
            imageButton.MouseLeave += (s, e) => statusBar.Text = "";

            return imageButton;
        }

        public Opcion()
        {
            ;
        }

        public Opcion(
                    string name,
                    string description,
                    string imagePath,
                    string command,
                    bool waitTillEnd,
                    string worlingDirectory,
                    int row, int col)
        {
            Nombre = name;
            Descripcion = description;
            RutaAImagen = imagePath;
            Comando = command;
            EsperarHastaFin = waitTillEnd;
            DirectorioDeEjecucion = worlingDirectory;
            Fila = row;
            Columna = col;
        }

        public void AñadeALayout(
                    TableLayoutPanel layout,
                    Label statusBar,
                    string foreNameColorRGB,
                    string backNameColorRGB,
                    string font,
                    float size)
        {
            Button botonImagen = CreaBotonImagen(statusBar);
            Label eiquetaNombre = CreaEtiquetaNombre(
                    foreNameColorRGB,
                    backNameColorRGB,
                    font,
                    size);

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
            {
                RowCount = 2,
                ColumnCount = 1,
                Name = $"tableLayoutPanel{Fila}{Columna}",
                Dock = DockStyle.Fill,
                Location = new Point(0, 0),
                BackColor = Common.FromRGBString(backNameColorRGB),
                ForeColor = Common.FromRGBString(foreNameColorRGB),
                TabStop = false
            };

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel.Controls.Add(eiquetaNombre, 0, 0);
            tableLayoutPanel.Controls.Add(botonImagen, 0, 1);

            layout.Controls.Add(tableLayoutPanel, Columna, Fila);
        }
    }
}
