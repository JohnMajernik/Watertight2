using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Watertight.Filesystem
{
    internal class ResourcePtrConverter : JsonConverter<ResourcePtr>
    {

        public override ResourcePtr ReadJson(JsonReader reader, Type objectType, [AllowNull] ResourcePtr existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new ResourcePtr(reader.Value as string);
        }


        public override void WriteJson(JsonWriter writer, [AllowNull] ResourcePtr value, JsonSerializer serializer)
        {
            writer.WriteValue(value?._uri.ToString() ?? "");
        }
    }

    [JsonConverter(typeof(ResourcePtrConverter))]
    public class ResourcePtr
    {
        public static readonly ResourcePtr Empty = new ResourcePtr((Uri)null);

        internal Uri _uri;


        public string ResourceScheme
        {
            get
            {
                return _uri.Scheme;
            }
        }

        public string Path
        {
            get
            {
                return _uri.LocalPath;
            }
        }

        public object LoadedResource
        {
            get
            {
                return FileSystem.GetLoadedResource(this);
            }
        }

        public T Get<T>() where T : class
        {
            return LoadedResource as T;
        }

        public bool Loaded
        {
            get
            {
                return FileSystem.IsResourceLoaded(this);
            }
        }

        public bool Valid
        {
            get
            {
                if (this == Empty)
                {
                    return false;
                }

                return FileSystem.ResourceExists(this);
            }
        }

        public override string ToString()
        {
            return _uri.ToString();
        }

        public void Load()
        {
            FileSystem.LoadResource(this);
        }

        public void Unload()
        {
            FileSystem.UnloadResource(this);
        }
              

        public ResourcePtr(string Path)
            : this(new Uri(Path))
        {
           
        }

        public ResourcePtr(Uri Path)
        {
            _uri = Path;
        }

        public override bool Equals(object obj)
        {
            if(obj is ResourcePtr)
            {
                return (obj as ResourcePtr)._uri == _uri; 
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _uri.GetHashCode();
        }
    }
}
