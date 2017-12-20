using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Data
{
    public interface ITable
    {
        void Add();
    }

    [Serializable]
    public class TableBase<T> : ITable where T : TableBase<T>
    {
        public int ID;
        public static Dictionary<int, ITable> dictionary = new Dictionary<int, ITable>();

        public TableBase() { } //此时数据还初始化

        /// <summary>
        /// 必须要在反序列化完成的回调中使用（此时ID才有值）
        /// </summary>
        public void Add()
        {
            dictionary[ID] = this;
        }

        public int GetID()
        {
            return ID;
        }

        public static T GetTable(int ID)
        {
            if (dictionary.ContainsKey(ID))
                return dictionary[ID] as T;
            else return null;
        }
    }

    [Serializable]
    public class PersonData : TableBase<PersonData>
    {
        public int Age;
        public string Name;
        public string Job;
    }

    public class TableTest
    {
        public void CreateData()
        {
            PersonData data = new PersonData();
            data.Age = 13;
            data.ID = 123;
            data.Name = "test";
            data.Job = "programmer";

            //PathUtility.ReadFromPath(PathHelper.filepath, obj =>
            //{
            //    ((PersonData)obj).Add();
            //});
        }
    }

}
