
using Newtonsoft.Json.Linq;
using NLog.Filters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Watertight.Filesystem;
using Watertight.Interfaces;
using Watertight.Util;

namespace Watertight.Scripts
{
    public class NoNativeClassException : Exception
    {
        public NoNativeClassException()
        {
        }

        public NoNativeClassException(string message) : base(message)
        {
        }

        public NoNativeClassException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class ObjectScript : IIsResource
    {
        const string NativeClassName = "$" + nameof(NativeClass);
        const string ParentScriptName = "$" + nameof(ParentScript);


        public JObject JObject
        {
            get;
            protected internal set;
        }

        public virtual SubclassOf<object> NativeClass
        {
            get
            {
                if (_NativeClass == null)
                {
                    string ClassName = JObject.Value<string>(NativeClassName) ?? "";
                    _NativeClass = Utils.FindTypeFromString(ClassName);
                }
                return _NativeClass;
            }
        }

        public virtual ObjectScript ParentScript
        {
            get
            {
                if (_ParentScript == null)
                {
                    _ParentScript = JObject.SelectToken(ParentScriptName)?.ToObject<ResourcePtr>(); 
                    _ParentScript?.Load();
                }
                return _ParentScript?.Get<ObjectScript>();
            }
        }

        private ResourcePtr _ParentScript;

        public ResourcePtr ResourcePtr
        {
            get;
            set;
        }

        private Type _NativeClass;

        public  bool HasParentScript
        {
            get
            {
                return ParentScript != null;
            }
        }

        public bool HasNativeClass
        {
            get
            {
                return NativeClass != null;
            }
        }        
        

        public virtual void ApplyToObject(object obj)
        {
            if(obj == null)
            {
                return;
            }

            Internal_ApplyToObject(obj);
           

            if(obj is IHasScript)
            {
                (obj as IHasScript).Script = this;
                (obj as IHasScript).PostScriptApplied();
            }
        }

        protected virtual void Internal_ApplyToObject(object obj)
        {
            if(ParentScript != null)
            {
                ParentScript?.Internal_ApplyToObject(obj);
            }

            Type t = obj.GetType();

            foreach (var Property in t.GetProperties())
            {
                if (JObject.ContainsKey(Property.Name))
                {
                    object val = JObject.SelectToken(Property.Name).ToObject(Property.PropertyType);
                    Property.SetValue(obj, val);
                }
            }
        }

        

        public virtual object CreateInstance()
        {
            //If we have a parent script and a native class, instantiate the native class and apply all parent scripts.  
            //If we have a Parent Script and No Native Class, walk up the chain until we find a Native Class (if no class, throw exception)
            //If we have No Parent Script and a Native Class, create the native class and apply this script.
            //If we have Neither Parent Script or Native Class, throw an exception.

            //Find the parent native class

            ObjectScript SearchScript = this;
            Type FoundNativeClass = NativeClass;
            while(FoundNativeClass == null)
            {
                SearchScript = SearchScript.ParentScript ?? throw new NoNativeClassException(String.Format("{0} Has no native class in any search path", ResourcePtr.ToString()));               
                FoundNativeClass = SearchScript.NativeClass;
            }

            object o = Activator.CreateInstance(FoundNativeClass);
            ApplyToObject(o);

            return o;
        }

        public virtual T CreateInstance<T>() where T : class
        {
            return CreateInstance() as T;
        }
    }
}
