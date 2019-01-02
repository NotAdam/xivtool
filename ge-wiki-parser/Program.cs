using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ge_wiki_parser
{
    class Program
    {
        static void PrintHelp()
        {
            Console.WriteLine( "ge-wiki-parser <data path> <module> [module args...]" );
        }

        static void Main( string[] args )
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if( args.Length < 2 )
            {
                Console.WriteLine( "Missing arguments." );
                PrintHelp();

                return;
            }

            var gameDir = args[ 0 ];

            if( !System.IO.Directory.Exists( gameDir ) )
            {
                Console.WriteLine( "Game path '{0}' doesn't exist!", gameDir );
                return;
            }

            // find parser in parser namespace
            var assembly = Assembly.GetEntryAssembly();
            var parserType = assembly.GetTypes().First( 
                t => t.Name.ToLower() == args[ 1 ].ToLower() && 
                t.Namespace == "ge_wiki_parser.Parser"
            );

            var parser = Activator.CreateInstance( parserType, args[ 0 ] );

            // cause we only have the type and using the base class will call the base method instead
            // we need to directly call the parse method on the created instance
            parserType.GetMethod( "Parse" ).Invoke( parser, new object[] { args.Skip( 2 ).ToArray() }  );
        }
    }
}
