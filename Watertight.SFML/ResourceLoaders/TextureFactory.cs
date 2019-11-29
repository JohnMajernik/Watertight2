using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Watertight.Filesystem;

namespace Watertight.SFML.ResourceLoaders
{
    class TextureFactory : ResourceFactory
    {
        public override string[] ResourceSchemes
        {
            get
            {
                return new string[]
                {                    
                    "texture",
                };
            }
        }

        public override IEnumerable<string> FileExtensions => new string[] { ".bmp", ".png", ".tga", ".jpg", ".gif", ".psd", ".hdr", ".pic" };
        public override object GetResource(Stream stream)
        {
            using (MemoryStream s = new MemoryStream())
            {
                stream.CopyTo(s);
                return new Texture(s.ToArray());
            }
        }
    }
}
