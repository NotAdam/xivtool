using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xivtool.Parser
{
    [Description("Calculates the radius for BNpcs and the median radius for all of them")]
    public class RadiusCalc : BaseParser
    {
        public RadiusCalc( string filePath ) : base( filePath ) { }

        private struct bullshit
        {
            public int modelcharaid;
            public float scale;
        }

        public new void Parse( string[] args )
        {
            var data = m_languageData[ SaintCoinach.Ex.Language.English ];

            var bnpcbase = data.GameData.GetSheet<SaintCoinach.Xiv.BNpcBase>();
            var modelskel = data.GameData.GetSheet( "ModelSkeleton" );

            var modelcharas = new Dictionary<int, bullshit>();

            foreach( var bnpc in bnpcbase )
            {
                if( !( bool )bnpc.GetRaw( 16 ) )
                    continue;

                var modelchara = bnpc.ModelChara;
                var modelid = modelchara.ModelKey;

                SaintCoinach.Xiv.XivRow skelRow;

                try
                {
                    skelRow = modelskel.First( e => e.Key == modelid );
                }
                catch( Exception )
                {
                    Console.WriteLine( "Failed getting modelskeleton for bnpc#{0} -> modelchara#{1}, modelid#{2}", bnpc.Key, modelchara.Key, modelid );
                    continue;
                }

                modelcharas.Add( bnpc.Key, new bullshit
                {
                    modelcharaid = bnpc.ModelChara.Key,
                    scale = ( float )skelRow.GetRaw( 0 ) * ( float )bnpc.Scale
                } );
            }


            // calc median the garbage way
            var numbers = new List<float>();

            foreach( var item in modelcharas )
            {
                Console.WriteLine( "Got bnpc#{0}, scale: {1}", item.Key, item.Value.scale );
                numbers.Add( item.Value.scale );
            }

            var arr = numbers.ToArray();
            Array.Sort( arr );

            var middle = arr.Length / 2;
            if( arr.Length % 2 == 0 )
            {
                Console.WriteLine( "median: {0}", arr[ middle ] );
            }
            else
            {
                var mid = arr[ middle ] + arr[ middle - 1 ] / 2;
                Console.WriteLine( "median: {0}", mid );
            }
        }
    }
}
