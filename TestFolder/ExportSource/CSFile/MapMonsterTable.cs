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



public class MapMonsterTable : BaseTxtTable<MapMonsterTable>
{
    
    public string XlsxMapping = "MapMonster.xlsx";
    
    /// <summary>
    ///  索引
    /// </summary>
    public int ID;
    
    /// <summary>
    ///  怪物id
    /// </summary>
    public int Monsterid;
    
    /// <summary>
    /// 地图ID
    /// </summary>
    public int MapID;
    
    /// <summary>
    /// 坐标
    /// </summary>
    public string xyz;
    
    /// <summary>
    /// 范围
    /// </summary>
    public int range;
    
    /// <summary>
    /// 数量
    /// </summary>
    public int number;
    
    /// <summary>
    /// 波次
    /// </summary>
    public int wave;
    
    /// <summary>
    /// 朝向
    /// </summary>
    public string angle;
    
    /// <summary>
    /// 巡逻路径
    /// </summary>
    public string Patrol;
    
    /// <summary>
    /// 怪物阵营
    /// </summary>
    public int Camp;
    
    public override int GetID()
    {
return ID ;
    }
}
