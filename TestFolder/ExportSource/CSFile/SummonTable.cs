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



public class SummonTable : BaseTxtTable<SummonTable>
{
    
    public string XlsxMapping = "Skill.xlsx";
    
    /// <summary>
    /// 索引
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 召唤物类型
    /// </summary>
    public int Type;
    
    /// <summary>
    /// 召唤物类型
    /// </summary>
    public int UnitType;
    
    /// <summary>
    /// 召唤物ID
    /// </summary>
    public int UnitId;
    
    /// <summary>
    /// 召唤物距离
    /// </summary>
    public string UnitCallDistance;
    
    /// <summary>
    /// 一次召唤数量
    /// </summary>
    public int UnitCount;
    
    /// <summary>
    /// 召唤物角度
    /// </summary>
    public string UnitCallAngle;
    
    /// <summary>
    /// 存活时间
    /// </summary>
    public int UnitAliveTime;
    
    /// <summary>
    /// 存活数量
    /// </summary>
    public int UnitAliveNum;
    
    /// <summary>
    /// 死亡挂钩
    /// </summary>
    public int UnitDeathBound;
    
    /// <summary>
    /// 召唤物AI
    /// </summary>
    public int Ai;
    
    /// <summary>
    /// AI参数扩展
    /// </summary>
    public string paramList;
    
    /// <summary>
    /// 召唤物基础属性
    /// </summary>
    public int Attr;
    
    /// <summary>
    /// 属性关联
    /// </summary>
    public string StatBound;
    
    public override int GetID()
    {
return ID ;
    }
}
