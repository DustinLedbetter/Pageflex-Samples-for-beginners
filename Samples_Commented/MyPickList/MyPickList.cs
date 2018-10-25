using System.Collections;
using Pageflex.Interfaces.Storefront;

namespace MyPickList
{

    /// <summary>
    /// The MyPickList extension is an example of providing pick list data via an extension.
    /// </summary>


    public class MyPickList : StorefrontExtension
	{

        #region At minimum an extension must ovveride these two methods

        // These methods set the display and unique names on the extension page in the storefront
        public override string UniqueName
        {
            // old-style GUID-based UniqueName.
            get { return "MyPickList--5C2B0EC8-3D26-4462-8503-09849922FEF9"; }
        }

        public override string DisplayName
        {
            get { return "MyPickList"; }
        }

        #endregion


        #region This section is for a method used to make an item when information is passed to it

        static PickListItem MakeItem(string n, string v, bool sel)
        {
            PickListItem pit = new PickListItem
            {
                Name = n,
                Value = v,
                IsSelected = sel
            };
            return pit;
        }

        #endregion


        #region This section is used to tell the names of pick lists this module can supply data for.

        public override int EnumeratePickLists(out string[] lists)
        {
            // This module can enumerate flowers and animals data
            lists = new string[]{ "Flowers", "Animals" };
            return eSuccess;
        }

        #endregion


        #region Gets the pick list items for the given list: "Flowers" or the list "Animals".

        public override int GetPickListData(string listname, string fieldType, string objectId, bool forDisplayOnly, bool forUserSite, out PickListItem[] items)
        {

            // Create an array list to hold items
            ArrayList picklist = new ArrayList();

            // Set items to be null at first
            items = null;

            // Check to see if the listname is "Flowers"
            if (listname == "Flowers")
            {
                // Add a type of flower to the list using the makeItem method
                picklist.Add(MakeItem("Rose",    "1", false));
                picklist.Add(MakeItem("Tulip",   "2", false));
                picklist.Add(MakeItem("Daisy",   "3", true));
                picklist.Add(MakeItem("Forget-me-not",   "4", false));
            }
            // Check to see if the listname is "Animals"
            else if (listname == "Animals")
            {
                // Add a type of animal to the list using the makeItem method
                picklist.Add(MakeItem("Dog", "dog", false));
                picklist.Add(MakeItem("Cat", "cat", false));
            }
            else
                // Return that nothing was added
                return eFailure;

            // Add the list to the "items" list to return to the storefront
            items = (PickListItem[])picklist.ToArray(typeof(PickListItem));

            // Return eSuccess to indicate that the pick list items have been set.
            return eSuccess;
        }

        #endregion

    }
}
