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



public class GuideTable : BaseTxtTable<GuideTable>
{
    
    public string XlsxMapping = "System.xlsx";
    
    /// <summary>
    /// ID索引
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 引导提示位置
    /// </summary>
    public string Place;
    
    /// <summary>
    /// 引导提示朝向
    /// </summary>
    public string Orientation;
    
    /// <summary>
    /// 引导提示文字内容
    /// </summary>
    public string Text;
    
    /// <summary>
    /// 引导点击控件
    /// </summary>
    public string Click;
    
    /// <summary>
    /// 是否增加强制保护
    /// </summary>
    public int ProTect;
    
    /// <summary>
    /// 提示清除方式
    /// </summary>
    public string Clear;
    
    /// <summary>
    /// 引导特效
    /// </summary>
    public string Effect;
    
    public override int GetID()
    {
return ID ;
    }
}
