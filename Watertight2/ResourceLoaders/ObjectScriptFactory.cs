using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Watertight.Filesystem;
using Watertight.Scripts;

namespace Watertight.ResourceLoaders
{
    class ObjectScriptFactory : ResourceFactory
    {
        protected virtual SubclassOf<ObjectScript> ScriptType
        {
            get;
            set;
        } = typeof(ObjectScript);

        public override string[] ResourceSchemes
        {
            get
            {
                return new string[]
                {
                    "oscript"
                };
            }
        }

        public override IEnumerable<string> FileExtensions => new string[] { ".oscript" };

        public override object GetResource(Stream stream)
        {
            using(TextReader Reader = new StreamReader(stream))
            using(JsonReader JReader = new JsonTextReader(Reader))
            {
                ObjectScript os = Activator.CreateInstance(ScriptType) as ObjectScript;
                os.JObject = JObject.Load(JReader);
                return os;
            }
            
        }
    }
}
