using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using Pageflex.Interfaces.Storefront;

namespace MyPickList
{
	public class MyPickList : StorefrontExtension
	{
        // The MyPickList extension is an example of providing pick list data
        // via an extension.
        //
        public override string UniqueName
        {
            // old-style GUID-based UniqueName.
            //
            get { return "MyPickList--5C2B0EC8-3D26-4462-8503-09849922FEF9"; }
        }

        public override string DisplayName
        {
            get { return "MyPickList"; }
        }

        static PickListItem MakeItem(string n, string v, bool sel)
        {
            PickListItem pit = new PickListItem();
            pit.Name = n;
            pit.Value  = v;
            pit.IsSelected = sel;
            return pit;
        }

        public override int EnumeratePickLists(out string[] lists)
        {
            lists = new string[]{ "Flowers", "Animals" };
            return eSuccess;
        }

        public override int GetPickListData(string listname, string fieldType, string objectId, bool forDisplayOnly, bool forUserSite, out PickListItem[] items)
        {
            ArrayList picklist = new ArrayList();
            items = null;
            if (listname == "Flowers")
            {
                picklist.Add(MakeItem("Rose",    "1", false));
                picklist.Add(MakeItem("Tulip",   "2", false));
                picklist.Add(MakeItem("Daisy",   "3", true));
                picklist.Add(MakeItem("Forget-me-not",   "4", false));
            }
            else if (listname == "Animals")
            {
                picklist.Add(MakeItem("Dog", "dog", false));
                picklist.Add(MakeItem("Cat", "cat", false));
            }
            else
                return eFailure;

            items = (PickListItem[])picklist.ToArray(typeof(PickListItem));
            return eSuccess;
        }


	}
}
