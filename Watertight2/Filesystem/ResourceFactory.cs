using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Watertight.Filesystem
{    
    public abstract class ResourceFactory 
    {

        public abstract string[] ResourceSchemes
        {
            get;
            
        }

        public virtual IEnumerable<string> FileExtensions
        {
            get
            {
                return null;
            }
        }

        public abstract object GetResource(Stream stream);

        public virtual object GetResource(ResourcePtr Ptr)
        {
            object instance = null;

            using (Stream s = FileSystem.GetFileStream(Ptr))
            {
                instance = GetResource(s);
            }                
            
            return instance;
        }
    }   



}
