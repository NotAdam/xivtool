using SaintCoinach;
using System;
using System.Collections.Generic;

namespace ge_wiki_parser.Parser
{
    public class BaseParser
    {
        public BaseParser( string dataPath )
        {
            m_dataPath = dataPath;
            m_languageData = new Dictionary<SaintCoinach.Ex.Language, ARealmReversed>();
            

            foreach( var lang in Enum.GetValues( typeof( SaintCoinach.Ex.Language ) ) )
            {
                var language = ( SaintCoinach.Ex.Language )lang;
                m_languageData[ language ] = new ARealmReversed( dataPath, language );
            }
        }

        public void Parse(string[] args)
        {
            throw new NotImplementedException( "Parser does not implement the parse method." );
        }

        protected readonly string m_dataPath;

        protected readonly Dictionary<SaintCoinach.Ex.Language, ARealmReversed> m_languageData;
    }
}
