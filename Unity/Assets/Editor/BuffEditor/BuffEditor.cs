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
        {5, "条件式触发"  },
        {6, "执行逻辑"  },
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
    private Dictionary<int, string> conditionTypes = new Dictionary<int, string>()
    {
        {0, "（空）"  },
        {1, "当生命低于"  },
        {2, "当生命低于百分比"  },
        {3, "当法力低于"  },
        {4, "当法力低于百分比"  },
    };
    private List<BuffConfig> buffConfigs = new List<BuffConfig>() { new BuffConfig() };
    public GUIStyle style = new GUIStyle();
    private string[] buffTypeKArr;
    private int[] buffTypeVArr;

    private string[] stateTypeKArr;
    private int[] stateTypeVArr;

    private string[] numericTypeKArr;
    private int[] numericTypeVArr;

    private string[] actionTypeKArr;
    private int[] actionTypeVArr;

    private string[] conditionTypeKArr;
    private int[] conditionTypeVArr;

    private Color col;


    [MenuItem("Tools/Buff编辑器/Buff编辑器窗口")]
    private static void ShowWindow()
    {
        GetWindow(typeof(BuffEditor));
    }

    [MenuItem("Tools/Buff编辑器/创建状态配置文件")]
    public static void CreateStateTableAsset()
    {
        CreateAsset("状态配置");
    }

    [MenuItem("Tools/Buff编辑器/创建属性配置文件")]
    public static void CreateNumericTableAsset()
    {
        CreateAsset("属性配置");
    }

    [MenuItem("Tools/Buff编辑器/创建动作配置文件")]
    public static void CreateActionTableAsset()
    {
        CreateAsset("动作配置");
    }

    [MenuItem("Tools/Buff编辑器/创建条件配置文件")]
    public static void CreateConditionTableAsset()
    {
        CreateAsset("条件配置");
    }

    public static void CreateAsset(string name)
    {
        var asset = ScriptableObject.CreateInstance<IdNameTableObject>();
        AssetDatabase.CreateAsset(asset, $"Assets/{name}.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    private void OnEnable()
    {
        //this.style.normal.textColor = Color.white;
        this.style.alignment = TextAnchor.MiddleLeft;
        //this.style.fixedHeight = 16;
        //this.style.fixedWidth = 20;
        col = Color.white * 0.5f;
    }

    private void OnGUI()
    {
        buffTypeKArr = buffTypes.Values.ToArray();
        buffTypeVArr = buffTypes.Keys.ToArray();

        var data = AssetDatabase.LoadAssetAtPath<IdNameTableObject>("Assets/状态配置.asset");

        //stateTypeKArr = data.Names.Select(x => x).ToArray();
        stateTypeKArr = new string[data.Names.Length+1];
        stateTypeKArr[0] = "（空）";
        stateTypeVArr = new int[data.Names.Length+1];
        for (int i = 0; i < data.Names.Length; i++)
        {
            stateTypeKArr[i+1] = data.Names[i];
        }
        for (int i = 0; i < stateTypeVArr.Length; i++)
        {
            stateTypeVArr[i] = i;
        }
        //stateTypeVArr = data.Names.Select(x => x.Id).ToArray();

        numericTypeKArr = numericTypes.Values.ToArray();
        numericTypeVArr = numericTypes.Keys.ToArray();
        actionTypeKArr = actionTypes.Values.ToArray();
        actionTypeVArr = actionTypes.Keys.ToArray();
        conditionTypeKArr = conditionTypes.Values.ToArray();
        conditionTypeVArr = conditionTypes.Keys.ToArray();

        //var type = EditorGUILayout.IntPopup(0, buffTypes.Values.ToArray(), buffTypes.Keys.ToArray());
        //if (GUILayout.Button("+"))
        //{
        //    buffTypes.Add(buffTypes.Count, "newType" + buffTypes.Count);
        //}

        BuffConfig removeBuff = null;
        foreach (var item in buffConfigs)
        {
            EditorGUILayout.BeginHorizontal(/*GUILayout.Height(16)*/);
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                GUILayout.Label("ID:");
                item.Id = EditorGUILayout.IntField(item.Id, GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                GUILayout.Label("名称:");
                item.Name = EditorGUILayout.TextField(item.Name, GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                GUILayout.Label("时间:");
                item.Duration = EditorGUILayout.FloatField(item.Duration, GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();

                if (item.Functions == null)
                {
                    item.Functions = new List<FunctionConfig>() { new FunctionConfig() { Type = 1 } };
                }
                FunctionConfig removeFunc = null;
                EditorGUILayout.BeginVertical();
                foreach (var func in item.Functions)
                {
                    EditorGUILayout.BeginHorizontal();
                    OnFuncDraw(func);
                    {
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(30));
                        if (GUILayout.Button("-"))
                        {
                            removeFunc = (func);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if (removeFunc != null)
                {
                    item.Functions.Remove(removeFunc);
                }
                if (item.Functions.Count == 0)
                {
                    removeBuff = item;
                }
                GUI.color = Color.white;
                {
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(30));
                    if (GUILayout.Button("+"))
                    {
                        item.Functions.Add(new FunctionConfig());
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        if (removeBuff != null)
        {
            buffConfigs.Remove(removeBuff);
        }
        if (GUILayout.Button("+", GUILayout.Height(30)))
        {
            buffConfigs.Add(new BuffConfig() { Id = buffConfigs.Count });
        }
    }

    private void OnFuncDraw(FunctionConfig func)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
        GUILayout.Label("功能:");
        func.Type = EditorGUILayout.IntPopup(func.Type, buffTypeKArr, buffTypeVArr, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        if (func.Type == 1)
        {
            if (func.ChangeState == null) func.ChangeState = new ChangeState();
            var changeState = func.ChangeState;
            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUI.color = Color.blue + col;
            GUILayout.Label("状态:");
            GUI.color = Color.white;
            changeState.State = EditorGUILayout.IntPopup(changeState.State, stateTypeKArr, stateTypeVArr, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUILayout.Label("数值表达式:");
            changeState.Value = EditorGUILayout.TextField(changeState.Value, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }
        if (func.Type == 2)
        {
            if (func.ChangeNumeric == null) func.ChangeNumeric = new ChangeNumeric();
            var changeNumeric = func.ChangeNumeric;
            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUI.color = Color.green + col;
            GUILayout.Label("属性:");
            GUI.color = Color.white;
            changeNumeric.Numeric = EditorGUILayout.IntPopup(changeNumeric.Numeric, numericTypeKArr, numericTypeVArr, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUILayout.Label("数值表达式:");
            changeNumeric.Value = EditorGUILayout.TextField(changeNumeric.Value, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }
        if (func.Type == 3)
        {
            if (func.ActionTrigger == null) func.ActionTrigger = new ActionTrigger();
            var actionTrigger = func.ActionTrigger;
            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUI.color = Color.red + col;
            GUILayout.Label("动作:");
            GUI.color = Color.white;
            actionTrigger.Action = EditorGUILayout.IntPopup(actionTrigger.Action, actionTypeKArr, actionTypeVArr, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUILayout.Label("功能:");
            //actionTrigger.Logic = EditorGUILayout.TextField(actionTrigger.Logic, GUILayout.Width(100));
            actionTrigger.Logic = EditorGUILayout.IntPopup(actionTrigger.Logic, buffTypeKArr, buffTypeVArr, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }
        if (func.Type == 4)
        {
            if (func.IntervalTrigger == null) func.IntervalTrigger = new IntervalTrigger();
            var intervalTrigger = func.IntervalTrigger;
            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUI.color = Color.yellow + col;
            GUILayout.Label("间隔:");
            GUI.color = Color.white;
            intervalTrigger.Interval = EditorGUILayout.IntField(intervalTrigger.Interval, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUILayout.Label("功能:");
            //intervalTrigger.Logic = EditorGUILayout.TextField(intervalTrigger.Logic, GUILayout.Width(100));
            intervalTrigger.Logic = EditorGUILayout.IntPopup(intervalTrigger.Logic, buffTypeKArr, buffTypeVArr, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }
        if (func.Type == 5)
        {
            if (func.ConditionTrigger == null) func.ConditionTrigger = new ConditionTrigger();
            var conditionTrigger = func.ConditionTrigger;

            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUI.color = Color.yellow + col;
            GUILayout.Label("条件:");
            GUI.color = Color.white;
            conditionTrigger.Condition = EditorGUILayout.IntPopup(conditionTrigger.Condition, conditionTypeKArr, conditionTypeVArr, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            GUILayout.Label("判定表达式:");
            conditionTrigger.Value = EditorGUILayout.TextField(conditionTrigger.Value, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            //GUILayout.Label("功能:");
            //conditionTrigger.Logic = EditorGUILayout.TextField(conditionTrigger.Logic, GUILayout.Width(100));
            //conditionTrigger.LogicFunc.Type = EditorGUILayout.IntPopup(conditionTrigger.LogicFunc.Type, buffTypeKArr, buffTypeVArr, GUILayout.Width(100));
            //EditorGUILayout.BeginVertical();
            {
                //EditorGUILayout.BeginHorizontal();
                OnFuncDraw(conditionTrigger.LogicFunc);
                //EditorGUILayout.EndHorizontal();
            }
            //EditorGUILayout.EndVertical();
        }
    }
}

public class BuffConfig
{
    public int Id;
    public int Type;
    public string Name;
    public float Duration;
    public List<FunctionConfig> Functions;
}

public class FunctionConfig
{
    public int Type;
    public ChangeState ChangeState;
    public ChangeNumeric ChangeNumeric;
    public ActionTrigger ActionTrigger;
    public IntervalTrigger IntervalTrigger;
    public ConditionTrigger ConditionTrigger;
    public ExecuteLogic ExecuteLogic;
}

public abstract class Function
{
    //public string ParamValue;
}

public abstract class Buff
{

}

public class ChangeState : Function
{
    public int State;
    public string Value;
}

public class ChangeNumeric : Function
{
    public int Numeric;
    public string Value;
}

public class ActionTrigger : Function
{
    public int Action;
    public int Logic;
}

public class IntervalTrigger : Function
{
    public int Interval;
    public int Logic;
}

public class ConditionTrigger : Function
{
    public int Condition;
    public string Value;
    public FunctionConfig LogicFunc = new FunctionConfig();
}

public class ExecuteLogic : Function
{
    public int Logic;
    public string Value;
}
