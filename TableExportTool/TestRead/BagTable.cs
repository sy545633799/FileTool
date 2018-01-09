using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BagTable : BaseTxtTable<BagTable>
{
    public int ID;
    public int OpenLevel;
    public int Consume;

    public override int GetID()
    {
        return ID;
    }
}
