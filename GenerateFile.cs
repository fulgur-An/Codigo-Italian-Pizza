using Backend.Contracts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianPizza
{
    public class GenerateFile
    {
        public void MakeInventoryReport(Uri filename, List<ItemContract> items)
        {
            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);
            // Indicamos donde vamos a guardar el documento
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(filename.OriginalString, FileMode.Create));

            // Abrimos el archivo
            doc.Open();
            Paragraph Enterprise = new Paragraph();
            Enterprise.Font = FontFactory.GetFont(FontFactory.HELVETICA, 18f, BaseColor.BLACK);
            Enterprise.Add("Italian pizza S.A de C.V");
            doc.Add(Enterprise);
            doc.Add(new Paragraph("C/Principal, 5"));
            doc.Add(new Paragraph("12345 Municipio (Provincia)"));
            doc.Add(new Paragraph("Fecha"));
            doc.Add(new Paragraph("Empleado"));
            doc.Add(new Paragraph("No de página"));

            PdfPTable table = new PdfPTable(7);

            table = MakeTableInventory();
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            FillInventoryTtable(table, items);
            doc.Add(table);
            doc.Close();
            doc.OpenDocument();
        }

        private PdfPTable MakeTableInventory()
        {
            PdfPTable table = new PdfPTable(7);

            Font fontHeader = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.WHITE);

            Phrase phraseEntry = new Phrase()
            {
                Font = fontHeader
            };
            phraseEntry.Add("Entrada");
            PdfPCell cellEntry = new PdfPCell(phraseEntry)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };


            Phrase phraseName = new Phrase()
            {
                Font = fontHeader
            };
            phraseName.Add("Nombre del articulo");
            PdfPCell cellName = new PdfPCell(phraseName)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };


            Phrase phraseSku = new Phrase()
            {
                Font = fontHeader
            };
            phraseSku.Add("Nombre del articulo");
            PdfPCell cellSku = new PdfPCell(phraseSku)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };


            Phrase phraseLastInventory = new Phrase()
            {
                Font = fontHeader
            };
            phraseLastInventory.Add("Nombre del articulo");
            PdfPCell cellLastInventory = new PdfPCell(phraseLastInventory)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };



            Phrase phraseQuantity = new Phrase()
            {
                Font = fontHeader
            };
            phraseQuantity.Add("Nombre del articulo");
            PdfPCell cellQuantity = new PdfPCell(phraseQuantity)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };



            Phrase phraseUnit = new Phrase()
            {
                Font = fontHeader
            };
            phraseUnit.Add("Nombre del articulo");
            PdfPCell cellUnit = new PdfPCell(phraseUnit)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };



            Phrase phrasePrice = new Phrase()
            {
                Font = fontHeader
            };
            phrasePrice.Add("Nombre del articulo");
            PdfPCell cellPrice = new PdfPCell(phrasePrice)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };



            Phrase phraseTotal = new Phrase()
            {
                Font = fontHeader
            };
            phraseTotal.Add("Nombre del articulo");
            PdfPCell cellTotal = new PdfPCell(phraseTotal)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };

            table.AddCell(cellEntry);
            table.AddCell(cellName);
            table.AddCell(cellSku);
            table.AddCell(cellLastInventory);
            table.AddCell(cellQuantity);
            table.AddCell(cellUnit);
            table.AddCell(cellPrice);
            table.AddCell(cellTotal);

            return table;
        }

        private void FillInventoryTtable(PdfPTable table, List<ItemContract> items)
        {
            foreach (ItemContract item in items)
            {
                PdfPCell cell = new PdfPCell(new Phrase(item.Name))
                {
                    HorizontalAlignment = 1
                };

                table.AddCell(cell);
                table.AddCell(cell);
                table.AddCell(cell);
                table.AddCell(cell);
                table.AddCell(cell);
                table.AddCell(cell);
                table.AddCell(cell);
                table.AddCell(cell);
            }
            
        }
    }
}
