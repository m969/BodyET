using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class BuffEditor : EditorWindow
{
    private Dictionary<int, string> buffTypes = new Dictionary<int, string>()
    {
        {0, "（空）"  },
        {1, "改变状态"  },
        {2, "改变数值"  },
        {3, "动作式触发"  },
        {4, "间隔式触发"  },
    };
    private Dictionary<int, string> stateTypes = new Dictionary<int, string>()
    {
        {0, "（空）"  },
        {1, "眩晕"  },
        {2, "中毒"  },
        {3, "燃烧"  },
        {4, "沉默"  },
        {5, "混乱"  },
    };
    private Dictionary<int, string> numericTypes = new Dictionary<int, string>()
    {
        {0, "（空）"  },
        {1, "攻击力"  },
        {2, "防御力"  },
        {3, "百分比攻击力"  },
        {4, "百分比防御力"  },
        {5, "生命回复"  },
    };
    private Dictionary<int, string> actionTypes = new Dictionary<int, string>()
    {
        {0, "（空）"  },
        {1, "发出普攻"  },
        {2, "遭受普攻"  },
    };
    private List<BuffConfig> buffConfigs = new List<BuffConfig>() { new BuffConfig() };
    public GUIStyle style = new GUIStyle();
    [MenuItem("Tools/Buff编辑器")]
    private static void ShowWindow()
    {
        GetWindow(typeof(BuffEditor));
    }

    private void OnEnable()
    {
        this.style.normal.textColor = Color.white;
        this.style.alignment = TextAnchor.MiddleLeft;
        //this.style.fixedHeight = 16;
        //this.style.fixedWidth = 20;
    }

    private void OnGUI()
    {
        //var type = EditorGUILayout.IntPopup(0, buffTypes.Values.ToArray(), buffTypes.Keys.ToArray());
        //if (GUILayout.Button("+"))
        //{
        //    buffTypes.Add(buffTypes.Count, "newType" + buffTypes.Count);
        //}
        var buffTypeKArr = buffTypes.Values.ToArray();
        var buffTypeVArr = buffTypes.Keys.ToArray();
        var stateTypeKArr = stateTypes.Values.ToArray();
        var stateTypeVArr = stateTypes.Keys.ToArray();
        var numericTypeKArr = numericTypes.Values.ToArray();
        var numericTypeVArr = numericTypes.Keys.ToArray();
        var actionTypeKArr = actionTypes.Values.ToArray();
        var actionTypeVArr = actionTypes.Keys.ToArray();
        foreach (var item in buffConfigs)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(16));
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                GUILayout.Label("ID:");
                item.Id = EditorGUILayout.IntField(item.Id, GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                GUILayout.Label("名称:");
                item.Name = EditorGUILayout.TextField(item.Name, GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
                GUILayout.Label("类型:");
                item.Type = EditorGUILayout.IntPopup(item.Type, buffTypeKArr, buffTypeVArr, GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();

                if (item.Type == 1)
                {
                    if (item.ChangeStateBuff == null)
                    {
                        item.ChangeStateBuff = new ChangeStateBuff();
                    }
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
                    this.style.normal.textColor = Color.blue;
                    GUILayout.Label("状态:", style);
                    item.ChangeStateBuff.State = EditorGUILayout.IntPopup(item.ChangeStateBuff.State, stateTypeKArr, stateTypeVArr, GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
                    GUILayout.Label("数值:");
                    item.ChangeStateBuff.Value = EditorGUILayout.TextField(item.ChangeStateBuff.Value, GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();
                }
                if (item.Type == 2)
                {
                    if (item.ChangeNumericBuff == null)
                    {
                        item.ChangeNumericBuff = new ChangeNumericBuff();
                    }
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
                    this.style.normal.textColor = Color.green;
                    GUILayout.Label("属性:", style);
                    item.ChangeNumericBuff.Numeric = EditorGUILayout.IntPopup(item.ChangeNumericBuff.Numeric, numericTypeKArr, numericTypeVArr, GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
                    GUILayout.Label("数值:");
                    item.ChangeNumericBuff.Value = EditorGUILayout.TextField(item.ChangeNumericBuff.Value, GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();
                }
                if (item.Type == 3)
                {
                    if (item.ActionTriggerBuff == null)
                    {
                        item.ActionTriggerBuff = new ActionTriggerBuff();
                    }
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
                    this.style.normal.textColor = Color.red;
                    GUILayout.Label("动作:", style);
                    item.ActionTriggerBuff.Action = EditorGUILayout.IntPopup(item.ActionTriggerBuff.Action, actionTypeKArr, actionTypeVArr, GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
                    GUILayout.Label("逻辑:");
                    item.ActionTriggerBuff.Logic = EditorGUILayout.TextField(item.ActionTriggerBuff.Logic, GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("+"))
        {
            buffConfigs.Add(new BuffConfig() { Id = buffConfigs.Count });
        }
    }
}

public class BuffConfig
{
    public int Id;
    public int Type;
    public string Name;
    public ChangeStateBuff ChangeStateBuff;
    public ChangeNumericBuff ChangeNumericBuff;
    public ActionTriggerBuff ActionTriggerBuff;
    public IntervalTriggerBuff IntervalTriggerBuff;
}

public abstract class Buff
{

}

public class ChangeStateBuff : Buff
{
    public int State;
    public string Value;
}

public class ChangeNumericBuff : Buff
{
    public int Numeric;
    public string Value;
}

public class ActionTriggerBuff : Buff
{
    public int Action;
    public string Logic;
}

public class IntervalTriggerBuff : Buff
{
    public int Interval;
}
