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



public class CardSuitTable : BaseTxtTable<CardSuitTable>
{
    
    public string XlsxMapping = "Card.xlsx";
    
    /// <summary>
    /// 套装等级
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 套装名称
    /// </summary>
    public string Name;
    
    /// <summary>
    /// 条件
    /// </summary>
    public string Condition;
    
    /// <summary>
    /// 属性
    /// </summary>
    public string Attr;
    
    public override int GetID()
    {
return ID ;
    }
}
