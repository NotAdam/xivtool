using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xivtool.Module
{
    [Description("Outputs gamerescape wiki template markup for items")]
    public class Item : BaseModule
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
            var outDict = new Dictionary<string, object>();

            var sheetEn = m_languageData[ SaintCoinach.Ex.Language.English ].GameData.Items;
            var sheetJp = m_languageData[ SaintCoinach.Ex.Language.Japanese ].GameData.Items;
            var sheetDe = m_languageData[ SaintCoinach.Ex.Language.German ].GameData.Items;

            var sheet = sheetEn[ itemid ];

            outDict[ "id" ] = itemid;
            outDict[ "name_en" ] = sheetEn[ itemid ].Name;
            outDict[ "name_jp" ] = sheetJp[ itemid ].Name;
            outDict[ "name_de" ] = sheetDe[ itemid ].Name;

            outDict[ "description" ] = sheet.GetRaw( "Description" );
            outDict[ "subheading" ] = sheet[ "ItemUICategory" ];
            outDict[ "itemlevel" ] = sheet.GetRaw( "Level{Item}" );
            outDict[ "rarity" ] = sheet.GetRaw( "Rarity" );
            outDict[ "untradable" ] = sheet.AsBoolean( "IsUntradable" ) ? "Yes" : "No";
            outDict[ "unique" ] = sheet.AsBoolean( "IsUnique" );
            outDict[ "dyeable" ] = sheet.AsBoolean( "IsDyeable" );
            outDict[ "iscrestworthy" ] = sheet.AsBoolean( "IsCrestWorthy" );
            outDict[ "canbehq" ] = sheet.AsBoolean( "CanBeHq" ) ? "Yes" : "No";
            outDict[ "advancedmelds" ] = sheet[ "IsAdvancedMeldingPermitted" ];

            outDict[ "requires" ] = sheet[ "ClassJobCategory" ].ToString().Replace( " ", ", " );

            var repairclass = sheet[ "ClassJob{Repair}" ].ToString();
            outDict[ "repairclass" ] = char.ToUpper( repairclass[ 0 ] ) + repairclass.Substring( 1 );

            var stackSize = sheet.AsInt32( "StackSize" );
            if( stackSize > 1 )
                outDict[ "stackSize" ] = stackSize;


            var price = sheet.AsInt32( "Price{Low}" );
            if( price > 0 )
                outDict[ "sells" ] = price;

            var materiaSlots = sheet.AsInt32( "MateriaSlotCount" );
            if( materiaSlots > 0 )
                outDict[ "materiaslots" ] = materiaSlots;

            var convertable = sheet.AsSingle( "MaterializeType" );
            // todo: there's a bunch of different values here, fuck knows what they mean
            if( convertable != 3 )
                outDict[ "convertable" ] = "Yes";
            else
                outDict[ "convertable" ] = "No";

            outDict[ "requiredlevel" ] = sheet.GetRaw( "Level{Equip}" );

            // item stats
            var physdmg = sheet.AsInt16( "Damage{Phys}" );
            var magdmg = sheet.AsInt16( "Damage{Mag}" );
            if( physdmg > 0 || magdmg > 0 )
            {
                outDict[ "physdmg" ] = physdmg;
                outDict[ "magdmg" ] = magdmg;

                var physdef = sheet.AsInt16( "Defense{Phys}" );
                if( physdef > 0 )
                {
                    outDict[ "physdef" ] = physdef;
                    outDict[ "magdef" ] = sheet[ "Defense{Mag}" ];
                }

                var blockstr = sheet.AsInt16( "Block" );
                if( blockstr > 0 )
                {
                    outDict[ "blockstr" ] = blockstr;
                    outDict[ "blockrate" ] = sheet[ "BlockRate" ];
                }

                var delay = ( double )sheet.AsInt16( "Delay<ms>" ) / 1000;

                outDict[ "attackdelay" ] = delay;
                outDict[ "autoattackdmg" ] = Math.Round( ( physdmg * delay ) / 3, 2, MidpointRounding.ToEven );


                // bonus stats
                var sb = new StringBuilder();
                for( int i = 0; i < 6; i++ )
                {
                    var nameKey = $"BaseParam[{i}]";
                    var valueKey = $"BaseParamValue[{i}]";

                    var nameHqKey = $"BaseParam{{Special}}[{i}]";
                    var valueHqKey = $"BaseParamValue{{Special}}[{i}]";

                    var value = sheet.AsInt16( valueKey );
                    var valueHq = sheet.AsInt16( valueHqKey );

                    if( value > 0 )
                    {
                        var name = sheet[ nameKey ];

                        sb.AppendLine( $"| Bonus {name} = +{value}" );

                        if( valueHq > 0 )
                        {
                            valueHq += value;
                            sb.AppendLine( $"| Bonus {name} HQ = +{valueHq}" );
                        }
                    }
                }

                if( sb.Length > 0 )
                    outDict[ "bonusstats" ] = sb.ToString();
            }

            SaveToTemplate( "Item", $"Item{itemid}.txt", outDict );
        }
    }
}
