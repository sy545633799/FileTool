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



public class SkillTable : BaseTxtTable<SkillTable>
{
    
    public string XlsxMapping = "Skill.xlsx";
    
    /// <summary>
    /// ID
    /// </summary>
    public int ID;
    
    /// <summary>
    /// 名称
    /// </summary>
    public string Name;
    
    /// <summary>
    /// 短描述
    /// </summary>
    public string ShortDescribe;
    
    /// <summary>
    /// 技能类型
    /// </summary>
    public int SkillType;
    
    /// <summary>
    /// 技能释放中心（释放类型）
    /// </summary>
    public int Lock;
    
    /// <summary>
    /// 释放中心产生的特效
    /// </summary>
    public int AttackEffect;
    
    /// <summary>
    /// 指示范围
    /// </summary>
    public string WarningScope;
    
    /// <summary>
    /// 范围指示特效
    /// </summary>
    public string EarlyWarning;
    
    /// <summary>
    /// 施法距离
    /// </summary>
    public float ConjureDistance;
    
    /// <summary>
    /// 范围特效
    /// </summary>
    public string ScopeEffect;
    
    /// <summary>
    /// 自动搜索方式
    /// </summary>
    public int SelfMotion;
    
    /// <summary>
    /// 辅助锁定距离
    /// </summary>
    public float Distance;
    
    /// <summary>
    /// 动作
    /// </summary>
    public string Action;
    
    /// <summary>
    /// 动作持续时间
    /// </summary>
    public string ActionDuration;
    
    /// <summary>
    /// 以自己为中心的特效
    /// </summary>
    public string Effect;
    
    /// <summary>
    /// 是否显示读条
    /// </summary>
    public string ProgressBar;
    
    /// <summary>
    /// 是否可被打断
    /// </summary>
    public string Break;
    
    /// <summary>
    /// 屏幕震动
    /// </summary>
    public string ScreenShake;
    
    /// <summary>
    /// 释放技能音效
    /// </summary>
    public string SoundEffect;
    
    /// <summary>
    /// 技能ICON
    /// </summary>
    public string SkillIcon;
    
    /// <summary>
    /// 碰撞对象ID
    /// </summary>
    public int NearTableId;
    
    /// <summary>
    /// 位移ID
    /// </summary>
    public int Displacement;
    
    /// <summary>
    /// 召唤延迟时间
    /// </summary>
    public string SummonDelay;
    
    /// <summary>
    /// 召唤物ID
    /// </summary>
    public string SummonID;
    
    /// <summary>
    /// 召唤延迟时间
    /// </summary>
    public string TriggerDelay;
    
    /// <summary>
    /// 触发器ID
    /// </summary>
    public string BuffTriggerId;
    
    /// <summary>
    /// 连击的下一个技能ID
    /// </summary>
    public int NextSkillId;
    
    /// <summary>
    /// 多久内有效
    /// </summary>
    public int ValidTime;
    
    /// <summary>
    /// 操作记忆时间
    /// </summary>
    public string MemoryTime;
    
    /// <summary>
    /// 公共CD组
    /// </summary>
    public int CdGroup;
    
    /// <summary>
    /// 职业限制
    /// </summary>
    public int Profession;
    
    /// <summary>
    /// 技能类型细分
    /// </summary>
    public int Type;
    
    /// <summary>
    /// 解锁等级
    /// </summary>
    public int ClearLevel;
    
    /// <summary>
    /// 槽位限制
    /// </summary>
    public int Slot;
    
    /// <summary>
    /// 冷却时间
    /// </summary>
    public int CoolingTime;
    
    /// <summary>
    /// 冷却时间增量
    /// </summary>
    public int CoolingTimeUpgrade;
    
    /// <summary>
    /// 法力消耗（固定值）
    /// </summary>
    public int MP;
    
    /// <summary>
    /// 法力消耗（固定值）每级增加
    /// </summary>
    public int MPUpgrade;
    
    /// <summary>
    /// 初始法力消耗（万分比）
    /// </summary>
    public int MPC;
    
    /// <summary>
    /// 法力消耗（万分比）每级增加
    /// </summary>
    public int MPCUpgrade;
    
    /// <summary>
    /// 消耗物品
    /// </summary>
    public string ItemID;
    
    public override int GetID()
    {
return ID ;
    }
}