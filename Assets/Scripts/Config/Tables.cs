
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Luban;
using SimpleJSON;

namespace cfg
{
public partial class Tables
{

    public static List<string> TbFileNames = new()
    {
	    "common_tbconfig",
    };


    public Common.TbConfig TbConfig {get; }

    public Tables(System.Func<string, JSONNode> loader)
    {
        TbConfig = new Common.TbConfig(loader("common_tbconfig"));
        ResolveRef();
    }
    
    private void ResolveRef()
    {
        TbConfig.ResolveRef(this);
    }
}

}