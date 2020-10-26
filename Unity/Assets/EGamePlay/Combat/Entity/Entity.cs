using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGamePlay
{
    public class Entity
    {
        public Entity Parent { get; set; }
        public List<Component> Components { get; set; }
    }
}