using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RunAndGun.Space
{
    public class CustomClasses
    {
    }
    public class CustomList<T> : List<T>
    {
        public new void Remove (T item)
        {
            
            base.Remove(item);
        }
    }
}