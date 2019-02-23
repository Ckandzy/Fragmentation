using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MuyiFrame
{
    public static class SingleTon<T> where T : class {
        private static T _instance;
        static SingleTon(){} // can't instance
        public static T Instance
        {
            get
            {
                if (_instance == null) _instance = (T)Activator.CreateInstance(typeof(T), true);
                return _instance;
            }
        }

        public static void Destroy()
        {
            _instance = null;
            return;
        }
    }
}



