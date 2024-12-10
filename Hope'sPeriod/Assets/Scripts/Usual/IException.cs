    using System;

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