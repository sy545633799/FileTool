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



public class GuideGroupTable : BaseTxtTable<GuideGroupTable>
{
    
    public string XlsxMapping = "System.xlsx";
    
    /// <summary>
    /// ID索引
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 引导名称
    /// </summary>
    public string Name;
    
    /// <summary>
    /// 触发条件
    /// </summary>
    public string Condition;
    
    /// <summary>
    /// 触发的具体事件及参数
    /// </summary>
    public string Inc;
    
    /// <summary>
    /// 具体引导群
    /// </summary>
    public string GuideTableID;
    
    /// <summary>
    /// 是否可循环引导
    /// </summary>
    public int Circulation;
    
    public override int GetID()
    {
return ID ;
    }
}
