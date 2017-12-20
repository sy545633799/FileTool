using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITable
{
    void ParseTxt(string txt);
}

public abstract class BaseTxtTable<T> : ITable where T : BaseTxtTable<T>
{
    
    public void ParseTxt(string txt)
    {
        Console.WriteLine(txt);
    }
}
