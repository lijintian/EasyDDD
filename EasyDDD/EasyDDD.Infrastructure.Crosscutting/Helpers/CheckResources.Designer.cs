﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasyDDD.Infrastructure.Crosscutting.Helpers {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CheckResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CheckResources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EasyDDD.Infrastructure.Crosscutting.Helpers.CheckResources", typeof(CheckResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 \&quot;{0}\&quot; cannot be null. 的本地化字符串。
        /// </summary>
        internal static string ArgumentCannotBeNull {
            get {
                return ResourceManager.GetString("ArgumentCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 \&quot;{0}\&quot; cannot be neither null nor an empty array. 的本地化字符串。
        /// </summary>
        internal static string ArgumentCannotBeNullOrEmptyArray {
            get {
                return ResourceManager.GetString("ArgumentCannotBeNullOrEmptyArray", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 \&quot;{0}\&quot; cannot be neither null nor an empty collection. 的本地化字符串。
        /// </summary>
        internal static string ArgumentCannotBeNullOrEmptyCollection {
            get {
                return ResourceManager.GetString("ArgumentCannotBeNullOrEmptyCollection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 \&quot;{0}\&quot; cannot be neither null nor an empty string. 的本地化字符串。
        /// </summary>
        internal static string ArgumentCannotBeNullOrEmptyString {
            get {
                return ResourceManager.GetString("ArgumentCannotBeNullOrEmptyString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 \&quot;{0}\&quot; must be between \&quot;{1}\&quot; and \&quot;{2}\&quot;. 的本地化字符串。
        /// </summary>
        internal static string ArgumentMustBeInRange {
            get {
                return ResourceManager.GetString("ArgumentMustBeInRange", resourceCulture);
            }
        }
    }
}