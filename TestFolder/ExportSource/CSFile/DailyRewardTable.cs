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



public class DailyRewardTable : BaseTxtTable<DailyRewardTable>
{
    
    public string XlsxMapping = "Daily.xlsx";
    
    /// <summary>
    /// 序号
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 活跃值
    /// </summary>
    public int Daily;
    
    /// <summary>
    /// 活跃奖励道具ICON
    /// </summary>
    public string Icon;
    
    /// <summary>
    /// 活跃奖励道具
    /// </summary>
    public string DailyReward;
    
    public override int GetID()
    {
return ID ;
    }
}
