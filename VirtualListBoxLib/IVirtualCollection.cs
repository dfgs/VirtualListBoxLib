using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualListBoxLib
{
	public interface IVirtualCollection 
	{
		
		object GetItem(int Index);
	}
}
