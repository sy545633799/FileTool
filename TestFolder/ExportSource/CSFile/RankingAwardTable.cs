//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.IO;



public class RankingAwardTable : BaseTxtTable<RankingAwardTable>
{
    
    public string XlsxMapping = "Battleground.xlsx";
    
    /// <summary>
    /// ID索引
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 获得荣誉
    /// </summary>
    public int Award;
    
    /// <summary>
    /// 奖励衰减
    /// </summary>
    public string Attenuation;
    
    public override int GetID()
    {
return ID ;
    }
}
