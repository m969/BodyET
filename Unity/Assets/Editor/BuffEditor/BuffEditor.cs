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
        {0, "（功能效果）"  },
        {1, "立即执行逻辑"  },
        {2, "条件执行逻辑"  },
        {3, "动作式触发（功能）"  },
        {4, "间隔式触发（功能）"  },
        {5, "条件式触发（功能）"  },
    };
    private Dictionary<int, string> logicTypes = new Dictionary<int, string>()
    {
        {0, "（逻辑类型）"  },
        {1, "改变状态"  },
        {2, "改变数值"  },
        {3, "执行逻辑"  },
    };
    private List<BuffConfig> buffConfigs = new List<BuffConfig>() { new BuffConfig() };
    public GUIStyle style = new GUIStyle();

    private string[] buffTypeKArr;
    private int[] buffTypeVArr;

    private string[] logicTypeKArr;
    private int[] logicTypeVArr;

    private string[] stateTypeKArr;
    private int[] stateTypeVArr;

    private string[] numericTypeKArr;
    private int[] numericTypeVArr;

    private string[] actionTypeKArr;
    private int[] actionTypeVArr;

    private string[] conditionTypeKArr;
    private int[] conditionTypeVArr;

    private Color col;
    private Color textColor;
    private Vector2 scrollPos;

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

    private (string[], int[]) LoadConfig(string configName)
    {
        var data = AssetDatabase.LoadAssetAtPath<IdNameTableObject>($"Assets/{configName}.asset");
        var kArr = new string[data.Names.Length + 1];
        kArr[0] = "（空）";
        var vArr = new int[data.Names.Length + 1];
        for (int i = 0; i < data.Names.Length; i++)
        {
            kArr[i + 1] = data.Names[i];
        }
        for (int i = 0; i < vArr.Length; i++)
        {
            vArr[i] = i;
        }
        return (kArr, vArr);
    }

    private void OnGUI()
    {
        textColor = GUI.skin.textField.normal.textColor;
        buffTypeKArr = buffTypes.Values.ToArray();
        buffTypeVArr = buffTypes.Keys.ToArray();
        logicTypeKArr = logicTypes.Values.ToArray();
        logicTypeVArr = logicTypes.Keys.ToArray();

        (stateTypeKArr, stateTypeVArr) = LoadConfig("状态配置");
        stateTypeKArr[0] = "（请选择状态）";
        (numericTypeKArr, numericTypeVArr) = LoadConfig("属性配置");
        numericTypeKArr[0] = "（请选择属性）";
        (actionTypeKArr, actionTypeVArr) = LoadConfig("动作配置");
        actionTypeKArr[0] = "（请选择动作）";
        (conditionTypeKArr, conditionTypeVArr) = LoadConfig("条件配置");
        conditionTypeKArr[0] = "（请选择条件）";

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos); // 组开始

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
                GUILayout.Label("：");
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

        EditorGUILayout.EndScrollView(); // 组结束
    }

    private int IntPopupDecorate(int value, string[] kArr, int[] vArr, int width = 100)
    {
        if (value == 0)
        {
            GUI.skin.textField.normal.textColor = Color.grey * 0.8f;
            value = EditorGUILayout.IntPopup(value, kArr, vArr, GUI.skin.textField, GUILayout.Width(width));
            GUI.skin.textField.normal.textColor = textColor;
        }
        else
        {
            value = EditorGUILayout.IntPopup(value, kArr, vArr, GUILayout.Width(width));
        }
        return value;
    }

    private void MiddleCenterLabel(string label)
    {
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label(label);
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
    }

    private string DefaultTextField(string defaultText, string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            text = defaultText;
        }
        if (text == defaultText)
        {
            GUI.skin.textField.normal.textColor = Color.grey * 0.8f;
            text = EditorGUILayout.TextField(text, GUI.skin.textField, GUILayout.Width(100));
            GUI.skin.textField.normal.textColor = textColor;
        }
        else
        {
            text = EditorGUILayout.TextField(text, GUILayout.Width(100));
        }
        return text;
    }

    private void OnFuncDraw(FunctionConfig func)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(120));
        if (func.Type == 1)
            GUILayout.Label("执行");
        else
            GUILayout.Label("附加");
        func.Type = IntPopupDecorate(func.Type, buffTypeKArr, buffTypeVArr, 120);
        EditorGUILayout.EndHorizontal();

        if (func.Type == 1)
        {
            if (func.ExecuteLogic == null) func.ExecuteLogic = new ExecuteLogic();
            var executeLogic = func.ExecuteLogic;

            EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
            executeLogic.Type = IntPopupDecorate(executeLogic.Type, logicTypeKArr, logicTypeVArr);
            EditorGUILayout.EndHorizontal();

            if (executeLogic.Type == 1)
            {
                if (executeLogic.ChangeState == null) executeLogic.ChangeState = new ChangeState();
                var changeState = executeLogic.ChangeState;
                EditorGUILayout.BeginHorizontal(GUILayout.Width(130));
                changeState.State = IntPopupDecorate(changeState.State, stateTypeKArr, stateTypeVArr);
                MiddleCenterLabel("->");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                changeState.Value = DefaultTextField("（数值表达式）", changeState.Value);
                EditorGUILayout.EndHorizontal();
            }
            if (executeLogic.Type == 2)
            {
                if (executeLogic.ChangeNumeric == null) executeLogic.ChangeNumeric = new ChangeNumeric();
                var changeNumeric = executeLogic.ChangeNumeric;
                EditorGUILayout.BeginHorizontal(GUILayout.Width(130));
                changeNumeric.Numeric = IntPopupDecorate(changeNumeric.Numeric, numericTypeKArr, numericTypeVArr);
                MiddleCenterLabel("+");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                changeNumeric.Value = DefaultTextField("（数值表达式）", changeNumeric.Value);
                EditorGUILayout.EndHorizontal();
            }
            if (executeLogic.Type == 3)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                executeLogic.Value = DefaultTextField("（逻辑类型）", executeLogic.Value);
                EditorGUILayout.EndHorizontal();
            }
        }
        if (func.Type == 2)
        {
            if (func.ActionTrigger == null) func.ActionTrigger = new ActionTrigger();
            var actionTrigger = func.ActionTrigger;
            EditorGUILayout.BeginHorizontal(GUILayout.Width(130));
            actionTrigger.Action = IntPopupDecorate(actionTrigger.Action, actionTypeKArr, actionTypeVArr);
            MiddleCenterLabel(">");
            EditorGUILayout.EndHorizontal();

            OnFuncDraw(actionTrigger.LogicFunc);
        }
        if (func.Type == 3)
        {
            if (func.IntervalTrigger == null) func.IntervalTrigger = new IntervalTrigger();
            var intervalTrigger = func.IntervalTrigger;
            EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
            intervalTrigger.Interval = EditorGUILayout.IntField(intervalTrigger.Interval, GUILayout.Width(60));
            GUILayout.Label("(秒)");
            MiddleCenterLabel(">");
            EditorGUILayout.EndHorizontal();

            OnFuncDraw(intervalTrigger.LogicFunc);
        }
        if (func.Type == 4)
        {
            if (func.ConditionTrigger == null) func.ConditionTrigger = new ConditionTrigger();
            var conditionTrigger = func.ConditionTrigger;

            EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
            conditionTrigger.Condition = IntPopupDecorate(conditionTrigger.Condition, conditionTypeKArr, conditionTypeVArr);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
            conditionTrigger.Value = DefaultTextField("（判定表达式）", conditionTrigger.Value);
            MiddleCenterLabel(">");
            EditorGUILayout.EndHorizontal();

            OnFuncDraw(conditionTrigger.LogicFunc);
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
    public ActionTrigger ActionTrigger;
    public IntervalTrigger IntervalTrigger;
    public ConditionTrigger ConditionTrigger;
    public ExecuteLogic ExecuteLogic;
}

public class LogicConfig
{
    public int Type;
    public ChangeState ChangeState;
    public ChangeNumeric ChangeNumeric;
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
    //public int Logic;
    public FunctionConfig LogicFunc = new FunctionConfig();
}

public class IntervalTrigger : Function
{
    public int Interval;
    //public int Logic;
    public FunctionConfig LogicFunc = new FunctionConfig();
}

public class ConditionTrigger : Function
{
    public int Condition;
    public string Value;
    public FunctionConfig LogicFunc = new FunctionConfig();
}

public class ExecuteLogic : Function
{
    //public int Logic;
    //public LogicConfig LogicConfig = new LogicConfig();
    public int Type;
    public ChangeState ChangeState;
    public ChangeNumeric ChangeNumeric;
    public string Value;
}
