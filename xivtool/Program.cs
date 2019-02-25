using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace xivtool
{
    class Program
    {
        static void PrintHelp()
        {
            Console.WriteLine( "xivtool <data path> <module> [module args...]" );

            Console.WriteLine();
            Console.WriteLine( "Available modules: " );

            var assembly = Assembly.GetEntryAssembly();

            var parsers = assembly.GetTypes().Where( t => t.Namespace == "xivtool.Module" );

            foreach( var parser in parsers )
            {
                var desc = parser.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();

                if( desc == null )
                    continue;

                Console.WriteLine( "  {0} - {1}", parser.Name, desc.Description );
            }
        }

        static void Main( string[] args )
        {
            Console.OutputEncoding = Encoding.UTF8;

            if( args.Length < 2 )
            {
                Console.WriteLine( "Error: Missing arguments.\n" );
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
                t.Namespace == "xivtool.Module"
            );

            var parser = Activator.CreateInstance( parserType, args[ 0 ] );

            // cause we only have the type and using the base class will call the base method instead
            // we need to directly call the parse method on the created instance
            parserType.GetMethod( "Parse" ).Invoke( parser, new object[] { args.Skip( 2 ).ToArray() }  );
        }
    }
}
