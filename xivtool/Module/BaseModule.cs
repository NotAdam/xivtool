using SaintCoinach;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace xivtool.Module
{
    public class BaseModule
    {
        public BaseModule( string dataPath )
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

        public void SaveToTemplate( string templateName, string outputName, Dictionary<string, object> data )
        {
            string basePath = Path.GetDirectoryName( System.Reflection.Assembly.GetEntryAssembly().Location );
            string templatePath = Path.Combine( basePath, "OutputFormats", $"{templateName}.txt" );
            string outputPath = Path.Combine( basePath, "Output" );
            
            if( !Directory.Exists ( outputPath  ) )
            {
                Directory.CreateDirectory( outputPath );
            }

            string output = File.ReadAllText( templatePath );

            // replace substitutions in template with values
            foreach( var item in data )
            {
                output = output.Replace( $"{{{item.Key}}}", item.Value.ToString() );
            }

            // remove any template params not set in the data dict
            string pattern = @"\{[a-zA-Z_0-9]+\}";
            output = Regex.Replace( output, pattern, "" );

            //Console.WriteLine( "Writing output to: {0}", templatePath );

            File.WriteAllText( Path.Combine( outputPath, outputName ), output );
        }

        protected readonly string m_dataPath;

        protected readonly Dictionary<SaintCoinach.Ex.Language, ARealmReversed> m_languageData;
    }
}
