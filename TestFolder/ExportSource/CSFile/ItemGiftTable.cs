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



public class ItemGiftTable : BaseTxtTable<ItemGiftTable>
{
    
    public string XlsxMapping = "Item.xlsx";
    
    /// <summary>
    /// 礼包ID
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 物品ID
    /// </summary>
    public int ItemId;
    
    /// <summary>
    /// 数量
    /// </summary>
    public int Count;
    
    /// <summary>
    /// 职业
    /// </summary>
    public int Job;
    
    /// <summary>
    /// 性别
    /// </summary>
    public int Sex;
    
    /// <summary>
    /// 物品获得权重
    /// </summary>
    public int Weight;
    
    /// <summary>
    /// 时限类型
    /// </summary>
    public int TimeLimit;
    
    public override int GetID()
    {
return ID ;
    }
}