using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Data
{
    public interface ITable
    {
    }

    public class TableBase<T>: ITable  where T : TableBase<T>
    {
        public int ID;
        public static Dictionary<int, ITable> list;
        public TableBase()
        {
            list.Add(this.ID, this);
        }
        
        
    }

    [Serializable]
    public class PersonData: TableBase<PersonData>
    {
       
        public int Age;
        public string Name;
        public string Job;
    }
}
