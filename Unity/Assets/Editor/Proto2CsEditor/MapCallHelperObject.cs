using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#if UNITY_EDITOR && False
using UnityEditor;
[CustomEditor(typeof(MapCallHelperObject))]
public class MapCallHelperObjectInspector : OdinEditor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        var mapCallHelperObject = target as MapCallHelperObject;

        if (mapCallHelperObject.MessageClasses == null)
            return;

        MessageClass removeItem = null;
        for (int i = 0; i < mapCallHelperObject.MessageClasses.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            {
                var item = mapCallHelperObject.MessageClasses[i];
                var visible = EditorPrefs.GetBool($"{item.GetHashCode()}");
                EditorGUILayout.BeginVertical();

                {
                    var isToggled = SirenixEditorGUI.BeginToggleGroup(item, ref item.Enabled, ref visible, item.TagName);
                    if (isToggled)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("类名", GUILayout.Width(100));
                        item.ClassName = EditorGUILayout.TextField(item.ClassName);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("别名", GUILayout.Width(100));
                        item.TagName = EditorGUILayout.TextField(item.TagName);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("消息类型", GUILayout.Width(100));
                        item.MessageType = (ETMessageType)EditorGUILayout.EnumPopup(item.MessageType);
                        EditorGUILayout.EndHorizontal();

                        MessageParamConfig removeParam = null;
                        foreach (var item2 in item.MessageParamConfigs)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                item2.ParamName = EditorGUILayout.TextField(item2.ParamName, GUILayout.Width(100));
                                item2.ParamType = (Proto3Type)EditorGUILayout.EnumPopup(item2.ParamType);
                                if (GUILayout.Button("-", GUILayout.Width(20)))
                                {
                                    removeParam = item2;
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        if (removeParam != null)
                        {
                            item.MessageParamConfigs.Remove(removeParam);
                        }

                        EditorGUILayout.BeginHorizontal();
                        {
                            SirenixEditorGUI.IndentSpace();
                            if (GUILayout.Button("+"))
                            {
                                item.MessageParamConfigs.Add(new MessageParamConfig());
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorPrefs.SetBool($"{item.GetHashCode()}", visible);
                    SirenixEditorGUI.EndToggleGroup();
                }

                EditorGUILayout.EndVertical();

                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    removeItem = item;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeItem != null)
        {
            mapCallHelperObject.MessageClasses.Remove(removeItem);
        }

        if (GUILayout.Button("+"))
        {
            mapCallHelperObject.MessageClasses.Add(new MessageClass());
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

[CreateAssetMenu()]
public class MapCallHelperObject : SerializedScriptableObject
{
    [LabelText("消息类列表")]
    public List<MessageClass> MessageClasses;
    //public List<MessageParamConfig> MessageParamConfigs = new List<MessageParamConfig>();

    [Button("生成消息类代码", ButtonHeight = 30)]
    public void GenerateMessage()
    {

    }

    [Button("反向导入消息类配置", ButtonHeight = 30)]
    public void ImportMessages()
    {
        MessageClasses = ParseMessages();
    }

    public static List<MessageClass> ParseMessages()
    {
        var MessageClasses = new List<MessageClass>();
        var lines = File.ReadAllLines("./../Proto/HotfixMessage.proto");
        MessageClass message = null;
        foreach (var item in lines)
        {
            var str = item.Trim();
            var repeated = false;
            if (str.StartsWith("message "))
            {
                message = new MessageClass();
                var match = Regex.Match(str, @"(?<=message ).*?(?= )");
                var className = match.Value;
                if (string.IsNullOrEmpty(className))
                {
                    match = Regex.Match(str, @"(?<=message ).*?(?=\n)");
                    className = match.Value;
                    if (string.IsNullOrEmpty(className))
                    {
                        className = str.Replace("message ", "");
                    }
                }
                if (!string.IsNullOrEmpty(className))
                {
                    message.ClassName = className;
                    message.TagName = className;
                }

                var typeName = str.Replace($"message {className} // ", "");
                var type = ETMessageType.None;
                switch (typeName)
                {
                    case "IMessage": type = ETMessageType.IMessage;break;
                    case "IRequest": type = ETMessageType.IRequest;break;
                    case "IResponse": type = ETMessageType.IResponse;break;
                    case "IActorMessage": type = ETMessageType.IActorMessage;break;
                    case "IActorLocationMessage": type = ETMessageType.IActorLocationMessage;break;
                    case "IActorLocationRequest": type = ETMessageType.IActorLocationRequest;break;
                    case "IActorLocationResponse": type = ETMessageType.IActorLocationResponse; break;
                    default:break;
                }
                message.MessageType = type;
                MessageClasses.Add(message);
                continue;
            }
            else if (str.StartsWith("repeated "))
            {
                str = str.Replace("repeated ", "");
                str.Trim();
                repeated = true;
            }
            if (message == null)
                continue;
            var paramName = "";
            var messageType = "";
            var paramType = Proto3Type.Int32;
            //Debug.Log(str);
            if (str.StartsWith("int32 "))
            {
                var match = Regex.Match(str, @"(?<=int32 ).*?(?==)");
                paramName = match.Value;
                paramType = Proto3Type.Int32;
                if (repeated) paramType = Proto3Type.RepeatedInt32;
            }
            else if (str.StartsWith("int64 "))
            {
                var match = Regex.Match(str, @"(?<=int64 ).*?(?==)");
                paramName = match.Value;
                paramType = Proto3Type.Int64;
                if (repeated) paramType = Proto3Type.RepeatedInt64;
            }
            else if (str.StartsWith("float "))
            {
                var match = Regex.Match(str, @"(?<=float ).*?(?==)");
                paramName = match.Value;
                paramType = Proto3Type.Float;
                if (repeated) paramType = Proto3Type.RepeatedFloat;
            }
            else if (str.StartsWith("string "))
            {
                var match = Regex.Match(str, @"(?<=string ).*?(?==)");
                paramName = match.Value;
                paramType = Proto3Type.String;
                if (repeated) paramType = Proto3Type.RepeatedString;
            }
            else if (str.StartsWith("bytes "))
            {
                var match = Regex.Match(str, @"(?<=bytes ).*?(?==)");
                paramName = match.Value;
                paramType = Proto3Type.Bytes;
                if (repeated) paramType = Proto3Type.RepeatedBytes;
            }
            else
            {
                if (!str.StartsWith("//") && str.EndsWith(";"))
                {
                    var arr = str.Split(' ');
                    if (arr.Length > 2)
                    {
                        paramName = arr[1];
                        messageType = arr[0];
                        paramType = Proto3Type.Message;
                        if (repeated) paramType = Proto3Type.RepeatedMessage;
                    }
                }
                else
                {
                    continue;
                }
            }
            //Debug.Log(paramName);
            message.MessageParamConfigs.Add(new MessageParamConfig() { ParamName = paramName.Trim(), ParamType = paramType, MessageClassName = messageType });
        }
        return MessageClasses;
    }
}

[Serializable]
public class MessageClass
{
    [ToggleGroup("Enabled", "$TagName")]
    public bool Enabled;

    [ToggleGroup("Enabled")]
    [LabelText("消息类型")]
    public ETMessageType MessageType;

    [ToggleGroup("Enabled")]
    [LabelText("类名")]
    public string ClassName = "C2M_Message";

    [ToggleGroup("Enabled")]
    [LabelText("别名")]
    public string TagName = "消息类";

    [ToggleGroup("Enabled")]
    [LabelText("消息参数列表")]
    public List<MessageParamConfig> MessageParamConfigs = new List<MessageParamConfig>();
}

[Serializable]
public class MessageParamConfig
{
    [HorizontalGroup(80)]
    [HideLabel]
    public string ParamName;

    [HorizontalGroup(100)]
    [HideLabel]
    public Proto3Type ParamType;

    [HorizontalGroup(100)]
    [HideLabel]
    [ShowIf("ShowMessageClassName", true)]
    [ValueDropdown("GetAllMessages", DropdownWidth = 150, NumberOfItemsBeforeEnablingSearch = 3)]
    public string MessageClassName;

    public bool ShowMessageClassName
    {
        get
        {
            return ParamType == Proto3Type.Message || ParamType == Proto3Type.RepeatedMessage;
        }
    }

    public static IEnumerable GetAllMessages()
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath<MapCallHelperObject>("Assets/HotfixMessage.asset").MessageClasses.Select(x => new ValueDropdownItem(x.ClassName, x.ClassName));
    }
}

[Serializable]
public enum ETMessageType
{
    None,
    IMessage,
    IRequest,
    IResponse,
    IActorMessage,
    IActorLocationMessage,
    IActorLocationRequest,
    IActorLocationResponse,
}

[Serializable]
public enum Proto3Type
{
    Int32,
    Int64,
    Float,
    String,
    Bytes,
    Message,
    RepeatedInt32,
    RepeatedInt64,
    RepeatedFloat,
    RepeatedString,
    RepeatedBytes,
    RepeatedMessage,
}
