using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Tile
{
    public class Parrilla
    {
        public int AnchoVentanaPx { get; set; }
        public int AltoVentanaPx { get; set; }
        public int Filas { get; set; }
        public int Columnas { get; set; }
        public string ColorPrimerPlanoEnNotacionHTML { get; set; }
        public string ColorFondoEnNotacionHTML { get; set; }
        public string Fuente { get; set; }
        public float TamFuentePt { get; set; }
        public List<Opcion> Opciones { get; set; }

        private Label CreaBarraDeEstado(
                     string foreNameColorRGB,
                     string backNameColorRGB,
                     string font,
                     float size)
        {
            return new Label
            {
                BackColor = Common.FromRGBString(backNameColorRGB),
                ForeColor = Common.FromRGBString(foreNameColorRGB),
                Font = new Font(font, size / 2, FontStyle.Regular, GraphicsUnit.Point),
                AutoSize = false,
                Dock = DockStyle.Fill,
                Name = $"status",
                Text = "",
                TabStop = false,
                TextAlign = ContentAlignment.TopRight
            };
        }

        private void DistribuyeLayoutPorporcionalmente(TableLayoutPanel parrilla)
        {
            const float PORENTEJE_ALTO_STATUSBAR = 5;

            float porcentajeAltoFila = (100 - PORENTEJE_ALTO_STATUSBAR) / Filas;
            float porcentajeAnchoColumna = 100 / Columnas;

            for (int row = 0; row < Filas; row++)
            {
                parrilla.RowStyles.Add(new RowStyle(SizeType.Percent, porcentajeAltoFila));
                for (int col = 0; col < Columnas; col++)
                {
                    parrilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, porcentajeAnchoColumna));
                }
            }
            parrilla.RowStyles.Add(new RowStyle(SizeType.Percent, PORENTEJE_ALTO_STATUSBAR));
        }

        public TableLayoutPanel CreaLayout()
        {
            TableLayoutPanel parrilla = new TableLayoutPanel
            {
                RowCount = Filas + 1, // Añadimos una fila para el StatusBar.
                ColumnCount = Columnas,
                Name = "tableLayoutPanel",
                Dock = DockStyle.Fill,
                Location = new Point(0, 0),
                BackColor = Common.FromRGBString(ColorFondoEnNotacionHTML),
                ForeColor = Common.FromRGBString(ColorPrimerPlanoEnNotacionHTML),
                TabStop = false
            };
            DistribuyeLayoutPorporcionalmente(parrilla);

            // Inizializa y añade barra de estado a parrilla.
            Label barraDeEstado = CreaBarraDeEstado(
                    ColorPrimerPlanoEnNotacionHTML,
                    ColorFondoEnNotacionHTML,
                    Fuente, TamFuentePt);
            parrilla.Controls.Add(barraDeEstado, 0, Filas);
            parrilla.SetColumnSpan(barraDeEstado, Columnas);

            // Inizializa y añade opciones a parrilla.
            foreach (Opcion o in Opciones)
                o.AñadeALayout(
                    parrilla,
                    barraDeEstado,
                    ColorPrimerPlanoEnNotacionHTML,
                    ColorFondoEnNotacionHTML,
                    Fuente, TamFuentePt);

            return parrilla;
        }

        public void Añade(Opcion o)
        {
            if (o.Fila >= Filas || o.Columna >= Columnas)
                throw new ArgumentException($"La opción a añadir en ({o.Fila},{o.Columna}), está fuera de la parrilla de dimensiones ({Filas},{Columnas}).");
            Opciones.Add(o);
        }

        public void SerializaXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Parrilla));
            TextWriter writer = new StreamWriter(Common.CONFIG_FILE);
            serializer.Serialize(writer, this);
            writer.Close();
        }

        static public Parrilla DeserializaXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Parrilla));
            FileStream reader = new FileStream(Common.CONFIG_FILE, FileMode.Open);
            return (Parrilla)serializer.Deserialize(reader);
        }

        public Parrilla()
        {
            ;
        }

        public Parrilla(
                    int widthPx, int heightPx,
                    int rows, int columns,
                    string foreNameColorRGB,
                    string backNameColorRGB,
                    string font,
                    float size)
        {
            AnchoVentanaPx = widthPx;
            AltoVentanaPx = heightPx;
            Filas = rows;
            Columnas = columns;
            ColorPrimerPlanoEnNotacionHTML = foreNameColorRGB;
            ColorFondoEnNotacionHTML = backNameColorRGB;
            Fuente = font;
            TamFuentePt = size;
            Opciones = new List<Opcion>();
        }

    }
}
