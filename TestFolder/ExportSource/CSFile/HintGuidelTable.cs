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



public class HintGuidelTable : BaseTxtTable<HintGuidelTable>
{
    
    public string XlsxMapping = "System.xlsx";
    
    /// <summary>
    /// ID索引
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 展示顺序
    /// </summary>
    public int Order;
    
    /// <summary>
    /// 功能名称
    /// </summary>
    public string Name;
    
    /// <summary>
    /// 功能icon
    /// </summary>
    public string Icon;
    
    /// <summary>
    /// 需求角色等级
    /// </summary>
    public int Level;
    
    /// <summary>
    /// 需求完成任务
    /// </summary>
    public int Task;
    
    /// <summary>
    /// 赠送奖励
    /// </summary>
    public string Award;
    
    /// <summary>
    /// 界面描述
    /// </summary>
    public string Text;
    
    /// <summary>
    /// 调用背景图资源
    /// </summary>
    public string Bg;
    
    /// <summary>
    /// 显示的模型资源
    /// </summary>
    public string Model;
    
    public override int GetID()
    {
return ID ;
    }
}
