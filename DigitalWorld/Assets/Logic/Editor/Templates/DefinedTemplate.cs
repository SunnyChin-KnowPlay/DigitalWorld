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
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class DefinedTemplate : DefinedTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("namespace DigitalWorld.Logic\r\n{\r\n\tpublic enum EEvent : int\r\n\t{\r\n\t\tUpdate = 0,\r\n");
            
            #line 17 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

		for (int i = 0; i < eventNames.Length; ++i)
		{

            
            #line default
            #line hidden
            this.Write("\t\t/// <summary>\r\n        /// ");
            
            #line 22 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventDescs[i]));
            
            #line default
            #line hidden
            this.Write("\r\n        /// </summary>\r\n\t\t");
            
            #line 24 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventNames[i]));
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 24 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventValues[i]));
            
            #line default
            #line hidden
            this.Write(",\r\n");
            
            #line 25 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

		}

            
            #line default
            #line hidden
            this.Write("\t}\r\n\r\n\tpublic enum ECondition : int\r\n\t{\r\n");
            
            #line 32 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

		for (int i = 0; i < conditionNames.Length; ++i)
		{

            
            #line default
            #line hidden
            this.Write("\t\t/// <summary>\r\n        /// ");
            
            #line 37 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(conditionDescs[i]));
            
            #line default
            #line hidden
            this.Write("\r\n        /// </summary>\r\n\t\t");
            
            #line 39 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(conditionNames[i]));
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 39 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(conditionValues[i]));
            
            #line default
            #line hidden
            this.Write(",\r\n");
            
            #line 40 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

		}

            
            #line default
            #line hidden
            this.Write("\t}\r\n\r\n\tpublic enum EAction : int\r\n\t{\r\n");
            
            #line 47 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

		for (int i = 0; i < actionNames.Length; ++i)
		{

            
            #line default
            #line hidden
            this.Write("\t\t/// <summary>\r\n        /// ");
            
            #line 52 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(actionDescs[i]));
            
            #line default
            #line hidden
            this.Write("\r\n        /// </summary>\r\n\t\t");
            
            #line 54 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(actionNames[i]));
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 54 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(actionValues[i]));
            
            #line default
            #line hidden
            this.Write(",\r\n");
            
            #line 55 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

		}

            
            #line default
            #line hidden
            this.Write("\t}\r\n\r\n\t public static class Defined\r\n\t {\r\n        public static string GetEventDe" +
                    "sc(EEvent e)\r\n        {\r\n\t\t\tswitch (e)\r\n            {\r\n");
            
            #line 66 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

				for (int i = 0; i < eventNames.Length; ++i)
				{

            
            #line default
            #line hidden
            this.Write("\t\t\t\tcase EEvent.");
            
            #line 70 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventNames[i]));
            
            #line default
            #line hidden
            this.Write(":\r\n\t\t\t\t\treturn \"");
            
            #line 71 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventDescs[i]));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 72 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

				}

            
            #line default
            #line hidden
            this.Write("\t\t\t\tdefault:\r\n\t\t\t\t\treturn null;\r\n\t\t\t}\r\n        }\r\n\r\n\t\tpublic static string GetAct" +
                    "ionDesc(EAction e)\r\n        {\r\n\t\t\tswitch (e)\r\n            {\r\n");
            
            #line 84 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

				for (int i = 0; i < actionNames.Length; ++i)
				{

            
            #line default
            #line hidden
            this.Write("\t\t\t\tcase EAction.");
            
            #line 88 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(actionNames[i]));
            
            #line default
            #line hidden
            this.Write(":\r\n\t\t\t\t\treturn \"");
            
            #line 89 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(actionDescs[i]));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 90 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

				}

            
            #line default
            #line hidden
            this.Write("\t\t\t\tdefault:\r\n\t\t\t\t\treturn null;\r\n\t\t\t}\r\n        }\r\n\r\n\t\tpublic static string GetCon" +
                    "ditionDesc(ECondition e)\r\n        {\r\n\t\t\tswitch (e)\r\n            {\r\n");
            
            #line 102 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

				for (int i = 0; i < conditionNames.Length; ++i)
				{

            
            #line default
            #line hidden
            this.Write("\t\t\t\tcase ECondition.");
            
            #line 106 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(conditionNames[i]));
            
            #line default
            #line hidden
            this.Write(":\r\n\t\t\t\t\treturn \"");
            
            #line 107 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(conditionDescs[i]));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 108 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

				}

            
            #line default
            #line hidden
            this.Write("\t\t\t\tdefault:\r\n\t\t\t\t\treturn null;\r\n\t\t\t}\r\n        }\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "D:\Projects\DigitalWorld\DigitalWorld\DigitalWorld\Assets\Logic\Editor\Templates\DefinedTemplate.tt"

private string[] _eventNamesField;

/// <summary>
/// Access the eventNames parameter of the template.
/// </summary>
private string[] eventNames
{
    get
    {
        return this._eventNamesField;
    }
}

private string[] _eventValuesField;

/// <summary>
/// Access the eventValues parameter of the template.
/// </summary>
private string[] eventValues
{
    get
    {
        return this._eventValuesField;
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

private string[] _conditionNamesField;

/// <summary>
/// Access the conditionNames parameter of the template.
/// </summary>
private string[] conditionNames
{
    get
    {
        return this._conditionNamesField;
    }
}

private string[] _conditionValuesField;

/// <summary>
/// Access the conditionValues parameter of the template.
/// </summary>
private string[] conditionValues
{
    get
    {
        return this._conditionValuesField;
    }
}

private string[] _conditionDescsField;

/// <summary>
/// Access the conditionDescs parameter of the template.
/// </summary>
private string[] conditionDescs
{
    get
    {
        return this._conditionDescsField;
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

private string[] _actionValuesField;

/// <summary>
/// Access the actionValues parameter of the template.
/// </summary>
private string[] actionValues
{
    get
    {
        return this._actionValuesField;
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


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool eventNamesValueAcquired = false;
if (this.Session.ContainsKey("eventNames"))
{
    this._eventNamesField = ((string[])(this.Session["eventNames"]));
    eventNamesValueAcquired = true;
}
if ((eventNamesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("eventNames");
    if ((data != null))
    {
        this._eventNamesField = ((string[])(data));
    }
}
bool eventValuesValueAcquired = false;
if (this.Session.ContainsKey("eventValues"))
{
    this._eventValuesField = ((string[])(this.Session["eventValues"]));
    eventValuesValueAcquired = true;
}
if ((eventValuesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("eventValues");
    if ((data != null))
    {
        this._eventValuesField = ((string[])(data));
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
bool conditionNamesValueAcquired = false;
if (this.Session.ContainsKey("conditionNames"))
{
    this._conditionNamesField = ((string[])(this.Session["conditionNames"]));
    conditionNamesValueAcquired = true;
}
if ((conditionNamesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("conditionNames");
    if ((data != null))
    {
        this._conditionNamesField = ((string[])(data));
    }
}
bool conditionValuesValueAcquired = false;
if (this.Session.ContainsKey("conditionValues"))
{
    this._conditionValuesField = ((string[])(this.Session["conditionValues"]));
    conditionValuesValueAcquired = true;
}
if ((conditionValuesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("conditionValues");
    if ((data != null))
    {
        this._conditionValuesField = ((string[])(data));
    }
}
bool conditionDescsValueAcquired = false;
if (this.Session.ContainsKey("conditionDescs"))
{
    this._conditionDescsField = ((string[])(this.Session["conditionDescs"]));
    conditionDescsValueAcquired = true;
}
if ((conditionDescsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("conditionDescs");
    if ((data != null))
    {
        this._conditionDescsField = ((string[])(data));
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
bool actionValuesValueAcquired = false;
if (this.Session.ContainsKey("actionValues"))
{
    this._actionValuesField = ((string[])(this.Session["actionValues"]));
    actionValuesValueAcquired = true;
}
if ((actionValuesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("actionValues");
    if ((data != null))
    {
        this._actionValuesField = ((string[])(data));
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
    public class DefinedTemplateBase
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
