﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本: 17.0.0.0
//  
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Assets.Logic.Editor.Templates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class LogicHelperTemplate : LogicHelperTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            
            #line 17 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tips));
            
            #line default
            #line hidden
            this.Write("\r\nnamespace ");
            
            #line 18 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(namespaceName));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n    public static partial class ");
            
            #line 20 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(className));
            
            #line default
            #line hidden
            this.Write(@"
    {
        /// <summary>
        /// 通过节点类型和ID来获取对应的节点对象
        /// </summary>
        /// <param name=""nodeType""></param>
        /// <param name=""id""></param>
        /// <returns></returns>
        public static NodeBase GetNode(ENodeType nodeType, int id)
        {
            switch (nodeType)
            {
                case ENodeType.Action:
                {
                    return GetAction(id);
                }
                case ENodeType.Trigger:
                {
                    return GetNode<Trigger>();
                }
                case ENodeType.Property:
                {
                    return GetProperty(id);
                }
                default:
                    return null;
            }
        }

        /// <summary>
        /// 通过枚举来获取对应的注释
        /// </summary>
        /// <param name=""action""></param>
        /// <returns></returns>
        public static string GetActionDesc(int id)
        {
            switch (id)
            {
");
            
            #line 58 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                for (int i = 0; i < actionEnums.Length; ++i)
                {

            
            #line default
            #line hidden
            this.Write("                case ");
            
            #line 62 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(actionEnums[i]));
            
            #line default
            #line hidden
            this.Write(":\r\n                    return \"");
            
            #line 63 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(actionDescs[i]));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 64 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                }

            
            #line default
            #line hidden
            this.Write(@"  
                default:
                    return null;
            }
        }

        /// <summary>
        /// 通过事件ID获取对应的注释
        /// </summary>
        /// <param name=""id"">事件ID</param>
        /// <returns></returns>
        public static string GetEventDesc(int id)
        {
            switch (id)
            {
");
            
            #line 81 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                for (int i = 0; i < eventEnums.Length; ++i)
                {

            
            #line default
            #line hidden
            this.Write("                case ");
            
            #line 85 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventEnums[i]));
            
            #line default
            #line hidden
            this.Write(":\r\n                    return \"");
            
            #line 86 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventDescs[i]));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 87 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                }

            
            #line default
            #line hidden
            this.Write(@"  
                default:
                    return null;
            }
        }

        /// <summary>
        /// 通过ID来获取对应的注释
        /// </summary>
        /// <param name=""action""></param>
        /// <returns></returns>
        public static string GetPropertyDesc(int id)
        {
            switch (id)
            {
");
            
            #line 104 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                for (int i = 0; i < propertyEnums.Length; ++i)
                {

            
            #line default
            #line hidden
            this.Write("                case ");
            
            #line 108 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyEnums[i]));
            
            #line default
            #line hidden
            this.Write(":\r\n                    return \"");
            
            #line 109 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyDescs[i]));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 110 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                }

            
            #line default
            #line hidden
            this.Write("  \r\n                default:\r\n                    return null;\r\n            }\r\n  " +
                    "      }\r\n\r\n        public static Actions.ActionBase GetAction(int id)\r\n        {" +
                    "\r\n            switch (id)\r\n            {\r\n");
            
            #line 122 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                for (int i = 0; i < actionEnums.Length; ++i)
                {

            
            #line default
            #line hidden
            this.Write("                case ");
            
            #line 126 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(actionEnums[i]));
            
            #line default
            #line hidden
            this.Write(":\r\n                    return GetNode<");
            
            #line 127 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(actionNames[i]));
            
            #line default
            #line hidden
            this.Write(">();\r\n");
            
            #line 128 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                }

            
            #line default
            #line hidden
            this.Write("  \r\n                default:\r\n                    return null;\r\n            }\r\n  " +
                    "      }\r\n\r\n        public static Properties.PropertyBase GetProperty(int id)\r\n  " +
                    "      {\r\n            switch (id)\r\n            {\r\n");
            
            #line 140 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                for (int i = 0; i < propertyEnums.Length; ++i)
                {

            
            #line default
            #line hidden
            this.Write("                case ");
            
            #line 144 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyEnums[i]));
            
            #line default
            #line hidden
            this.Write(":\r\n                    return GetNode<");
            
            #line 145 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyNames[i]));
            
            #line default
            #line hidden
            this.Write(">();\r\n");
            
            #line 146 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

                }

            
            #line default
            #line hidden
            this.Write("  \r\n                default:\r\n                    return null;\r\n            }\r\n  " +
                    "      }\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\LogicHelperTemplate.tt"

private string _classNameField;

/// <summary>
/// Access the className parameter of the template.
/// </summary>
private string className
{
    get
    {
        return this._classNameField;
    }
}

private string[] _actionEnumsField;

/// <summary>
/// Access the actionEnums parameter of the template.
/// </summary>
private string[] actionEnums
{
    get
    {
        return this._actionEnumsField;
    }
}

private string[] _actionNamesField;

/// <summary>
/// Access the actionNames parameter of the template.
/// </summary>
private string[] actionNames
{
    get
    {
        return this._actionNamesField;
    }
}

private string[] _actionDescsField;

/// <summary>
/// Access the actionDescs parameter of the template.
/// </summary>
private string[] actionDescs
{
    get
    {
        return this._actionDescsField;
    }
}

private string[] _propertyEnumsField;

/// <summary>
/// Access the propertyEnums parameter of the template.
/// </summary>
private string[] propertyEnums
{
    get
    {
        return this._propertyEnumsField;
    }
}

private string[] _propertyNamesField;

/// <summary>
/// Access the propertyNames parameter of the template.
/// </summary>
private string[] propertyNames
{
    get
    {
        return this._propertyNamesField;
    }
}

private string[] _propertyDescsField;

/// <summary>
/// Access the propertyDescs parameter of the template.
/// </summary>
private string[] propertyDescs
{
    get
    {
        return this._propertyDescsField;
    }
}

private string[] _eventEnumsField;

/// <summary>
/// Access the eventEnums parameter of the template.
/// </summary>
private string[] eventEnums
{
    get
    {
        return this._eventEnumsField;
    }
}

private string[] _eventDescsField;

/// <summary>
/// Access the eventDescs parameter of the template.
/// </summary>
private string[] eventDescs
{
    get
    {
        return this._eventDescsField;
    }
}

private string _tipsField;

/// <summary>
/// Access the tips parameter of the template.
/// </summary>
private string tips
{
    get
    {
        return this._tipsField;
    }
}

private string _namespaceNameField;

/// <summary>
/// Access the namespaceName parameter of the template.
/// </summary>
private string namespaceName
{
    get
    {
        return this._namespaceNameField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool classNameValueAcquired = false;
if (this.Session.ContainsKey("className"))
{
    this._classNameField = ((string)(this.Session["className"]));
    classNameValueAcquired = true;
}
if ((classNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("className");
    if ((data != null))
    {
        this._classNameField = ((string)(data));
    }
}
bool actionEnumsValueAcquired = false;
if (this.Session.ContainsKey("actionEnums"))
{
    this._actionEnumsField = ((string[])(this.Session["actionEnums"]));
    actionEnumsValueAcquired = true;
}
if ((actionEnumsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("actionEnums");
    if ((data != null))
    {
        this._actionEnumsField = ((string[])(data));
    }
}
bool actionNamesValueAcquired = false;
if (this.Session.ContainsKey("actionNames"))
{
    this._actionNamesField = ((string[])(this.Session["actionNames"]));
    actionNamesValueAcquired = true;
}
if ((actionNamesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("actionNames");
    if ((data != null))
    {
        this._actionNamesField = ((string[])(data));
    }
}
bool actionDescsValueAcquired = false;
if (this.Session.ContainsKey("actionDescs"))
{
    this._actionDescsField = ((string[])(this.Session["actionDescs"]));
    actionDescsValueAcquired = true;
}
if ((actionDescsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("actionDescs");
    if ((data != null))
    {
        this._actionDescsField = ((string[])(data));
    }
}
bool propertyEnumsValueAcquired = false;
if (this.Session.ContainsKey("propertyEnums"))
{
    this._propertyEnumsField = ((string[])(this.Session["propertyEnums"]));
    propertyEnumsValueAcquired = true;
}
if ((propertyEnumsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("propertyEnums");
    if ((data != null))
    {
        this._propertyEnumsField = ((string[])(data));
    }
}
bool propertyNamesValueAcquired = false;
if (this.Session.ContainsKey("propertyNames"))
{
    this._propertyNamesField = ((string[])(this.Session["propertyNames"]));
    propertyNamesValueAcquired = true;
}
if ((propertyNamesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("propertyNames");
    if ((data != null))
    {
        this._propertyNamesField = ((string[])(data));
    }
}
bool propertyDescsValueAcquired = false;
if (this.Session.ContainsKey("propertyDescs"))
{
    this._propertyDescsField = ((string[])(this.Session["propertyDescs"]));
    propertyDescsValueAcquired = true;
}
if ((propertyDescsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("propertyDescs");
    if ((data != null))
    {
        this._propertyDescsField = ((string[])(data));
    }
}
bool eventEnumsValueAcquired = false;
if (this.Session.ContainsKey("eventEnums"))
{
    this._eventEnumsField = ((string[])(this.Session["eventEnums"]));
    eventEnumsValueAcquired = true;
}
if ((eventEnumsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("eventEnums");
    if ((data != null))
    {
        this._eventEnumsField = ((string[])(data));
    }
}
bool eventDescsValueAcquired = false;
if (this.Session.ContainsKey("eventDescs"))
{
    this._eventDescsField = ((string[])(this.Session["eventDescs"]));
    eventDescsValueAcquired = true;
}
if ((eventDescsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("eventDescs");
    if ((data != null))
    {
        this._eventDescsField = ((string[])(data));
    }
}
bool tipsValueAcquired = false;
if (this.Session.ContainsKey("tips"))
{
    this._tipsField = ((string)(this.Session["tips"]));
    tipsValueAcquired = true;
}
if ((tipsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("tips");
    if ((data != null))
    {
        this._tipsField = ((string)(data));
    }
}
bool namespaceNameValueAcquired = false;
if (this.Session.ContainsKey("namespaceName"))
{
    this._namespaceNameField = ((string)(this.Session["namespaceName"]));
    namespaceNameValueAcquired = true;
}
if ((namespaceNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("namespaceName");
    if ((data != null))
    {
        this._namespaceNameField = ((string)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class LogicHelperTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
