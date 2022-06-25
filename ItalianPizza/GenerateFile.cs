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
        public void MakeInventoryReport(Uri filename, List<StockTakingContract> stockTakings)
        {
            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.A4.Rotate());
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


            table = MakeTableInventory(stockTakings);

            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            doc.Add(table);
            doc.Close();
            doc.OpenDocument();
        }

        private PdfPTable MakeTableInventory(List<StockTakingContract> stockTakings)
        {
            PdfPTable table = new PdfPTable(7);

            Font fontHeader = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.WHITE);

            Phrase phraseEntry = new Phrase()
            {
                Font = fontHeader
            };
            phraseEntry.Add("id");
            PdfPCell cellEntry = new PdfPCell(phraseEntry)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };


            Phrase phrasePhysicQuantity = new Phrase()
            {
                Font = fontHeader
            };
            phrasePhysicQuantity.Add("Cantidad fisica");
            PdfPCell cellPhysicQuantity = new PdfPCell(phrasePhysicQuantity)
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
            phraseSku.Add("Código");
            PdfPCell cellSku = new PdfPCell(phraseSku)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };


            Phrase phraseLastInventory = new Phrase()
            {
                Font = fontHeader
            };
            phraseLastInventory.Add("Cantidad Actual");
            PdfPCell cellLastInventory = new PdfPCell(phraseLastInventory)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };



            Phrase phraseQuantity = new Phrase()
            {
                Font = fontHeader
            };
            phraseQuantity.Add("Fecha");
            PdfPCell cellQuantity = new PdfPCell(phraseQuantity)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };



            Phrase phraseUnit = new Phrase()
            {
                Font = fontHeader
            };
            phraseUnit.Add("Cantidad fisica");
            PdfPCell cellUnit = new PdfPCell(phraseUnit)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };



            Phrase phraseDescription = new Phrase()
            {
                Font = fontHeader
            };
            phraseDescription.Add("Descripción");
            PdfPCell cellDescription = new PdfPCell(phraseDescription)
            {
                BackgroundColor = BaseColor.BLACK,
                HorizontalAlignment = 1
            };

            table.AddCell(cellEntry);
            table.AddCell(cellPhysicQuantity);
            table.AddCell(cellName);
            table.AddCell(cellSku);
            table.AddCell(cellLastInventory);
            table.AddCell(cellQuantity);
            table.AddCell(cellDescription);

            Font fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.BLACK);
            foreach (StockTakingContract item in stockTakings)
            {
                Phrase id = new Phrase()
                {
                    Font = fontCell
                };
                id.Add(item.IdItem.ToString());
                PdfPCell itemId = new PdfPCell(id)
                {
                    BackgroundColor = BaseColor.WHITE,
                    HorizontalAlignment = 1
                };

                Phrase name = new Phrase()
                {
                    Font = fontCell
                };
                name.Add(item.Name.ToString());
                PdfPCell itemNAme = new PdfPCell(name)
                {
                    BackgroundColor = BaseColor.WHITE,
                    HorizontalAlignment = 1
                };

                Phrase sku = new Phrase()
                {
                    Font = fontCell
                };
                sku.Add(item.Sku.ToString());
                PdfPCell itemSku = new PdfPCell(sku)
                {
                    BackgroundColor = BaseColor.WHITE,
                    HorizontalAlignment = 1
                };

                Phrase lastInventory = new Phrase()
                {
                    Font = fontCell
                };
                lastInventory.Add(item.Date.ToString());
                PdfPCell itemDate = new PdfPCell(lastInventory)
                {
                    BackgroundColor = BaseColor.WHITE,
                    HorizontalAlignment = 1
                };

                Phrase quantity = new Phrase()
                {
                    Font = fontCell
                };
                quantity.Add(item.CurrentAmount.ToString());
                PdfPCell itemQuantity = new PdfPCell(quantity)
                {
                    BackgroundColor = BaseColor.WHITE,
                    HorizontalAlignment = 1
                };
                Phrase physicAmount = new Phrase()
                {
                    Font = fontCell
                };
                physicAmount.Add(item.PhysicalAmount.ToString());
                PdfPCell itemphysicAmount = new PdfPCell(physicAmount)
                {
                    BackgroundColor = BaseColor.WHITE,
                    HorizontalAlignment = 1
                };
                Phrase description = new Phrase()
                {
                    Font = fontCell
                };
                description.Add(item.Description.ToString());
                PdfPCell itemDescription = new PdfPCell(description)
                {
                    BackgroundColor = BaseColor.WHITE,
                    HorizontalAlignment = 1
                };

                table.AddCell(itemId);
                table.AddCell(itemphysicAmount);
                table.AddCell(itemNAme);
                table.AddCell(itemSku);
                table.AddCell(itemQuantity);
                table.AddCell(itemDate);
                table.AddCell(itemDescription);

            }


            return table;
        }

    }
}