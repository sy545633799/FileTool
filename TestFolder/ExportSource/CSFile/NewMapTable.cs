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



public class NewMapTable : BaseTxtTable<NewMapTable>
{
    
    public string XlsxMapping = "Map.xlsx";
    
    /// <summary>
    /// 地图ID
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 地图名称
    /// </summary>
    public string SceneName;
    
    /// <summary>
    /// PK模式
    /// </summary>
    public int Pktype;
    
    /// <summary>
    /// 场景类型
    /// </summary>
    public int Type;
    
    /// <summary>
    /// 进入地图等级
    /// </summary>
    public string Level;
    
    /// <summary>
    /// 出生区域
    /// </summary>
    public int Point;
    
    /// <summary>
    /// 安全复活区域
    /// </summary>
    public int Resurgence;
    
    /// <summary>
    /// 场景资源ID
    /// </summary>
    public string ResName;
    
    /// <summary>
    /// 场景寻路文件
    /// </summary>
    public string NavMesh;
    
    /// <summary>
    /// 能否打开小地图
    /// </summary>
    public int OpenMap;
    
    /// <summary>
    /// 小地图资源
    /// </summary>
    public string Minimap;
    
    /// <summary>
    /// 小地图资源大小
    /// </summary>
    public string MinimapSize;
    
    /// <summary>
    /// 游戏地图大小
    /// </summary>
    public string MapSize;
    
    /// <summary>
    /// 小地图横偏移X
    /// </summary>
    public float X;
    
    /// <summary>
    /// 小地图纵向偏移Y
    /// </summary>
    public float Y;
    
    /// <summary>
    /// 是否世界地图显示
    /// </summary>
    public int WordMap;
    
    /// <summary>
    /// 显示资源
    /// </summary>
    public string WordMapPic;
    
    /// <summary>
    /// UI资源位置
    /// </summary>
    public string Position;
    
    /// <summary>
    /// 能否原地复活
    /// </summary>
    public int Raw;
    
    /// <summary>
    /// 原地复活消耗
    /// </summary>
    public string Expend;
    
    /// <summary>
    /// 原地复活时间
    /// </summary>
    public int Time1;
    
    /// <summary>
    /// 原地延迟时间
    /// </summary>
    public int Delay;
    
    /// <summary>
    /// 对应按钮名称
    /// </summary>
    public string Name;
    
    /// <summary>
    /// 安全复活时间
    /// </summary>
    public int Time2;
    
    public override int GetID()
    {
return ID ;
    }
}
