using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

namespace HolyAngelMaternitySystem
{
    public static class DocumentFunctions
    {
        public static void IterateTextBody(WTextBody textBody)
        {
            //Iterates through each of the child items of WTextBody
            for (int i = 0; i < textBody.ChildEntities.Count; i++)
            {
                //IEntity is the basic unit in DocIO DOM. 
                //Accesses the body items (should be either paragraph, table or block content control) as IEntity
                IEntity bodyItemEntity = textBody.ChildEntities[i];
                //A Text body has 3 types of elements - Paragraph, Table and Block Content Control
                //Decides the element type by using EntityType
                switch (bodyItemEntity.EntityType)
                {
                    case EntityType.Paragraph:
                        WParagraph paragraph = bodyItemEntity as WParagraph;
                        //Checks for particular style name and removes the paragraph from DOM
                        if (paragraph.StyleName == "MyStyle")
                        {
                            int index = textBody.ChildEntities.IndexOf(paragraph);
                            textBody.ChildEntities.RemoveAt(index);
                        }
                        break;
                    case EntityType.Table:
                        //Table is a collection of rows and cells
                        //Iterates through table's DOM
                        IterateTable(bodyItemEntity as WTable);
                        break;
                    case EntityType.BlockContentControl:
                        BlockContentControl blockContentControl = bodyItemEntity as BlockContentControl;
                        //Iterates to the body items of Block Content Control.
                        IterateTextBody(blockContentControl.TextBody);
                        break;
                }
            }
        }

        private static void IterateTable(WTable table)
        {
            //Iterates the row collection in a table
            foreach (WTableRow row in table.Rows)
            {
                //Iterates the cell collection in a table row
                foreach (WTableCell cell in row.Cells)
                {
                    //Table cell is derived from (also a) TextBody
                    //Reusing the code meant for iterating TextBody
                    IterateTextBody(cell);
                }
            }
        }
    }
}
