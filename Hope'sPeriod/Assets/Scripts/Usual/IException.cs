    using System;
    using UnityEngine;

    public class OutOfRange: Exception{

        public OutOfRange(int range = 1, int index = 1)
            :base($"Out of range. this range is {range}, but you try to access {index}.") {}
        
         public OutOfRange(int range, int index, string moreInfo) 
                    :base($"Out of range. this range is {range}, but you try to access {index}. +{moreInfo}") {}
                

        public OutOfRange(float startRange, float endRange, float index)
            : base($"Out of range. this range is {startRange} ~ {endRange}, but you try to access {index}.") {}

        public OutOfRange(float startRange, float endRange, float index, string moreInfo)
            : base(
                $"Out of range. this range is {startRange} ~ {endRange}, but you try to access {index}. +{moreInfo}") {}
    }

    public class ForgetSetUpInspector : Exception {

        public ForgetSetUpInspector(Type type, string name)
            : base($"You seem to forget setup {type.Name}: {name}") {}

        public ForgetSetUpInspector(Type type)
            : base($"You seem to forget setup {type.FullName}") {}

        public ForgetSetUpInspector(string info)
            : base($"You seem to forget setup {info}") {}
    }

    public class TypeMissMatched : Exception {

        public TypeMissMatched(string a, string b)
            : base($"{a}'s component type isn't matched {b}") { }

        
        public TypeMissMatched(string a, Type b)
            : base($"{a}'s component type isn't matched {b}") { }
        //==================================================||
        public TypeMissMatched(GameObject a, Type b)
            : base($"{a.name}'s component type isn't matched {b}") { }
        public TypeMissMatched(Type a, Type b, GameObject @Object)
            : base($"{a.Name} type isn't matched {b}, check about {@Object.name}") { }
    }