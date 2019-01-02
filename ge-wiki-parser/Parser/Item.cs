using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ge_wiki_parser.Parser
{
    public class Item : BaseParser
    {
        public Item( string filePath ) : base( filePath )
        {

        }

        public new void Parse( string[] args )
        {
            if( args.Length < 1 )
            {
                Console.WriteLine( "Missing module arguments, requires an itemid." );
            }

            var itemid = int.Parse( args[ 0 ] );
            var outDict = new Dictionary<string, string>();

            var sheetEn = m_languageData[ SaintCoinach.Ex.Language.English ].GameData.Items;
            var sheetJp = m_languageData[ SaintCoinach.Ex.Language.Japanese ].GameData.Items;
            var sheetDe = m_languageData[ SaintCoinach.Ex.Language.German ].GameData.Items;

            var item = sheetEn[ itemid ];

            outDict[ "id" ] = itemid.ToString();
            outDict[ "name_en" ] = sheetEn[ itemid ].Name;
            outDict[ "name_jp" ] = sheetJp[ itemid ].Name;
            outDict[ "name_de" ] = sheetDe[ itemid ].Name;

            foreach( var entry in outDict )
            {
                System.IO.File.AppendAllText( "out.txt", $"{entry.Key}\t\t{entry.Value}\n" );
            }
        }
    }
}
