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



public class DonateTable : BaseTxtTable<DonateTable>
{
    
    public string XlsxMapping = "Faction.xlsx";
    
    /// <summary>
    /// 捐献类型ID
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 单次捐献消耗
    /// </summary>
    public string Expend;
    
    /// <summary>
    /// 获得帮会资金
    /// </summary>
    public int FactionFund;
    
    /// <summary>
    /// 获得帮会贡献
    /// </summary>
    public int Contribute;
    
    /// <summary>
    /// 获得捐献进度
    /// </summary>
    public int Plan;
    
    public override int GetID()
    {
return ID ;
    }
}
