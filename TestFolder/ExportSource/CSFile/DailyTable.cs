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



public class DailyTable : BaseTxtTable<DailyTable>
{
    
    public string XlsxMapping = "Daily.xlsx";
    
    /// <summary>
    /// 序号
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 顺序
    /// </summary>
    public int Order;
    
    /// <summary>
    /// 日常类型
    /// </summary>
    public string Type;
    
    /// <summary>
    /// 名称等级描述
    /// </summary>
    public string ConditionDesc;
    
    /// <summary>
    /// 开启等级
    /// </summary>
    public int Lvl;
    
    /// <summary>
    /// 需要完成次数
    /// </summary>
    public int Count;
    
    /// <summary>
    /// 玩家活力值奖励
    /// </summary>
    public int PlayerDaily;
    
    /// <summary>
    /// 活动ICON
    /// </summary>
    public string ICON;
    
    /// <summary>
    /// 活动小标签
    /// </summary>
    public string Tips;
    
    /// <summary>
    /// 玩法描述
    /// </summary>
    public string GameDesc;
    
    /// <summary>
    /// 奖励展示
    /// </summary>
    public string Reward;
    
    /// <summary>
    /// 领取奖励
    /// </summary>
    public string ReceiveReward;
    
    /// <summary>
    /// 入口
    /// </summary>
    public string Entrance;
    
    public override int GetID()
    {
return ID ;
    }
}
