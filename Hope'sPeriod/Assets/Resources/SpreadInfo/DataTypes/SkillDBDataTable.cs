//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpreadInfo {
    
    
    [System.SerializableAttribute()]
    public class SkillDBData : DefaultDataType {
        
        [UnityEngine.SerializeField()]
        private int m_Code;
        
        [UnityEngine.SerializeField()]
        private string m_Name;
        
        [UnityEngine.SerializeField()]
        private string m_DetailDescription;
        
        [UnityEngine.SerializeField()]
        private string m_Description;
        
        [UnityEngine.SerializeField()]
        private int m_UseHP;
        
        [UnityEngine.SerializeField()]
        private int m_UseMP;
        
        [UnityEngine.SerializeField()]
        private int m_ATK;
        
        [UnityEngine.SerializeField()]
        private int m_ATKCon;
        
        [UnityEngine.SerializeField()]
        private bool m_SkillItem;
        
        [UnityEngine.SerializeField()]
        private int m_SkillItemCon;
        
        public int Code {
            get {
                return this.m_Code;
            }
        }
        
        public string Name {
            get {
                return this.m_Name;
            }
        }
        
        public string DetailDescription {
            get {
                return this.m_DetailDescription;
            }
        }
        
        public string Description {
            get {
                return this.m_Description;
            }
        }
        
        public int UseHP {
            get {
                return this.m_UseHP;
            }
        }
        
        public int UseMP {
            get {
                return this.m_UseMP;
            }
        }
        
        public int ATK {
            get {
                return this.m_ATK;
            }
        }
        
        public int ATKCon {
            get {
                return this.m_ATKCon;
            }
        }
        
        public bool SkillItem {
            get {
                return this.m_SkillItem;
            }
        }
        
        public int SkillItemCon {
            get {
                return this.m_SkillItemCon;
            }
        }
    }
    
    public class SkillDBDataTable : DefaultDataTable<SpreadInfo.SkillDBData> {
    }
}
