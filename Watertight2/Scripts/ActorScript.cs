
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Watertight.Filesystem;
using Watertight.Framework;
using Watertight.Util;

namespace Watertight.Scripts
{
    
    public class ActorScript : ObjectScript
    {
        const string ComponentEntryName = "$" + nameof(Components);

        static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        List<ObjectScript> Components = new List<ObjectScript>();

        public override void ApplyToObject(object obj)
        {
            if(!(obj is Actor))
            {
                throw new ArgumentException("obj must be an actor", nameof(obj));               
            }

            Actor actor = obj as Actor;

            
            if(JObject.ContainsKey(ComponentEntryName) && JObject[ComponentEntryName] is JArray)
            {
                JArray ComponetArray = JObject[ComponentEntryName] as JArray;
                foreach(JObject jo in ComponetArray)
                {
                    ObjectScript compscript = new ObjectScript()
                    {
                        JObject = jo
                    };

                    Components.Add(compscript);

                }
            }

            base.ApplyToObject(obj);
        }

        protected override void Internal_ApplyToObject(object obj)
        {
            base.Internal_ApplyToObject(obj);
        }



        public void ApplyComponentsToActor(Actor actor)
        {
            foreach(ObjectScript os in Components)
            {               
                SubclassOf<ActorComponent> ComponentSubclass = os.NativeClass.SubclassType;                

                ActorComponent AC = actor.GetComponentByClass(ComponentSubclass);
                bool RegisterComponent = false;                               
                if(AC == null)
                {
                    AC = os.CreateInstance<ActorComponent>();
                    RegisterComponent = true;
                }
                else
                {
                    //CreateInstance will apply the script.  If we didn't apply the inlined CompScript, then we need to apply it
                    os.ApplyToObject(AC);
                }

                
                if(RegisterComponent)
                {
                    AC.Owner = actor;
                    AC.Register();
                }
            }
        }
    }
}
