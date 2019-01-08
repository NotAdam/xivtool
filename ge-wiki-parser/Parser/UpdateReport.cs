using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ge_wiki_parser.Parser
{
    class UpdateReport : BaseParser
    {
        public UpdateReport( string filePath ) : base( filePath )
        {

        }

        public new void Parse( string[] args )
        {
            var data = m_languageData[ SaintCoinach.Ex.Language.English ];

            if( !data.IsCurrentVersion )
            {
                const bool IncludeDataChanges = true;
                var updateReport = data.Update( IncludeDataChanges );
            }
        }
    }
}
