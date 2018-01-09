using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AppearanceTable : BaseTxtTable<AppearanceTable>
{
    public int ID;
    public string Name;
    public string AppearanceRes;

    public override int GetID()
    {
        return ID;
    }
}
