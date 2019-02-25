using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xivtool.Module
{
    [Description("Attempts to automagically update ex.json using the SaintCoinach built in diff tool")]
    class UpdateReport : BaseModule
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
