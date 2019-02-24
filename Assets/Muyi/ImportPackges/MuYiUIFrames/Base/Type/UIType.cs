using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuyiFrame
{
	public abstract class UIType{
        public string name { get; private set; }
        public string Path;

        public UIType(string _path)
        {
            name = _path.Substring(_path.LastIndexOf('/') + 1);
            Path = _path;
        }

        public override string ToString()
        {
            return string.Format("this uitype path is {0}, name is {1}", Path, name);
        }
    }
}


